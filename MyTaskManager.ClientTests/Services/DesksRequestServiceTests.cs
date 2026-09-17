using MyTaskManager.Client.Models;
using MyTaskManager.Common.Models;
using Newtonsoft.Json;
using System.Net;

namespace MyTaskManager.Client.Services.Tests
{
    [TestClass()]
    public class DesksRequestServiceTests
    {
        private AuthToken _token;
        private DesksRequestService _service;

        public DesksRequestServiceTests()
        {
            _token = new UsersRequestService().GetToken("admin@mail.ru", "qwerty@#$tr55ofimov!");
            _service = new DesksRequestService();
        }

        [TestMethod()]
        public void GetAllDesksTest()
        {
            var desks = _service.GetAllDesks(_token);

            var desksJson = JsonConvert.SerializeObject(desks, Formatting.Indented);
            Console.WriteLine(desksJson);

            Assert.AreNotEqual(Array.Empty<DeskDto>(), desks.ToArray());
        }

        [TestMethod()]
        public void GetDeskByIdTest()
        {
            var desk = _service.GetDeskById(_token, 5);

            var deskJson = JsonConvert.SerializeObject(desk, Formatting.Indented);
            Console.WriteLine(deskJson);

            Assert.AreNotEqual(null, desk);
        }

        [TestMethod()]
        public void GetDeskByProjectTest()
        {
            var desks = _service.GetDeskByProject(_token, 2);

            var desksJson = JsonConvert.SerializeObject(desks, Formatting.Indented);
            Console.WriteLine(desksJson);

            Assert.AreNotEqual(0, desks.Count);
            Assert.AreEqual(2, desks.Count);
        }

        [TestMethod()]
        public void CreateDeskTest()
        {
            DeskDto newDesk = new DeskDto("Desk", "Superrr desk", false, new string[] { "New", "IsProgress", "Ready" }, 1, 2);

            var result = _service.CreateDesk(_token, newDesk);

            Assert.AreEqual(HttpStatusCode.OK, result);
        }

        [TestMethod()]
        public void UpdateDeskTest()
        {
            DeskDto desk = new DeskDto("Desk updated", "Superrr desk update", true, new string[] { "New", "IsProgress", "Ready" }, 16, 2);
            desk.Id = 6;

            var result = _service.UpdateDesk(_token, desk);

            Assert.AreEqual(HttpStatusCode.OK, result);
        }

        [TestMethod()]
        public void DeleteDeskByIdTest()
        {
            var result = _service.DeleteDeskById(_token, 6);

            Assert.AreEqual(HttpStatusCode.OK, result);
        }
    }
}