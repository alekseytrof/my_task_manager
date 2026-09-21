using MyTaskManager.Client.Models;
using MyTaskManager.Client.Services;
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

        #endregion

        public ProjectsPageViewModel(AuthToken token)
        {
            _commonViewService = new CommonViewService();
            _usersRequestService = new UsersRequestService();
            _projectsRequestService = new ProjectsRequestService();

            _token = token;

            OpenNewProjectCommand = new DelegateCommand(OpenNewProject);
            OpenUpdateProjectCommand = new DelegateCommand<object>(UpdateNewProject);
            ShowProjectInfoCommand = new DelegateCommand<object>(ShowProjectInfo);
        }

        #region PROPERTIES
        public List<ModelClient<ProjectDto>> UserProjects
        {
            get => _projectsRequestService.GetAllProjects(_token).Select(project => new ModelClient<ProjectDto>(project)).ToList();
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
            _commonViewService.ShowMessage("11");
        }

        private void UpdateNewProject(object projectId)
        {
            SelectedProject = GetProjectClientById(projectId);
            _commonViewService.ShowMessage("21");
        }

        private void ShowProjectInfo(object projectId)
        {
            SelectedProject = GetProjectClientById(projectId);
            _commonViewService.ShowMessage("31");
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
        #endregion
    }
}
