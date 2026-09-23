using MyTaskManager.Client.Models;
using MyTaskManager.Client.Services;
using MyTaskManager.Common.Models;
using Prism.Mvvm;

namespace MyTaskManager.Client.ViewModels
{
    public class ProjectDesksPageViewModel : BindableBase
    {
        private CommonViewService _viewService;
        private DesksRequestService _desksRequestService;

        #region COMMANDS



        #endregion

        public ProjectDesksPageViewModel(AuthToken token, ProjectDto project)
        {
            Token = token;
            Project = project;
            _viewService = new CommonViewService();
            _desksRequestService = new DesksRequestService();

            ProjectDesks = GetDesks(project.Id);
        }

        #region PROPERTIES
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

        public List<ModelClient<DeskDto>> GetDesks(int projectId)
        {
            var result = new List<ModelClient<DeskDto>>();
            var desks = _desksRequestService.GetDeskByProject(Token, projectId);

            if (desks != null)
            {
                result = desks.Select(d => new ModelClient<DeskDto>(d)).ToList();
            }

            return result;
        }

        #endregion
    }
}
