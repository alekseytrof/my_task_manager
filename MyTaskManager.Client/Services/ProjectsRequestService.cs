using MyTaskManager.Client.Models;
using MyTaskManager.Common.Models;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;

namespace MyTaskManager.Client.Services
{
    public class ProjectsRequestService : CommonRequestService
    {
        private string _projectsConrollerUrl = HOST + "projects";

        public List<ProjectDto> GetAllProjects(AuthToken token)
        {
            string response = GetDataByUrl(HttpMethod.Get, _projectsConrollerUrl, token);
            List<ProjectDto> projects = JsonConvert.DeserializeObject<List<ProjectDto>>(response);
            return projects;
        }

        public ProjectDto GetProjectById(AuthToken token, int projectId)
        {
            var result = GetDataByUrl(HttpMethod.Get, _projectsConrollerUrl + $"/{projectId}", token);
            ProjectDto project = JsonConvert.DeserializeObject<ProjectDto>(result);
            return project;
        }

        public HttpStatusCode CreateProject(AuthToken token, ProjectDto project)
        {
            string projectJson = JsonConvert.SerializeObject(project);
            return SendDataByUrl(HttpMethod.Post, _projectsConrollerUrl, token, projectJson);
        }

        public HttpStatusCode UpdateProject(AuthToken token, ProjectDto project)
        {
            string projectJson = JsonConvert.SerializeObject(project);
            return SendDataByUrl(HttpMethod.Patch, _projectsConrollerUrl + $"/{project.Id}", token, projectJson);
        }

        public HttpStatusCode DeleteProject(AuthToken token, int projectId)
        {
            return DeleteDataByUrl(_projectsConrollerUrl + $"/{projectId}", token);
        }

        public HttpStatusCode AddUsersToProject(AuthToken token, int projectId, List<int> usersIds)
        {
            string usersIdsJson = JsonConvert.SerializeObject(usersIds);
            var result = SendDataByUrl(HttpMethod.Patch, _projectsConrollerUrl + $"/{projectId}/users", token, usersIdsJson);
            return result;
        }

        public HttpStatusCode RemoveUsersFromProject(AuthToken token, int projectId, List<int> usersIds)
        {
            string usersIdsJson = JsonConvert.SerializeObject(usersIds);
            var result = SendDataByUrl(HttpMethod.Patch, _projectsConrollerUrl + $"/{projectId}/users/remove", token, usersIdsJson);
            return result;
        }
    }
}
