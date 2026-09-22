using MyTaskManager.Client.Models;
using MyTaskManager.Client.Services;
using MyTaskManager.Client.Views.AddWindows;
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
        private CommonViewService _commonViewService;

        #region COMMANDS
        public DelegateCommand OpenNewProjectCommand { get; private set; }
        public DelegateCommand<object> OpenUpdateProjectCommand { get; private set; }
        public DelegateCommand<object> ShowProjectInfoCommand { get; private set; }
        public DelegateCommand CreateOrUpdateProjectCommand { get; private set; }
        public DelegateCommand DeleteProjectCommand { get; private set; }
        public DelegateCommand SelectPhotoForProjectCommand { get; private set; }

        #endregion

        public ProjectsPageViewModel(AuthToken token)
        {
            _commonViewService = new CommonViewService();
            _usersRequestService = new UsersRequestService();
            _projectsRequestService = new ProjectsRequestService();

            _token = token;

            UserProjects = GetProjectsToClient();

            OpenNewProjectCommand = new DelegateCommand(OpenNewProject);
            OpenUpdateProjectCommand = new DelegateCommand<object>(UpdateNewProject);
            ShowProjectInfoCommand = new DelegateCommand<object>(ShowProjectInfo);
            CreateOrUpdateProjectCommand = new DelegateCommand(CreateOrUpdateProject);
            DeleteProjectCommand = new DelegateCommand(DeleteProject);
            SelectPhotoForProjectCommand = new DelegateCommand(SelectPhotoForProject);
        }

        #region PROPERTIES
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

                if (SelectedProject.Model.AllUsersIds != null && SelectedProject.Model.AllUsersIds.Count > 0)
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
        #endregion

        #region METHODS
        private void OpenNewProject()
        {
            ClientAction = ClientAction.Create;
            SelectedProject = new ModelClient<ProjectDto>(new ProjectDto());
            var wnd = new CreateOrUpdateProjectWindow();
            _commonViewService.OpenWindow(wnd, this);
        }

        private void UpdateNewProject(object projectId)
        {
            SelectedProject = GetProjectClientById(projectId);

            ClientAction = ClientAction.Update;
            var wnd = new CreateOrUpdateProjectWindow();
            _commonViewService.OpenWindow(wnd, this);
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
            UserProjects = GetProjectsToClient();
        }

        private void CreateProject()
        {
            var resultAction = _projectsRequestService.CreateProject(_token, SelectedProject.Model);
            _commonViewService.ShowActionResult(resultAction, "New Project is created");
            _commonViewService.CurrentOpenedWindow?.Close();
        }

        private void UpdateProject()
        {
            var resultAction = _projectsRequestService.UpdateProject(_token, SelectedProject.Model);
            _commonViewService.ShowActionResult(resultAction, "New Project is updated");
            _commonViewService.CurrentOpenedWindow?.Close();
        }

        private void DeleteProject()
        {
            var resultAction = _projectsRequestService.DeleteProject(_token, SelectedProject.Model.Id);
            _commonViewService.ShowActionResult(resultAction, "New Project is deleted");
            UserProjects = GetProjectsToClient();
            _commonViewService.CurrentOpenedWindow?.Close();
        }

        private List<ModelClient<ProjectDto>> GetProjectsToClient()
        {
            return _projectsRequestService.GetAllProjects(_token).Select(project => new ModelClient<ProjectDto>(project)).ToList();
        }

        private void SelectPhotoForProject()
        {
            _commonViewService.SetPhotoForObject(SelectedProject.Model);
            SelectedProject = new ModelClient<ProjectDto>(SelectedProject.Model);
        }
        #endregion
    }
}
