using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyTaskManager.Client.Services;
using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}