using MyTaskManager.Client.Models;
using MyTaskManager.Client.Services;
using MyTaskManager.Client.Views.AddWindows;
using MyTaskManager.Client.Views.Pages;
using MyTaskManager.Common.Models;
using Prism.Commands;
using Prism.Mvvm;

namespace MyTaskManager.Client.ViewModels
{
    public class ProjectsPageViewModel : BindableBase
    {
        private AuthToken _token;
        private UsersRequestService _usersRequestService;
        private ProjectsRequestService _projectsRequestService;
        private CommonViewService _viewService;
        private MainWindowViewModel _mainWindowViewModel;

        #region COMMANDS
        public DelegateCommand OpenNewProjectCommand { get; private set; }
        public DelegateCommand<object> OpenUpdateProjectCommand { get; private set; }
        public DelegateCommand<object> ShowProjectInfoCommand { get; private set; }
        public DelegateCommand CreateOrUpdateProjectCommand { get; private set; }
        public DelegateCommand DeleteProjectCommand { get; private set; }
        public DelegateCommand SelectPhotoForProjectCommand { get; private set; }
        public DelegateCommand AddUsersToProjectCommand { get; private set; }
        public DelegateCommand OpenUsersToProjectCommand { get; private set; }
        public DelegateCommand<object> DeleteUserFromProjectCommand { get; private set; }
        public DelegateCommand OpenProjectDesksPageCommand { get; private set; }

        #endregion

        public ProjectsPageViewModel(AuthToken token, MainWindowViewModel mainWindowViewModel)
        {
            _viewService = new CommonViewService();
            _usersRequestService = new UsersRequestService();
            _projectsRequestService = new ProjectsRequestService();
            _mainWindowViewModel = mainWindowViewModel;

            _token = token;

            UpdatePage();

            OpenNewProjectCommand = new DelegateCommand(OpenNewProject);
            OpenUpdateProjectCommand = new DelegateCommand<object>(UpdateNewProject);
            ShowProjectInfoCommand = new DelegateCommand<object>(ShowProjectInfo);
            CreateOrUpdateProjectCommand = new DelegateCommand(CreateOrUpdateProject);
            DeleteProjectCommand = new DelegateCommand(DeleteProject);
            SelectPhotoForProjectCommand = new DelegateCommand(SelectPhotoForProject);
            AddUsersToProjectCommand = new DelegateCommand(AddUsersToProject);
            OpenUsersToProjectCommand = new DelegateCommand(OpenUsersToProject);
            OpenProjectDesksPageCommand = new DelegateCommand(OpenProjectDesksPage);
            DeleteUserFromProjectCommand = new DelegateCommand<object>(DeleteUserFromProject);
            _mainWindowViewModel = mainWindowViewModel;
        }

