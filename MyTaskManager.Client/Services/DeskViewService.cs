using MyTaskManager.Client.Models;
using MyTaskManager.Client.Views.AddWindows;
using MyTaskManager.Common.Models;
using Prism.Mvvm;

namespace MyTaskManager.Client.Services
{
    public class DeskViewService
    {
        private DesksRequestService _desksRequestService;
        private AuthToken _token;
        private CommonViewService _viewService;

        public DeskViewService(AuthToken token, DesksRequestService desksRequestService, CommonViewService viewService)
        {
            _desksRequestService = desksRequestService;
            _viewService = viewService;
            _token = token;
        }

        public ModelClient<DeskDto> GetDeskClientById(object deskId)
        {
            try
            {
                int id = (int)deskId;
                DeskDto desk = _desksRequestService.GetDeskById(_token, id);
                return new ModelClient<DeskDto>(desk);
            }
            catch (Exception ex)
            {
                return new ModelClient<DeskDto>(null);
            }
        }

        public List<ModelClient<DeskDto>> GetDesks(int projectId)
        {
            var result = new List<ModelClient<DeskDto>>();
            var desks = _desksRequestService.GetDeskByProject(_token, projectId);

            if (desks != null)
            {
                result = desks.Select(d => new ModelClient<DeskDto>(d)).ToList();
            }

            return result;
        }

        public List<ModelClient<DeskDto>> GetAllDesks()
        {
            var result = new List<ModelClient<DeskDto>>();
            var desks = _desksRequestService.GetAllDesks(_token);

            if (desks != null)
            {
                result = desks.Select(d => new ModelClient<DeskDto>(d)).ToList();
            }

            return result;
        }

        public void OpenViewDeskInfo(object deskId, BindableBase context)
        {
            var wnd = new CreateOrUpdateDeskWindow();
            _viewService.OpenWindow(wnd, context);
        }

        public void UpdateDesk(DeskDto desk)
        {
            var resultAction = _desksRequestService.UpdateDesk(_token, desk);
            _viewService.ShowActionResult(resultAction, "New Desk is updated");
        }

        public void DeleteDesk(int deskId)
        {
            var resultAction = _desksRequestService.DeleteDesk(_token, deskId);
            _viewService.ShowActionResult(resultAction, "New Desk is deleted");
        }

        public ModelClient<DeskDto> SelectPhotoForDesk(ModelClient<DeskDto> selectedDesk)
        {
            if (selectedDesk?.Model != null)
            {
                _viewService.SetPhotoForObject(selectedDesk.Model);
                selectedDesk = new ModelClient<DeskDto>(selectedDesk.Model);
            }
            return selectedDesk;
        }
    }
}
