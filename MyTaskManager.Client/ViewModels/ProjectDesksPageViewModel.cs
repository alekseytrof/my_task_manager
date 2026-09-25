using MyTaskManager.Client.Models;
using MyTaskManager.Client.Services;
using MyTaskManager.Client.Views.AddWindows;
using MyTaskManager.Common.Models;
using Prism.Commands;
using Prism.Mvvm;
using System.Collections.ObjectModel;

namespace MyTaskManager.Client.ViewModels
{
    public class ProjectDesksPageViewModel : BindableBase
    {
        private CommonViewService _viewService;
        private DesksRequestService _desksRequestService;
        private UsersRequestService _usersRequestService;
        private DeskViewService _deskViewService;

        #region COMMANDS
        public DelegateCommand OpenNewDeskCommand { get; private set; }
        public DelegateCommand<object> OpenUpdateDeskCommand { get; private set; }
        public DelegateCommand CreateOrUpdateDeskCommand { get; private set; }
        public DelegateCommand DeleteDeskCommand { get; private set; }
        public DelegateCommand SelectPhotoForDeskCommand { get; private set; }
        public DelegateCommand AddNewColumnItemCommand { get; private set; }
        public DelegateCommand<object> RemoveColumnItemCommand { get; private set; }

        #endregion

        public ProjectDesksPageViewModel(AuthToken token, ProjectDto project)
        {
            Token = token;
            Project = project;
            _viewService = new CommonViewService();
            _desksRequestService = new DesksRequestService();
            _usersRequestService = new UsersRequestService();
            _deskViewService = new DeskViewService(Token, _desksRequestService, _viewService);
            ProjectDesks = _deskViewService.GetDesks(project.Id);

            UpdatePage();

            OpenNewDeskCommand = new DelegateCommand(OpenNewDesk);
            OpenUpdateDeskCommand = new DelegateCommand<object>(OpenUpdateDesk);
            CreateOrUpdateDeskCommand = new DelegateCommand(CreateOrUpdateDesk);
            DeleteDeskCommand = new DelegateCommand(DeleteDesk);
            SelectPhotoForDeskCommand = new DelegateCommand(SelectPhotoForDesk);
            AddNewColumnItemCommand = new DelegateCommand(AddNewColumnItem);
            RemoveColumnItemCommand = new DelegateCommand<object>(RemoveColumnItem);
        }

        #region PROPERTIES
        public UserDto CurrentUser
        {
            get => _usersRequestService.GetCurrentUser(_token);
        }

        private ObservableCollection<ColumnBindingHelp> _columnsForNewDesk = new ObservableCollection<ColumnBindingHelp>()
        {
            new ColumnBindingHelp("New"),
            new ColumnBindingHelp("In Progress"),
            new ColumnBindingHelp("In Review"),
            new ColumnBindingHelp("Done"),
        };
        public ObservableCollection<ColumnBindingHelp> ColumnsForNewDesk
        {
            get => _columnsForNewDesk;
            set
            {
                _columnsForNewDesk = value;
                RaisePropertyChanged(nameof(ColumnsForNewDesk));
            }
        }

        private AuthToken _token;
        public AuthToken Token
        {
            get => _token;
            private set
            {
                _token = value;
                RaisePropertyChanged(nameof(Token));
            }
        }

        private ProjectDto _project;
        public ProjectDto Project
        {
            get => _project;
            private set
            {
                _project = value;
                RaisePropertyChanged(nameof(Project));
            }
        }

        private List<ModelClient<DeskDto>> _projectDesks;
        public List<ModelClient<DeskDto>> ProjectDesks
        {
            get => _projectDesks;
            set
            {
                _projectDesks = value;
                RaisePropertyChanged(nameof(ProjectDesks));
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

        private ModelClient<DeskDto> _selectedDesk;
        public ModelClient<DeskDto> SelectedDesk
        {
            get => _selectedDesk;
            set
            {
                _selectedDesk = value;
                RaisePropertyChanged(nameof(SelectedDesk));
            }
        }
        #endregion

        #region METHODS

        private void OpenNewDesk()
        {
            ClientAction = ClientAction.Create;
            SelectedDesk = new ModelClient<DeskDto>(new DeskDto());
            var wnd = new CreateOrUpdateDeskWindow();
            _viewService.OpenWindow(wnd, this);
        }

        private void CreateOrUpdateDesk()
        {
            if (ClientAction == ClientAction.Create)
            {
                CreateDesk();
            }
            if (ClientAction == ClientAction.Update)
            {
                UpdateDesk();
            }
            UpdatePage();
        }

        private void CreateDesk()
        {
            SelectedDesk.Model.Columns = ColumnsForNewDesk.Select(c => c.Value).ToArray();
            SelectedDesk.Model.ProjectId = Project.Id;

            var resultAction = _desksRequestService.CreateDesk(_token, SelectedDesk.Model);
            _viewService.ShowActionResult(resultAction, "New Desk is created");
        }

        private void UpdateDesk()
        {
            SelectedDesk.Model.Columns = ColumnsForNewDesk.Select(c => c.Value).ToArray();
            _deskViewService.UpdateDesk(SelectedDesk.Model);
        }

        private void DeleteDesk()
        {
            _deskViewService.DeleteDesk(SelectedDesk.Model.Id);
            UpdatePage();
        }

        private void AddNewColumnItem()
        {
            ColumnsForNewDesk.Add(new ColumnBindingHelp("Column"));
        }

        private void RemoveColumnItem(object item)
        {
            if (item is ColumnBindingHelp column)
            {
                ColumnsForNewDesk.Remove(column);
            }
        }

        private void SelectPhotoForDesk()
        {
            _deskViewService.SelectPhotoForDesk(SelectedDesk);
        }

        private void UpdatePage()
        {
            SelectedDesk = null;
            ProjectDesks = _deskViewService.GetDesks(_project.Id);
            _viewService.CurrentOpenedWindow?.Close();
        }

        private void OpenUpdateDesk(object deskId)
        {
            SelectedDesk = _deskViewService.GetDeskClientById(deskId);

            if (CurrentUser.Id != SelectedDesk.Model.AdminId)
            {
                _viewService.ShowMessage("You are not admin");
                return;
            }

            ClientAction = ClientAction.Update;
            ColumnsForNewDesk = new ObservableCollection<ColumnBindingHelp>(SelectedDesk.Model.Columns.Select(c => new ColumnBindingHelp(c)));

            _deskViewService.OpenViewDeskInfo(deskId, this);
        }
        #endregion
    }
}
