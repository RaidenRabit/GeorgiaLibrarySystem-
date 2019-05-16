using System;
using NUnit.Framework;
using Moq;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using Core;
using GTLService.DataAccess.Code;
using GTLService.DataAccess.Database;
using GTLService.DataManagement.Code;
using GTLService.DataManagement.Database;
using GTLService.Controller;
using System.Linq;
using System.Linq.Expressions;

namespace Tests.IntegrationTest
{
    public class LoginTest
    {
        [Test]
        [TestCase(10000000, "test", true)]
        [TestCase(10000000, "", true)]
        [TestCase(1, "test", true)]
        [TestCase(1, "", true)]
        [TestCase(10000000, "testnasdfnsndfnsdfjnsdnas", true)]
        [TestCase(1000000000, "testnasdfnsndfnsdfjnsdnas", true)]
        public void LoginService_Database(int ssn, string password, bool passing)
        {
            //Arrange
            var mock = new Mock<Context>();
            
            var objectResultMock = new Mock<ObjectResult<int?>>();
            objectResultMock.Setup(x => x.GetEnumerator()).Returns(new List<int?> {1}.GetEnumerator());

            mock.Setup(x => x.Login(It.IsAny<int>(), It.IsAny<string>()))
                .Returns(objectResultMock.Object);
            var loginService = new LoginService(new LoginDm_Database(new LoginDa_Database(mock.Object)));

            //Act
            var result = loginService.Login(ssn,password);

            //Assert
            Assert.IsTrue(result == passing);
        }

        [Test]
        //pass
        [TestCase(555555555, "test", true)]
        [TestCase(999999999, "testtesttesttest", true)]
        [TestCase(100000000, "t", true)]
        //fail
        [TestCase(9999999, "test", false)]
        [TestCase(999999999, "testtesttesttesttest", false)]
        [TestCase(10000000, "", false)]
        public void LoginService_Code(int ssn, string password, bool passing)
        {
            //Arrange
            var mock = new Mock<Context>();
            
            mock.Setup(x => x.People.Find(It.IsAny<int>()))
                .Returns(new Person{Password = password});

            var loginService = new LoginService(new LoginDm_Code(new LoginDa_Code(mock.Object)));

            //Act
            var result = loginService.Login(ssn,password);

            //Assert
            Assert.IsTrue(result == passing);
        }
    }
}