using System;
using System.Linq;
using GtlService.Controller;
using NUnit.Framework;
using GtlService.DataAccess;
using GtlService.DataManagement;
using GtlService.Model;
using Moq;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;

namespace Tests.IntegrationTest
{
    public class LoginTest
    {
        [Test]
        public void Login_CorrectSSNAndPassword()
        {
            //Arrange
            var mock = new Mock<GTLEntities>();
            
            var myMockedObjectResult = new Mock<ObjectResult<int?>>();
            myMockedObjectResult.Setup(x => x.GetEnumerator()).Returns(new List<int?> {1}.GetEnumerator());

            mock.Setup(x => x.Login(It.IsAny<int>(), It.IsAny<string>()))
                .Returns(myMockedObjectResult.Object);
            var repo = new LoginController(new LoginDmDatabase(new LoginDaDatabase(mock.Object)));

            //Act
            var result = repo.Login(10000000,"Test",0);

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
    //        var repo = new LoginDaDatabase(mock.Object);

    //        //Act
    //        var result = repo.Login(1, "test");

    //        //Assert
    //        Assert.IsTrue(result);
    //    }
    }
}