        #region PROPERTIES
        public UserDto CurrentUser
        {
            get => _usersRequestService.GetCurrentUser(_token);
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

        private List<ModelClient<ProjectDto>> _userProjects;
        public List<ModelClient<ProjectDto>> UserProjects
        {
            get => _userProjects;
            set
            {
                _userProjects = value;
                RaisePropertyChanged(nameof(UserProjects));
            }
        }

        private ModelClient<ProjectDto> _selectedProject;
        public ModelClient<ProjectDto> SelectedProject
        {
            get => _selectedProject;
            set
            {
                _selectedProject = value;
                RaisePropertyChanged(nameof(SelectedProject));

                if (SelectedProject?.Model.AllUsersIds != null && SelectedProject?.Model.AllUsersIds.Count > 0)
                {
                    UsersProject = SelectedProject.Model.AllUsersIds?.Select(userId => _usersRequestService.GetUserById(_token, userId)).ToList();
                }
                else
                {
                    UsersProject = new List<UserDto>();
                }
            }
        }

        private List<UserDto> _usersProject;
        public List<UserDto> UsersProject
        {
            get => _usersProject;
            set
            {
                _usersProject = value;
                RaisePropertyChanged(nameof(UsersProject));
            }
        }

        public List<UserDto> NewUsersForSelectedProject
        {
            get => _usersRequestService.GetAllUsers(_token).Where(user => UsersProject.Any(u => user.Id == u.Id) == false).ToList();
        }

        private List<UserDto> _selectedUsersForProject = new List<UserDto>();

        public List<UserDto> SelectedUsersForProject
        {
            get => _selectedUsersForProject;
            set
            {
                _selectedUsersForProject = value;
                RaisePropertyChanged(nameof(SelectedUsersForProject));
            }
        }
        private UserDto _selectedUser;
        public UserDto SelectedUser
        {
            get => _selectedUser;
            set
            {
                _selectedUser = value;
                RaisePropertyChanged(nameof(SelectedUser));
            }
        }
        #endregion

        #region METHODS
        private void OpenNewProject()
        {
            ClientAction = ClientAction.Create;
            SelectedProject = new ModelClient<ProjectDto>(new ProjectDto());
            var wnd = new CreateOrUpdateProjectWindow();
            _viewService.OpenWindow(wnd, this);
        }

        private void UpdateNewProject(object projectId)
        {
            SelectedProject = GetProjectClientById(projectId);

            ClientAction = ClientAction.Update;
            var wnd = new CreateOrUpdateProjectWindow();
            _viewService.OpenWindow(wnd, this);
        }

        private void ShowProjectInfo(object projectId)
        {
            SelectedProject = GetProjectClientById(projectId);
        }

        private ModelClient<ProjectDto> GetProjectClientById(object projectId)
        {
            try
            {
                int id = (int)projectId;
                ProjectDto project = _projectsRequestService.GetProjectById(_token, id);
                return new ModelClient<ProjectDto>(project);
            }
            catch (Exception ex)
            {
                return new ModelClient<ProjectDto>(null);
            }
        }

        private void CreateOrUpdateProject()
        {
            if (ClientAction == ClientAction.Create)
            {
                CreateProject();
            }
            if (ClientAction == ClientAction.Update)
            {
                UpdateProject();
            }
            UpdatePage();
        }

        private void CreateProject()
        {
            var resultAction = _projectsRequestService.CreateProject(_token, SelectedProject.Model);
            _viewService.ShowActionResult(resultAction, "New Project is created");
            _viewService.CurrentOpenedWindow?.Close();
        }

        private void UpdateProject()
        {
            var resultAction = _projectsRequestService.UpdateProject(_token, SelectedProject.Model);
            _viewService.ShowActionResult(resultAction, "New Project is updated");
            _viewService.CurrentOpenedWindow?.Close();
        }

        private void DeleteProject()
        {
            var resultAction = _projectsRequestService.DeleteProject(_token, SelectedProject.Model.Id);
            _viewService.ShowActionResult(resultAction, "New Project is deleted");
            UpdatePage();
            _viewService.CurrentOpenedWindow?.Close();
        }

        private List<ModelClient<ProjectDto>> GetProjectsToClient()
        {
            _viewService.CurrentOpenedWindow?.Close();
            return _projectsRequestService.GetAllProjects(_token).Select(project => new ModelClient<ProjectDto>(project)).ToList();
        }

        private void SelectPhotoForProject()
        {
            _viewService.SetPhotoForObject(SelectedProject.Model);
            SelectedProject = new ModelClient<ProjectDto>(SelectedProject.Model);
        }

        private void AddUsersToProject()
        {
            if (SelectedUsersForProject == null || SelectedUsersForProject?.Count == 0)
            {
                _viewService.ShowMessage("No selected users!");
                return;
            }

            var resultAction = _projectsRequestService.AddUsersToProject(_token, SelectedProject.Model.Id, SelectedUsersForProject.Select(user => user.Id).ToList());
            _viewService.ShowActionResult(resultAction, "New users are added to project");
            UpdatePage();
        }

        private void OpenUsersToProject()
        {
            var wnd = new AddUsersToProjectWindow();
            _viewService.OpenWindow(wnd, this);
        }

        private void UpdatePage()
        {
            UserProjects = GetProjectsToClient();
            SelectedProject = null;
            SelectedUsersForProject = new List<UserDto>();
        }

        private void DeleteUserFromProject(object paremeter)
        {
            if (paremeter is UserDto user)
            {
                SelectedUsersForProject.Add(user);
                var resultAction = _projectsRequestService
                    .RemoveUsersFromProject(_token, SelectedProject.Model.Id, SelectedUsersForProject.Select(user => user.Id).ToList());

                _viewService.ShowActionResult(resultAction, "New users are deleted to project");

                UsersProject.Remove(user);

                UpdatePage();
            }
        }

        private void OpenProjectDesksPage()
        {
            if (SelectedProject?.Model != null)
            {
                var page = new ProjectDesksPage();
                _mainWindowViewModel.OpenPage(page, $"Desks of {SelectedProject.Model.Name}", new ProjectDesksPageViewModel(_token, SelectedProject.Model));
            }
        }
        #endregion
    }
}
