using NUnit.Framework;
using Moq;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using Core;
using GtlService.DataAccess.Database;
using GtlService.DataManagement.Database;
using GTLService.Controller;

namespace Tests.IntegrationTest
{
    public class LoginTest
    {
        [Test]
        public void Login_CorrectSSNAndPassword()
        {
            //Arrange
            var mock = new Mock<Context>();
            
            var myMockedObjectResult = new Mock<ObjectResult<int?>>();
            myMockedObjectResult.Setup(x => x.GetEnumerator()).Returns(new List<int?> {1}.GetEnumerator());

            mock.Setup(x => x.Login(It.IsAny<int>(), It.IsAny<string>()))
                .Returns(myMockedObjectResult.Object);
            var repo = new LoginService(new LoginDm_Database(new LoginDa_Database(mock.Object)));

            //Act
            var result = repo.Login(10000000,"Test");

            //Assert
            Assert.IsTrue(result);
        }

    //    [Test]
    //    public void LoginDataAccess_CorrectSSNAndPassword_True()
    //    {
    //        //Arrange
    //        var mock = new Mock<GTLEntities>();
    //        mock.Setup(x => x.People.Any(It.IsAny<Expression<Func<Person, bool>>>()))
    //            .Returns(true);
    //        //mock.Setup(x => x.People.Find(It.IsAny<int>()))
    //        //    .Returns(new Person{Password = "test"});
    //        var repo = new LoginDa_Database(mock.Object);

    //        //Act
    //        var result = repo.Login(1, "test");

    //        //Assert
    //        Assert.IsTrue(result);
    //    }
    }
}