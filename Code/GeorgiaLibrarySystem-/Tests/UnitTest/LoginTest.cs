﻿using NUnit.Framework;
using GtlService.DataAccess;
using GtlService.DataManagement;
using Moq;

namespace Tests.UnitTest
{
    public class LoginTest
    {
        [Test]
        [TestCase(55555555, "test")]
        [TestCase(99999999, "testtesttesttest")]
        [TestCase(10000000, "t")]
        public void Login_CorrectSSNAndPassword(int ssn, string password)
        {
            //Arrange
            var mock = new Mock<LoginDaDatabase>(null);
            mock.Setup(x => x.Login(It.IsAny<int>(),It.IsAny<string>()))
                .Returns(true);
            var repo = new LoginDmCode(mock.Object);

            //Act
            var result = repo.Login(ssn, password);

            //Assert
            Assert.IsTrue(result);
        }

        [Test]
        [TestCase(9999999, "test")]
        [TestCase(999999999, "testtesttesttesttest")]
        [TestCase(10000000, "")]
        public void Login_FalseSSNOrPassword(int ssn, string password)
        {
            //Arrange
            var mock = new Mock<LoginDaDatabase>(null);
            mock.Setup(x => x.Login(It.IsAny<int>(),It.IsAny<string>()))
                .Returns(true);
            var repo = new LoginDmCode(mock.Object);

            //Act
            var result = repo.Login(ssn, password);

            //Assert
            Assert.IsFalse(result);
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