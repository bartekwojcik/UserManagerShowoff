using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManager.Services;
using UserManager.Implementation;
using UserManager.Contract;
using UserManager.ActionResults;

namespace UserManager.Tests.ServicesTests.UserManager
{
    [TestClass]
    public class UserManagerTests
    {
        [TestMethod]
        public void UserManager_Initialize_InitializationFailed_ShouldReturnResultFalse()
        {
            //arrange
            Mock<IDatabaseInitializer> mockInitializer = new Mock<IDatabaseInitializer>();
            var failErrors = new List<string> { "Initialization failed because of reasons" };
            mockInitializer.Setup(x => x.Initialize()).Returns(new ActionResults.InitializationResult(false, failErrors));

            IUserManager userManager = new ConcreteUserManager(null, null);

            //Act
            var initResult = userManager.InitializeDatabase(mockInitializer.Object);

            //Assert
            Assert.IsTrue(initResult.Errors.Any());
            Assert.IsFalse(initResult.IsSuccess);
        }

        [TestMethod]
        public void UserManager_Initialize_InitializationSucceeded_ShouldReturnResulTrue()
        {
            //arrange
            Mock<IDatabaseInitializer> mockInitializer = new Mock<IDatabaseInitializer>();
            mockInitializer.Setup(x => x.Initialize()).Returns(new ActionResults.InitializationResult(true));
            IUserManager userManager = new ConcreteUserManager(null, null);

            //Act
            var initResult = userManager.InitializeDatabase(mockInitializer.Object);

            //Assert
            Assert.IsFalse(initResult.Errors.Any());
            Assert.IsTrue(initResult.IsSuccess);
        }

        [TestMethod]
        public void UserManager_Register_RegisterNewUser_ShouldSucceed()
        {
            //arrange
            var login = "john@smith.com";
            var password = "P@ssw0rd";
            Mock<IRegisterService> mock = new Mock<IRegisterService>();
            mock.Setup(x => x.Register(login, password, password)).Returns(new RegisterResult(true));
            IUserManager userManager = new ConcreteUserManager(mock.Object, null);

            //Act
            var registerResult = userManager.RegisterUser(login, password, password);

            //Assert
            Assert.IsFalse(registerResult.Errors.Any());
            Assert.IsTrue(registerResult.IsSuccess);
        }

        [TestMethod]
        public void UserManager_Register_RegisterExistingUser_ShouldFail()
        {
            //arrange
            var login = "john@smith.com";
            var password = "P@ssw0rd";
            var errors = new List<string> { $"User {login} already exists" };
            Mock<IRegisterService> mock = new Mock<IRegisterService>();
            mock.Setup(x => x.Register(login, password, password)).Returns(new RegisterResult(false, errors));
            IUserManager userManager = new ConcreteUserManager(mock.Object, null);

            //Act
            var registerResult = userManager.RegisterUser(login, password, password);

            //Assert
            Assert.IsTrue(registerResult.Errors.Any());
            Assert.IsFalse(registerResult.IsSuccess);
        }

        [TestMethod]
        public void UserManager_Login_LoginExistingUser_ShouldSucceed()
        {
            //arrange
            var login = "john@smith.com";
            var password = "P@ssw0rd";

            Mock<ILoginService> mock = new Mock<ILoginService>();
            mock.Setup(x => x.Login(login, password)).Returns(new LoginResult(true, "randomtoken", DateTime.Now.AddDays(1)));
            var loginDate = DateTime.Now;
            IUserManager userManager = new ConcreteUserManager(null, mock.Object);

            //Act
            var loginResult = userManager.Login(login, password);

            //Assert
            Assert.IsFalse(loginResult.Errors.Any());
            Assert.IsTrue(loginResult.IsSuccess);
            Assert.IsNotNull(loginResult.Token);
            Assert.IsTrue(loginResult.TokenExpiratioDate > loginDate);
        }

        [TestMethod]
        public void UserManager_Login_LoginUnregisteredUser_ShouldFail()
        {
            //arrange
            var login = "john@smith.com";
            var password = "P@ssw0rd";
            var errors = new List<string> { $"Incorrect user or password" };
            Mock<ILoginService> mock = new Mock<ILoginService>();
            mock.Setup(x => x.Login(login, password)).Returns(new LoginResult(false, errors));            
            IUserManager userManager = new ConcreteUserManager(null, mock.Object);

            //Act
            var loginResult = userManager.Login(login, password);

            //Assert
            Assert.IsTrue(loginResult.Errors.Any());
            Assert.IsFalse(loginResult.IsSuccess);
            Assert.IsNull(loginResult.Token);
        }
    }
}
