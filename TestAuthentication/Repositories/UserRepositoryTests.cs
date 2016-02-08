using Microsoft.VisualStudio.TestTools.UnitTesting;
using Authentication.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Authentication.Models;
using System.Transactions;

namespace Authentication.Repositories.Tests
{
    [TestClass()]
    public class UserRepositoryTests
    {
        private static UserRepository ur = new UserRepository();

        [TestMethod()]
        public void GetSHA512Test()
        {
            //arrange
            var text = "test";
            var hexExpected = "EE26B0DD4AF7E749AA1A8EE3C10AE9923F618980772E473F8819A5D4940E0DB27AC185F8A0E1D5F84F88BC887FD67B143732C304CC5FA9AD8E6F57F50028A8FF";

            //act
            var hash = UserRepository.GetSHA512(text, "");
            hash = hash.Replace("-", string.Empty);


            Assert.IsTrue(hash == hexExpected);
        }

        [TestMethod()]
        public void AddTest()
        {
            using (TransactionScope transaction = new TransactionScope())
            {
                User user = new User();
                user.Age = 21;
                user.Autonomous_community = "Andalucía";
                user.Email = "jorgeron1993@hotmail.com";
                user.Genre = "Masculino";
                user.UserName = "testUser";
                user.Password = "testUser";
                user.Password = UserRepository.GetSHA512(user.Password, user.UserName);

                ur.Add(user);
            }
        }

        [TestMethod()]
        public void UserExistsTest()
        {
            using (TransactionScope transaction = new TransactionScope())
            {
                bool exists = ur.UserExists(1);
                Assert.IsTrue(exists);
            }
        }

        [TestMethod()]
        public void UniqueTest()
        {
            using (TransactionScope transaction = new TransactionScope())
            {
                bool unique = ur.Unique("test0");
                Assert.IsFalse(unique);

                unique = ur.Unique("jorgeron1993@hotmail.com");
                Assert.IsFalse(unique);

                unique = ur.Unique("wefwef");
                Assert.IsTrue(unique);
            }
        }

        [TestMethod()]
        public void ValidLoginTest()
        {
            using (TransactionScope transaction = new TransactionScope())
            {
                bool valid = ur.ValidLogin("test0", UserRepository.GetSHA512("test0", "test0"));
                Assert.IsTrue(valid);
            }
        }

        [TestMethod()]
        public void EditTest()
        {
            using (TransactionScope transaction = new TransactionScope())
            {
                User user = ur.FindByUsername("test0");

                ur.Edit(user);
            }
        }

        [TestMethod()]
        public void RemoveTest()
        {
            using (TransactionScope transaction = new TransactionScope())
            {
                User user = ur.FindByUsername("test0");

                ur.Remove(user);
            }
            //ur.Add(user);
        }

        [TestMethod()]
        public void GetUsersTest()
        {
            using (TransactionScope transaction = new TransactionScope())
            {
                IEnumerable<User> users = ur.GetUsers();
                Assert.IsNotNull(users);
            }
        }

        [TestMethod()]
        public void FindByIdTest()
        {
            using (TransactionScope transaction = new TransactionScope())
            {
                User user = ur.FindById(1);
                Assert.IsNotNull(user);
            }
        }

        [TestMethod()]
        public void tokenIsCorrectTest()
        {
            using (TransactionScope transaction = new TransactionScope())
            {
                string token = "test1" + ":" + UserRepository.GetSHA512("test1" + UserRepository.GetSHA512("test1", "test1"), "test1");
                bool valid = ur.tokenIsCorrect(token);
                Assert.IsTrue(valid);
            }
        }
    }
}