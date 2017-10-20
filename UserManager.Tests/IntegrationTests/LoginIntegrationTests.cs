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
using Castle.Windsor;
using Castle.MicroKernel.Registration;

namespace UserManager.Tests.IntegrationTests
{
    [TestClass]
    public class LoginIntegrationTests
    {
        IUserManager _userManager;
        private WindsorContainer Container;
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
            
            Container = new WindsorContainer();
            Container.Register(Component.For<IDatabaseInitializer>().ImplementedBy<BasicDatabaseInitializer>().LifestyleTransient()
                 .DependsOn(Dependency.OnValue("connectionString", TestConfig.TestConnectionString)));
            Container.Register(Component.For<ILoginService>().ImplementedBy<BasicLoginService>().LifestyleTransient()
                 .DependsOn(Dependency.OnValue("connectionString", TestConfig.TestConnectionString),
                 Dependency.OnValue("tokenValidityTime", new TimeSpan(1, 0, 0, 0))));
            Container.Register(Component.For<IRegisterService>().ImplementedBy<BasicRegisterService>().LifestyleTransient()
                .DependsOn(Dependency.OnValue("connectionString", TestConfig.TestConnectionString)));
            Container.Register(Component.For<IUserManager>().ImplementedBy<ConcreteUserManager>().LifestyleTransient());

            // I realise this is more like Service Locator anti-pattern, but as I mentioned before
            // here I would not use contaniner at the first place

            var initializer = Container.Resolve<IDatabaseInitializer>();
            _userManager = Container.Resolve<IUserManager>();

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
        public void IntegrationTests_LoginService_CanLoginMultipleUsers_ShouldSucceed()
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

            results.ForEach(result =>
            {
                Assert.IsTrue(result.IsSuccess);
                Assert.IsNotNull(result.Token);
                Assert.IsTrue(result.TokenExpiratioDate > timeOfLogin);
            });
        }

        [TestMethod]
        public void IntegrationTests_LoginService_CanLoginUnregistredUsers_ShouldFail()
        {
            var randomLogin = Guid.NewGuid().ToString("N");
            var randomPasswords = Guid.NewGuid().ToString("N");
            var result = _userManager.Login(randomLogin, randomPasswords);
            Assert.IsFalse(result.IsSuccess);
            Assert.IsNull(result.Token);
            Assert.IsTrue(result.Errors.Any());
        }

        [TestMethod]
        public void IntegrationTests_LoginService_CanLoginMismatchedPassword_ShouldSucceed()
        {
            //arrange                
            var registerResult = _userManager.RegisterUser(_login1, _password1, _password1);
            //act
            var mismatchedPassword = _password1 + "failure";
            var loginResult = _userManager.Login(_login1, mismatchedPassword);

            //assert
            Assert.IsFalse(loginResult.IsSuccess);
            Assert.IsNull(loginResult.Token);
            Assert.IsTrue(registerResult.IsSuccess);
            Assert.IsFalse(registerResult.Errors.Any());

        }
    }
}
