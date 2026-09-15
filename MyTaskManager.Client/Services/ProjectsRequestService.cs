using MyTaskManager.Client.Models;
using MyTaskManager.Common.Models;
using Newtonsoft.Json;
using System.Net.Http;

namespace MyTaskManager.Client.Services
{
    public class ProjectsRequestService : CommonRequestService
    {
        private string _projectsConrollerUrl = HOST + "projects";

        public List<ProjectDto> GetAllUsers(AuthToken token)
        {
            string response = GetDataByUrl(HttpMethod.Get, _projectsConrollerUrl, token);
            List<ProjectDto> projects = JsonConvert.DeserializeObject<List<ProjectDto>>(response);
            return projects;
        }
    }
}
