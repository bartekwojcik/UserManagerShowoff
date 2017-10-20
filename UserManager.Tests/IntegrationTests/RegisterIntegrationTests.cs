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
        IRegisterService _registerService;
        User _tempUser;
        const string _login = "john@smith.com";
        const string _password = "P@ssword";
        const string _passwordConf = "P@ssword";

        public RegisterIntegrationTests()
        {
            var initializer = new BasicDatabaseInitializer(TestConfig.TestConnectionString);
            _registerService = new BasicRegisterService(TestConfig.TestConnectionString);              
            _tempUser = new User()
            {
                Login = _login,
                Password = _password,
                PasswordConfirmation = _passwordConf
            };
        }
        [TestMethod]
        public void RegisterService_IntegrationTests_CanRegisterDistinctUsers_ShouldSucceeded()
        {
            var result = _registerService.Register(_tempUser.Login, _tempUser.Password, _tempUser.PasswordConfirmation);
            var result2 = _registerService.Register("another@guys.com", "qwerty", "qwerty");
            Assert.IsTrue(result.IsSuccess);
            Assert.IsFalse(result.Errors.Any());
            Assert.IsTrue(result2.IsSuccess);
            Assert.IsFalse(result2.Errors.Any());
        }

        [TestMethod]
        public void RegisterService_IntegrationTests_CanRegisterTwoUsersWithTheSameName_ShouldFail()
        {
            var result = _registerService.Register(_tempUser.Login, _tempUser.Password, _tempUser.PasswordConfirmation);
            var result2 = _registerService.Register(_tempUser.Login, _tempUser.Password, _tempUser.PasswordConfirmation);
            Assert.IsTrue(result.IsSuccess);
            Assert.IsFalse(result.Errors.Any());
            Assert.IsFalse(result2.IsSuccess);
            Assert.IsTrue(result2.Errors.Any());
        }

        [TestMethod]
        public void RegisterService_IntegrationTests_CanRegisterMismatchPassword_ShouldFailed()
        {
            var result = _registerService.Register("whatever@gmail.com", _tempUser.Password, _tempUser.PasswordConfirmation + "difference");
            Assert.IsFalse(result.IsSuccess);
            Assert.IsTrue(result.Errors.Any());
        }
    }
}
