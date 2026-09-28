using MyTaskManager.Client.Models;
using MyTaskManager.Client.Services;
using MyTaskManager.Client.Views.Components;
using MyTaskManager.Client.Views.Pages;
using MyTaskManager.Common.Models;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MyTaskManager.Client.ViewModels
{
    public class DeskTasksPageViewModel : BindableBase
    {
        private AuthToken _authToken;
        private DeskDto _desk;
        private UsersRequestService _usersRequestService;
        private TasksRequestService _tasksRequestService;
        private CommonViewService _viewService;

        private DeskTasksPage _page;

        public DeskTasksPageViewModel(AuthToken authToken, DeskDto desk, DeskTasksPage page)
        {
            _authToken = authToken;
            _desk = desk;
            _viewService = new CommonViewService();
            _usersRequestService = new UsersRequestService();
            _tasksRequestService = new TasksRequestService();
            _page = page;

            TaskByColumns = GetTasksByColumns(_desk.Id);
            _page.TasksGrid.Children.Add(CreateTasksGrid());
        }


        #region
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
        #endregion

        #region METHODS
        private Dictionary<string, List<TaskClient>> GetTasksByColumns(int deskId)
        {
            var tasksByColumns = new Dictionary<string, List<TaskClient>>();
            var allTasks = _tasksRequestService.GetTaskByDesk(_authToken, deskId);
            foreach (var column in _desk.Columns)
            {
                tasksByColumns.Add(column, allTasks
                    .Where(t => t.Column == column)
                    .Select(t => new TaskClient(t))
                    .ToList());
            }
            return tasksByColumns;
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

                var taskViews = new List<TaskControl>();

                foreach (var task in column.Value)
                {
                    var taskView = new TaskControl(task);
                    taskViews.Add(taskView);
                }

                columnControl.ItemsSource = taskViews;
                grid.Children.Add(columnControl);

                columnCount++;
            }

            return grid;
        }
        #endregion
    }
}
