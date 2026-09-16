using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyTaskManager.Client.Models;
using MyTaskManager.Client.Services;
using MyTaskManager.Common.Models;
using Newtonsoft.Json;
using System.Net;

namespace MyTaskManager.Client.Services.Tests
{
    [TestClass()]
    public class ProjectsRequestServiceTests
    {
        private AuthToken _token;
        private ProjectsRequestService _service;

        public ProjectsRequestServiceTests()
        {
            _token = new UsersRequestService().GetToken("admin@mail.ru", "qwerty@#$tr55ofimov!");
            _service = new ProjectsRequestService();
        }

        [TestMethod()]
        public void GetAllProjectsTest()
        {
            var projects = _service.GetAllProjects(_token);

            var projectsJson = JsonConvert.SerializeObject(projects, Formatting.Indented);
            Console.WriteLine(projectsJson);

            Assert.AreNotEqual(Array.Empty<ProjectDto>(), projects.ToArray());
        }

        [TestMethod()]
        public void GetProjectByIdTest()
        {
            var project = _service.GetProjectById(_token, 12);

            var projectJson = JsonConvert.SerializeObject(project, Formatting.Indented);
            Console.WriteLine(projectJson);

            Assert.AreNotEqual(null, project);
        }

        [TestMethod()]
        public void CreateProjectTest()
        {
            ProjectDto newProject = new ProjectDto("Desk", "Superrr", ProjectStatus.InProgress, 1);

            var result = _service.CreateProject(_token, newProject);

            Assert.AreEqual(HttpStatusCode.OK, result);
        }

        [TestMethod()]
        public void UpdateProjectTest()
        {
            ProjectDto project = new ProjectDto("Desk Update", "Superrr update", ProjectStatus.Suspended, 1);
            project.Id = 14;

            var result = _service.UpdateProject(_token, project);

            Assert.AreEqual(HttpStatusCode.OK, result);
        }

        [TestMethod()]
        public void DeleteProjectTest()
        {
            var result = _service.DeleteProject(_token, 14);

            Assert.AreEqual(HttpStatusCode.OK, result);
        }

        [TestMethod()]
        public void AddUsersToProjectTest()
        {
            var result = _service.AddUsersToProject(_token, 13, new List<int>() { 30, 31 });

            Assert.AreEqual(HttpStatusCode.OK, result);
        }

        [TestMethod()]
        public void RemoveUsersFromProjectTest()
        {
            var result = _service.RemoveUsersFromProject(_token, 13, new List<int>() { 31 });

            Assert.AreEqual(HttpStatusCode.OK, result);
        }
    }
}