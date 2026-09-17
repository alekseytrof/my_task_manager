using MyTaskManager.Client.Models;
using MyTaskManager.Common.Models;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;

namespace MyTaskManager.Client.Services
{
    public class TasksRequestService : CommonRequestService
    {
        private string _tasksConrollerUrl = HOST + "tasks";

        public List<TaskDto> GetAllTasks(AuthToken token)
        {
            string response = GetDataByUrl(HttpMethod.Get, _tasksConrollerUrl + "/user", token);
            List<TaskDto> tasks = JsonConvert.DeserializeObject<List<TaskDto>>(response);
            return tasks;
        }

        public TaskDto GetTaskById(AuthToken token, int taskId)
        {
            var result = GetDataByUrl(HttpMethod.Get, _tasksConrollerUrl + $"/{taskId}", token);
            TaskDto task = JsonConvert.DeserializeObject<TaskDto>(result);
            return task;
        }

        public List<TaskDto> GetTaskByDesk(AuthToken token, int deskId)
        {
            var parameters = new Dictionary<string, string>();
            parameters.Add("deskId", deskId.ToString());
            var response = GetDataByUrl(HttpMethod.Get, _tasksConrollerUrl, token, null, null, parameters);
            List<TaskDto> tasks = JsonConvert.DeserializeObject<List<TaskDto>>(response);
            return tasks;
        }

        public HttpStatusCode CreateTask(AuthToken token, TaskDto task)
        {
            string taskJson = JsonConvert.SerializeObject(task);
            return SendDataByUrl(HttpMethod.Post, _tasksConrollerUrl, token, taskJson);
        }

        public HttpStatusCode UpdateTask(AuthToken token, TaskDto task)
        {
            string taskJson = JsonConvert.SerializeObject(task);
            return SendDataByUrl(HttpMethod.Patch, _tasksConrollerUrl + $"/{task.Id}", token, taskJson);
        }

        public HttpStatusCode DeleteTaskById(AuthToken token, int taskId)
        {
            return DeleteDataByUrl(_tasksConrollerUrl + $"/{taskId}", token);
        }
    }
}
