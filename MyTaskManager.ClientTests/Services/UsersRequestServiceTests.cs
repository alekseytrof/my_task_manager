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

        [TestMethod()]
        public void DeleteUserTest()
        {
            var service = new UsersRequestService();
            var token = service.GetToken("admin@mail.ru", "qwerty@#$tr55ofimov!");

            var result = service.DeleteUser(token, 25);

            Assert.AreEqual(HttpStatusCode.OK, result);
        }

        [TestMethod()]
        public void CreateMultiplyUsersTest()
        {
            var service = new UsersRequestService();
            var token = service.GetToken("admin@mail.ru", "qwerty@#$tr55ofimov!");

            UserDto userTest1 = new UserDto("Petr", "Trofimov", "trof@mail.ru", "qwerty", UserStatus.User, "89203636374");
            UserDto userTest2 = new UserDto("Alex", "Belov", "belov@mail.ru", "qwerty", UserStatus.Editor, "89243636374");
            UserDto userTest3 = new UserDto("Oleg", "Shigal", "shigal@mail.ru", "qwerty", UserStatus.User, "89203635374");

            List<UserDto> users = new List<UserDto>() { userTest1, userTest2, userTest3 };

            var result = service.CreateMultiplyUsers(token, users);

            Assert.AreEqual(HttpStatusCode.OK, result);
        }

        [TestMethod()]
        public void UpdateUserTest()
        {
            var service = new UsersRequestService();
            var token = service.GetToken("admin@mail.ru", "qwerty@#$tr55ofimov!");

            UserDto userTest = new UserDto("Oleg", "Shigalev", "shigalev@mail.ru", "qwerty", UserStatus.Editor, "+89203635374");
            userTest.Id = 31;

            var result = service.UpdateUser(token, userTest);

            Assert.AreEqual(HttpStatusCode.OK, result);
        }
    }
}