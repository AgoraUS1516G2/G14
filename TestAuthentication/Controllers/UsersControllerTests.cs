using Microsoft.VisualStudio.TestTools.UnitTesting;
using Authentication.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Authentication.Models;
using System.Web.Http;
using Authentication.Repositories;
using System.Web.Http.Results;

namespace Authentication.Controllers.Tests
{
    [TestClass()]
    public class UsersControllerTests
    {

        UsersController uc = new UsersController();

        [TestMethod()]
        public void GetUsersTest()
        {
            IEnumerable<User> result;

            result = uc.GetUsers();

            Assert.IsNotNull(result);
        }

        [TestMethod()]
        public void GetUserTest()
        {
            IHttpActionResult result;

            result = uc.GetUser("test1");

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<User>));
        }

        [TestMethod()]
        public void checkTokenTest()
        {
            IHttpActionResult result;
            string token = "test1" + ":" + UserRepository.GetSHA512("test1" + UserRepository.GetSHA512("test1", "test1"), "test1");

            result = uc.checkToken(token);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<TokenValidationResult>));
        }

        [TestMethod()]
        public void checkTokenUserTest()
        {
            IHttpActionResult result;

            string token = "test1" + ":" + UserRepository.GetSHA512("test1" + UserRepository.GetSHA512("test1", "test1"), "test1");
            String[] split = token.Split(':');
            token = split[1].Trim();
            var username = split[0].Trim();

            result = uc.checkTokenUser(token, username);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<TokenValidationResult>));
        }
    }
}