using Microsoft.VisualStudio.TestTools.UnitTesting;
using Authentication.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Authentication.Models;
using System.Web.Mvc;
using System.Transactions;

namespace Authentication.Controllers.Tests
{
    [TestClass()]
    public class HomeControllerTests
    {
        [TestMethod()]
        public void LoginTest()
        {
            using (TransactionScope transaction = new TransactionScope())
            {
                UserLoginModel model = new UserLoginModel();
                HomeController controller = new HomeController();

                model.Password = "test0";
                model.Username = "test0";

                ActionResult result = controller.Login(model, null);

                Assert.IsNotNull(result);
            }
        }

        [TestMethod]
        public void RegisterTest()
        {
            using (TransactionScope transaction = new TransactionScope())
            {
                UserRegisterModel model = new UserRegisterModel();
                HomeController controller = new HomeController();
                model.Age = 21;
                model.Autonomous_community = "Andalucía";
                model.Email = "asdf@hotmail.com";
                model.Genre = "Masculino";
                model.Username = "pepito";
                model.Password = "pepito";
                model.ConfirmPassword = "pepito";

                ActionResult ar = controller.Register(model);

                Assert.IsNotNull(ar);
            }
        }

        [TestMethod()]
        public void LoginTest1()
        {
            String url = "/Help/Index";
            HomeController controller = new HomeController();
            ActionResult result = controller.Login(url);
            Assert.IsNotNull(result);
        }

        [TestMethod()]
        public void RegisterTest1()
        {
            HomeController controller = new HomeController();
            ActionResult result = controller.Register();
            Assert.IsNotNull(result);
        }
    }
}