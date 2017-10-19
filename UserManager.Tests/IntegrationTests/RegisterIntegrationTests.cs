using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManager.Contract;
using UserManager.Implementation;
using UserManager.Services;
using UserManager.Tests;
using UserManager.Tests.MockClasses;

namespace UserManager.IntegrationTests
{
    [TestClass]
    public class RegisterIntegrationTests
    {
        IUserManager _userManager;
        User _tempUser;
        const string _login = "john@smith.com";
        const string _password = "P@ssword";
        const string _passwordConf = "P@ssword";


        public RegisterIntegrationTests()
        {
            var initializer = new BasicDatabaseInitializer(TestConfig.TestConnectionString);
            var registerService = new BasicRegisterService(TestConfig.TestConnectionString);
            _userManager = new ConcreteUserManager(registerService, null);
            _userManager.InitializeDatabase(initializer);
            _tempUser = new User()
            {
                Login = _login,
                Password = _password,
                PasswordConfirmation = _passwordConf
            };
        }
        [TestMethod]
        public void CanRegisterDistinctUsers_ShouldSucceeded()
        {

            var result = _userManager.RegisterUser(_tempUser.Login, _tempUser.Password, _tempUser.PasswordConfirmation);
            var result2 = _userManager.RegisterUser("another@guys.com", "qwerty", "qwerty");
            Assert.IsTrue(result.IsSuccess);
            Assert.IsFalse(result.Errors.Any());
            Assert.IsTrue(result2.IsSuccess);
            Assert.IsFalse(result2.Errors.Any());
        }

        [TestMethod]
        public void CanRegisterTwoUsersWithTheSameName_ShouldFail()
        {
            var result = _userManager.RegisterUser(_tempUser.Login, _tempUser.Password, _tempUser.PasswordConfirmation);
            var result2 = _userManager.RegisterUser(_tempUser.Login, _tempUser.Password, _tempUser.PasswordConfirmation);
            Assert.IsTrue(result.IsSuccess);
            Assert.IsFalse(result.Errors.Any());
            Assert.IsFalse(result2.IsSuccess);
            Assert.IsTrue(result2.Errors.Any());
        }

        [TestMethod]
        public void CanRegisterMismatchPassword_ShouldFailed()
        {
            var result = _userManager.RegisterUser("whatever@gmail.com", _tempUser.Password, _tempUser.PasswordConfirmation + "difference");
            Assert.IsFalse(result.IsSuccess);
            Assert.IsTrue(result.Errors.Any());
        }
    }
}
