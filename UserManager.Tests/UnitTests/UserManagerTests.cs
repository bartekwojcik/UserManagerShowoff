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

namespace UserManager.Tests.ServicesTests.UserManager
{
    [TestClass]
    public class UserManagerTests
    {
        [TestMethod]
        public void Initialize_InitializationFailed_ShouldReturnResultFalse()
        {
            //arrange
            Mock<IDatabaseInitializer> mockInitializer = new Mock<IDatabaseInitializer>();
            var failErrors = new List<string> { "Initialization failed because of reasons" };
            mockInitializer.Setup(x => x.Initialize()).Returns(new ActionResults.InitializationResult(false, failErrors));

            IUserManager userManager = new ConcreteUserManager(null,null);

            //Act
            var initResult = userManager.InitializeDatabase(mockInitializer.Object);

            //Assert
            Assert.IsTrue(initResult.Errors.Any());
            Assert.IsFalse(initResult.IsSuccess);
        }

        [TestMethod]
        public void Initialize_InitializationSucceeded_ShouldReturnResulTrue()
        {
            //arrange
            Mock<IDatabaseInitializer> mockInitializer = new Mock<IDatabaseInitializer>();
            mockInitializer.Setup(x => x.Initialize()).Returns(new ActionResults.InitializationResult(true));
            IUserManager userManager = new ConcreteUserManager(null,null);

            //Act
            var initResult = userManager.InitializeDatabase(mockInitializer.Object);

            //Assert
            Assert.IsFalse(initResult.Errors.Any());
            Assert.IsTrue(initResult.IsSuccess);
        }
    }
}
