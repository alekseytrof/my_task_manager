using MyTaskManager.Client.Models;
using MyTaskManager.Client.Services;
using MyTaskManager.Client.Views.AddWindows;
using MyTaskManager.Client.Views.Components;
using MyTaskManager.Client.Views.Pages;
using MyTaskManager.Common.Models;
using Prism.Commands;
using Prism.Mvvm;
using System.Windows;
using System.Windows.Controls;

namespace MyTaskManager.Client.ViewModels
{
    public class DeskTasksPageViewModel : BindableBase
    {
        private AuthToken _token;
        private DeskDto _desk;
        private UsersRequestService _usersRequestService;
        private TasksRequestService _tasksRequestService;
        private ProjectsRequestService _projectsRequestService;
        private CommonViewService _viewService;
        private DeskTasksPage _page;

        #region COMMAND
        public DelegateCommand OpenNewTaskCommand { get; private set; }
        public DelegateCommand OpenUpdateTaskCommand { get; private set; }
        public DelegateCommand CreateOrUpdateTaskCommand { get; private set; }
        public DelegateCommand DeleteTaskCommand { get; private set; }
        #endregion

        public DeskTasksPageViewModel(AuthToken authToken, DeskDto desk, DeskTasksPage page)
        {
            _token = authToken;
            _desk = desk;
            _viewService = new CommonViewService();
            _usersRequestService = new UsersRequestService();
            _tasksRequestService = new TasksRequestService();
            _projectsRequestService = new ProjectsRequestService();
            _page = page;

            TaskByColumns = GetTasksByColumns(_desk.Id);
            _page.TasksGrid.Children.Add(CreateTasksGrid());
            OpenNewTaskCommand = new DelegateCommand(OpenNewTask);
            OpenUpdateTaskCommand = new DelegateCommand(OpenUpdateTask);
            CreateOrUpdateTaskCommand = new DelegateCommand(CreateOrUpdateTask);
            DeleteTaskCommand = new DelegateCommand(DeleteTask);
        }


        #region PROPERTIES
        private Dictionary<string, List<TaskClient>> _taskByColumns = new Dictionary<string, List<TaskClient>>();
        public Dictionary<string, List<TaskClient>> TaskByColumns
        {
            get => _taskByColumns;
            set
            {
                _taskByColumns = value;
                RaisePropertyChanged(nameof(TaskByColumns));
            }
        }

        private TaskClient _selectedTask;
        public TaskClient SelectedTask
        {
            get => _selectedTask;
            set
            {
                _selectedTask = value;
                RaisePropertyChanged(nameof(SelectedTask));
            }
        }

        private ClientAction _clientAction;
        public ClientAction ClientAction
        {
            get => _clientAction;
            set
            {
                _clientAction = value;
                RaisePropertyChanged(nameof(ClientAction));
            }
        }

        private UserDto _selectedTaskExecutor;
        public UserDto SelectedTaskExecutor
        {
            get => _selectedTaskExecutor;
            set
            {
                _selectedTaskExecutor = value;
                RaisePropertyChanged(nameof(SelectedTaskExecutor));
            }
        }

        private ProjectDto Project
        {
            get => _projectsRequestService.GetProjectById(_token, _desk.ProjectId);
        }

        public List<UserDto> AllProjectUsers
        {
            get => Project?.AllUsersIds?.Select(userId => _usersRequestService.GetUserById(_token, userId)).ToList();
        }

        private string _selectedColumnName;
        public string SelectedColumnName
        {
            get => _selectedColumnName;
            set
            {
                _selectedColumnName = value;
                RaisePropertyChanged(nameof(SelectedColumnName));
            }
        }

        #endregion

        #region METHODS
        private Dictionary<string, List<TaskClient>> GetTasksByColumns(int deskId)
        {
            var tasksByColumns = new Dictionary<string, List<TaskClient>>();
            var allTasks = _tasksRequestService.GetTaskByDesk(_token, deskId);
            foreach (var column in _desk.Columns)
            {
                tasksByColumns.Add(column, allTasks
                    .Where(t => t.Column == column)
                    .Select(t =>
                    {
                        var tV = new TaskClient(t);
                        tV.Creator = _usersRequestService.GetCurrentUser(_token);
                        if (t.ExecutorId != null)
                        {
                            tV.Executor = _usersRequestService.GetUserById(_token, (int)t.ExecutorId);
                        }
                        return tV;
                    }
            ).ToList());
            }
            return tasksByColumns;
        }

