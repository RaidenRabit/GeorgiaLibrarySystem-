using NUnit.Framework;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Core;
using GTLService.DataAccess.Code;
using GTLService.DataAccess.Database;
using GTLService.DataManagement.Code;
using GTLService.DataManagement.Database;
using GTLService.Controller;

namespace Tests.IntegrationTest
{
    public class LoginTest
    {
        #region Database
        [Test]
        //pass
        [TestCase(123456789, "test", true)]
        //fail
        [TestCase(123456789, "", false)]//too short password
        [TestCase(1, "test", false)]//too short ssn
        [TestCase(1, "", false)]//too short ssn and password
        [TestCase(123456789, "testnasdfnsndfnsdfjnsdnas", false)]//too long password
        [TestCase(1000000000, "", false)]//too long ssn
        [TestCase(1000000000, "testnasdfnsndfnsdfjnsdnas", false)]//too long ssn and password
        public void LoginService_Database_Login(int ssn, string password, bool passing)
        {
            //Arrange
            var loginService = new LoginService(new LoginDm_Database(new LoginDa_Database(new Context())));

            //Act
            var result = loginService.Login(ssn,password);

            //Assert
            Assert.IsTrue(result == passing);
        }
        #endregion

        #region Code
        [Test]
        //pass
        [TestCase(123456789, "test", true)]
        //fail
        [TestCase(123456789, "", false)]//too short password
        [TestCase(1, "test", false)]//too short ssn
        [TestCase(1, "", false)]//too short ssn and password
        [TestCase(123456789, "testnasdfnsndfnsdfjnsdnas", false)]//too long password
        [TestCase(1000000000, "", false)]//too long ssn
        [TestCase(1000000000, "testnasdfnsndfnsdfjnsdnas", false)]//too long ssn and password
        public void LoginService_Code_Login(int ssn, string password, bool passing)
        {
            //Arrange
            var people = new List<Person>
            {
                new Person {SSN = 123456789, Password = "test"}
            }.AsQueryable();
            
            var dbPeopleSet = DbContextMock.CreateDbSetMock(people);

            var mock = new Mock<Context>();
            mock.Setup(x => x.People).Returns(dbPeopleSet.Object);

            var loginService = new LoginService(new LoginDm_Code(new LoginDa_Code(mock.Object)));

            //Act
            var result = loginService.Login(ssn,password);

            //Assert
            Assert.IsTrue(result == passing);
        }
        #endregion
    }
}