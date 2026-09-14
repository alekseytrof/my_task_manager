using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyTaskManager.Client.Services;
using MyTaskManager.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MyTaskManager.Client.Services.Tests
{
    [TestClass()]
    public class UsersRequestServiceTests
    {
        [TestMethod()]
        public void GetTokenTest()
        {
            var token = new UsersRequestService().GetToken("admin@mail.ru", "qwerty@#$tr55ofimov!");

            Console.WriteLine(token.access_token);
            Assert.IsNotNull(token.access_token);
        }

        [TestMethod()]
        public void CreateUserTest()
        {
            var service = new UsersRequestService();
            var token = service.GetToken("admin@mail.ru", "qwerty@#$tr55ofimov!");

            UserDto userTest = new UserDto("Petr", "Trofimov", "trof@mail.ru", "qwerty", UserStatus.User, "89203636374");

            var result = service.CreateUser(token, userTest);

            Assert.AreEqual(HttpStatusCode.OK, result);
        }

        [TestMethod()]
        public void GetAllUsersTest()
        {
            var service = new UsersRequestService();
            var token = service.GetToken("admin@mail.ru", "qwerty@#$tr55ofimov!");

            var result = service.GetAllUsers(token);

            Console.WriteLine(result.Count);
            Assert.AreNotEqual(Array.Empty<UserDto>(), result.ToArray());
        }
    }
}