        private void CreateOrUpdateTask()
        {
            if (ClientAction == ClientAction.Create)
            {
                CreateTask();
            }
            if (ClientAction == ClientAction.Update)
            {
                UpdateTask();
            }
            UpdatePage();
        }

        private void CreateTask()
        {
            SelectedTask.Model.DeskId = _desk.Id;
            SelectedTask.Model.ExecutorId = SelectedTaskExecutor.Id;
            SelectedTask.Model.Column = _desk.Columns.FirstOrDefault();

            var resultAction = _tasksRequestService.CreateTask(_token, SelectedTask.Model);
            _viewService.ShowActionResult(resultAction, "New Task is created");
        }

        private void UpdateTask()
        {
            _tasksRequestService.UpdateTask(_token, SelectedTask.Model);
        }

        private void DeleteTask()
        {
            _tasksRequestService.DeleteTask(_token, SelectedTask.Model.Id);
            UpdatePage();
        }

        private void UpdatePage()
        {
            SelectedTask = null;
            TaskByColumns = GetTasksByColumns(_desk.Id);
            _page.TasksGrid.Children.Add(CreateTasksGrid());
            _viewService.CurrentOpenedWindow?.Close();
        }

        private void OpenNewTask()
        {
            ClientAction = ClientAction.Create;

            SelectedTask = new TaskClient(new TaskDto());

            var wnd = new CreateOrUpdateTaskWindow();
            _viewService.OpenWindow(wnd, this);
        }

        private void OpenUpdateTask()
        {
            ClientAction = ClientAction.Update;
            var wnd = new CreateOrUpdateTaskWindow();
            _viewService.OpenWindow(wnd, this);
        }

        private Grid CreateTasksGrid()
        {
            ResourceDictionary resource = new ResourceDictionary();
            resource.Source = new Uri("./Resources/Styles/MainStyles.xaml", UriKind.Relative);

            Grid grid = new Grid();
            var row0 = new RowDefinition();
            row0.Height = new GridLength(30);

            var row1 = new RowDefinition();

            grid.RowDefinitions.Add(row0);
            grid.RowDefinitions.Add(row1);

            int columnCount = 0;
            foreach (var column in TaskByColumns)
            {
                var col = new ColumnDefinition();
                grid.ColumnDefinitions.Add(col);

                //header
                TextBlock header = new TextBlock();
                header.Text = column.Key;
                header.Style = resource["headerTBlock"] as Style;

                Grid.SetRow(header, 0);
                Grid.SetColumn(header, columnCount);

                grid.Children.Add(header);

                //column
                ItemsControl columnControl = new ItemsControl();
                Grid.SetRow(columnControl, 1);
                Grid.SetColumn(columnControl, columnCount);
                columnControl.Style = resource["tasksColumnPanel"] as Style;
                columnControl.Tag = column.Key;

                columnControl.MouseEnter += new System.Windows.Input.MouseEventHandler((sender, e) =>
                {
                    GetSelectedColumn(sender);
                });
                columnControl.MouseLeftButtonUp += new System.Windows.Input.MouseButtonEventHandler((sender, e) =>
                {
                    SendTaskToNewColumn();
                });

                var taskViews = new List<TaskControl>();

                foreach (var task in column.Value)
                {
                    var taskView = new TaskControl(task);
                    taskView.MouseDown += new System.Windows.Input.MouseButtonEventHandler((sender, e) =>
                    {
                        SelectedTask = task;
                    });
                    taskViews.Add(taskView);
                }

                columnControl.ItemsSource = taskViews;
                grid.Children.Add(columnControl);

                columnCount++;
            }
            return grid;
        }

        private void GetSelectedColumn(object senderControl)
        {
            SelectedColumnName = ((ItemsControl)senderControl).Tag.ToString();
        }

        private void SendTaskToNewColumn()
        {
            if (SelectedTask != null && SelectedTask.Model?.Column != SelectedColumnName)
            {
                SelectedTask.Model.Column = SelectedColumnName;
                _tasksRequestService.UpdateTask(_token, SelectedTask.Model);
                UpdatePage();
                SelectedTask = null;
            }
        }
        #endregion
    }
}
