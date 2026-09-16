using MyTaskManager.Client.Models;
using MyTaskManager.Common.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MyTaskManager.Client.Services
{
    public class DesksRequestService : CommonRequestService
    {
        private string _desksConrollerUrl = HOST + "desks";

        public List<DeskDto> GetAllDesks(AuthToken token)
        {
            string response = GetDataByUrl(HttpMethod.Get, _desksConrollerUrl, token);
            List<DeskDto> desks = JsonConvert.DeserializeObject<List<DeskDto>>(response);
            return desks;
        }

        public DeskDto GetDeskById(AuthToken token, int deskId)
        {
            var result = GetDataByUrl(HttpMethod.Get, _desksConrollerUrl + $"/{deskId}", token);
            DeskDto desk = JsonConvert.DeserializeObject<DeskDto>(result);
            return desk;
        }

        public List<DeskDto> GetDeskByProject(AuthToken token, int projectId)
        {
            var parameters = new Dictionary<string, string>();
            parameters.Add("projectId", projectId.ToString());
            var response = GetDataByUrl(HttpMethod.Get, _desksConrollerUrl + "/project", token, null, null, parameters);
            List<DeskDto> desks = JsonConvert.DeserializeObject<List<DeskDto>>(response);
            return desks;
        }

        public HttpStatusCode CreateDesk(AuthToken token, DeskDto desk)
        {
            string deskJson = JsonConvert.SerializeObject(desk);
            return SendDataByUrl(HttpMethod.Post, _desksConrollerUrl, token, deskJson);
        }

        public HttpStatusCode UpdateDesk(AuthToken token, DeskDto desk)
        {
            string deskJson = JsonConvert.SerializeObject(desk);
            return SendDataByUrl(HttpMethod.Patch, _desksConrollerUrl + $"/{desk.Id}", token, deskJson);
        }

        public HttpStatusCode DeleteDeskById(AuthToken token, int deskId)
        {
            return DeleteDataByUrl(_desksConrollerUrl + $"/{deskId}", token);
        }
    }
}
