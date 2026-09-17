using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyTaskManager.Client.Models;
using MyTaskManager.Client.Services;
using MyTaskManager.Common.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MyTaskManager.Client.Services.Tests
{
    [TestClass()]
    public class TasksRequestServiceTests
    {
        private AuthToken _token;
        private TasksRequestService _service;

        public TasksRequestServiceTests()
        {
            _token = new UsersRequestService().GetToken("admin@mail.ru", "qwerty@#$tr55ofimov!");
            _service = new TasksRequestService();
        }

        [TestMethod()]
        public void GetAllTasksTest()
        {
            var tasks = _service.GetAllTasks(_token);

            var tasksJson = JsonConvert.SerializeObject(tasks, Formatting.Indented);
            Console.WriteLine(tasksJson);

            Assert.AreNotEqual(0, tasks.Count);
        }

        [TestMethod()]
        public void GetTaskByIdTest()
        {
            var task = _service.GetTaskById(_token, 10);

            var taskJson = JsonConvert.SerializeObject(task, Formatting.Indented);
            Console.WriteLine(taskJson);

            Assert.AreNotEqual(null, task);
        }

        [TestMethod()]
        public void GetTaskByTaskTest()
        {
            var tasks = _service.GetTaskByDesk(_token, 5);

            var tasksJson = JsonConvert.SerializeObject(tasks, Formatting.Indented);
            Console.WriteLine(tasksJson);

            Assert.AreNotEqual(0, tasks.Count);
            Assert.AreEqual(1, tasks.Count);
        }

        [TestMethod()]
        public void CreateTaskTest()
        {
            TaskDto newTask = new TaskDto("Task 2", "Superrr task", DateTime.Now, DateTime.Now.AddDays(1), 5, "New", 16);

            var result = _service.CreateTask(_token, newTask);

            Assert.AreEqual(HttpStatusCode.OK, result);
        }

        [TestMethod()]
        public void UpdateTaskTest()
        {
            TaskDto taskForUpdate = new TaskDto("Task updated", "Superrr task updated", DateTime.Now, DateTime.Now.AddDays(2), 5, "New", 15);
            taskForUpdate.Id = 11;
            taskForUpdate.ExecutorId = 17;

            var result = _service.UpdateTask(_token, taskForUpdate);

            Assert.AreEqual(HttpStatusCode.OK, result);
        }

        [TestMethod()]
        public void DeleteTaskByIdTest()
        {
            var result = _service.DeleteTaskById(_token, 11);

            Assert.AreEqual(HttpStatusCode.OK, result);
        }
    }
}