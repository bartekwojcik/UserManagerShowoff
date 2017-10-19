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
        static IUserManager _userManager;
        static User _tempUser;
        static readonly string _login = "john@smith.com";
        static readonly string _password = "P@ssword";
        static readonly string _passwordConf = "P@ssword";

        [ClassInitialize]
        public static void Init_RegisterIntegrationTests(TestContext testContext)
        {
            var initializer = new BasicDatabaseInitializer(TestConfig.TestConnectionString);
            var registerService = new BasicRegisterService(TestConfig.TestConnectionString);
            _userManager = new ConcreteUserManager(registerService);
            _userManager.InitializeDatabase(initializer);
            _tempUser = new User()
            {
                Login = _login,
                Password = _password,
                PasswordConfirmation = _passwordConf
            };
        }
        [TestMethod]
        public void CanRegisterFirstUser_ShouldSucceeded()
        {

            var result = _userManager.RegisterUser(_tempUser.Login, _tempUser.Password, _tempUser.PasswordConfirmation);
            var debug = 5;

            //todo clear users table after
            //arrange, act, assert
        }

        [TestMethod]
        public void CanRegisterTwoUsersWithTheSameName_ShouldFail()
        {
            var result = _userManager.RegisterUser(_tempUser.Login, _tempUser.Password, _tempUser.PasswordConfirmation);
            var debug = 5;

            //todo clear users table after
            //arrange, act, assert
        }

        //todo testy gdy są błęde hasla
    }
}
