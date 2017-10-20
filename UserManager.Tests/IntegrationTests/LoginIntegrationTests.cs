using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UserManager.Contract;
using UserManager.Tests.MockClasses;
using UserManager.Services;
using UserManager.Implementation;
using UserManager.ActionResults;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace UserManager.Tests.IntegrationTests
{
    [TestClass]
    public class LoginIntegrationTests
    {
        IUserManager _userManager;
        User _user1;
        User _user2;
        User _user3;
        const string _login1 = "john@smith.com";
        const string _password1 = "P@ssword";
        const string _login2 = "kelly@ready.com";
        const string _password2 = "P@ssword";
        const string _login3 = "adam@hume.com";
        const string _password3 = "P@ssword";

        IList<User> registeredUsers = new List<User>();

        public LoginIntegrationTests()
        {
            var initializer = new BasicDatabaseInitializer(TestConfig.TestConnectionString);
            var loginservice = new BasicLoginService(TestConfig.TestConnectionString, new TimeSpan(1, 0, 0, 0));
            var registerService = new BasicRegisterService(TestConfig.TestConnectionString);
            _userManager = new ConcreteUserManager(registerService, loginservice);
            _userManager.InitializeDatabase(initializer);
            _user1 = new User()
            {
                Login = _login1,
                Password = _password1,
                PasswordConfirmation = _password1
            };
            _user2 = new User()
            {
                Login = _login2,
                Password = _password2,
                PasswordConfirmation = _password2
            };
            _user3 = new User()
            {
                Login = _login3,
                Password = _password3,
                PasswordConfirmation = _password3
            };
            registeredUsers.Add(_user1);
            registeredUsers.Add(_user2);
            registeredUsers.Add(_user3);
        }       

        [TestMethod]
        public void Login_IntegrationTests_CanLoginMultipleUsers_ShouldSucceed()
        {
            //arrange    
            foreach (var user in registeredUsers)
            {
                _userManager.RegisterUser(user.Login, user.Password, user.PasswordConfirmation);
            }

            //act
            var results = new List<LoginResult>();
            DateTime timeOfLogin = DateTime.UtcNow;
            foreach (var user in registeredUsers)
            {
                var result = _userManager.Login(user.Login, user.Password);
                results.Add(result);
            }

            //assert
            Assert.IsTrue(results.Any());
            foreach (var result in results)
            {
                Assert.IsTrue(result.IsSuccess);
                Assert.IsNotNull(result.Token);
                Assert.IsTrue(result.TokenExpiratioDate > timeOfLogin);
            }
        }
    }
}
