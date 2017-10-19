using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UserManager.Services;
using Moq;
using System.Collections.Generic;
using System.Linq;

namespace UserManager.Tests.ServicesTests.DatabaseInitializer
{
    [TestClass]
    public class LoggableInitializerTests
    {
        [TestMethod]
        public void Initialize_InitializationFailed_ShouldLoggedErrors()
        {
            //Arange
            Mock<BasicDatabaseInitializer> mockInitializer = new Mock<BasicDatabaseInitializer>();
            var failErrors = new List<string> { "Initialization failed because of reasons" };
            mockInitializer.Setup(x => x.Initialize()).Returns(new ActionResults.InitializationResult(false, failErrors));
            LoggableDatabaseInitializer testedInstance = new LoggableDatabaseInitializer(mockInitializer.Object);

            //Act
            testedInstance.Initialize();

            //Assert
            Assert.IsTrue(testedInstance.LoggedErrors.Any());
        }

        [TestMethod]
        public void Initialize_InitializationSucceeded_ErrorsShouldBeClear()
        {
            //Arange
            Mock<BasicDatabaseInitializer> mockInitializer = new Mock<BasicDatabaseInitializer>();
            mockInitializer.Setup(x => x.Initialize()).Returns(new ActionResults.InitializationResult(true));
            LoggableDatabaseInitializer testedInstance = new LoggableDatabaseInitializer(mockInitializer.Object);

            //Act
            testedInstance.Initialize();

            //Assert
            Assert.IsFalse(testedInstance.LoggedErrors.Any());
        }
    }
}
