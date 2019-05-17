using NUnit.Framework;
using GTLService.DataAccess.Code;
using GTLService.DataManagement.Code;
using Moq;

namespace Tests.UnitTest
{
    public class LoginTest
    {
        #region Code
        [Test]
        //pass
        [TestCase(555555555, "test", true)]
        [TestCase(999999999, "testtesttesttest", true)]
        [TestCase(100000000, "t", true)]
        //fail
        [TestCase(9999999, "test", false)]
        [TestCase(999999999, "testtesttesttesttest", false)]
        [TestCase(10000000, "", false)]
        public void LoginDm_Code_Login(int ssn, string password, bool passing)
        {
            //Arrange
            var mock = new Mock<LoginDa_Code>(null);
            mock.Setup(x => x.Login(It.IsAny<int>(),It.IsAny<string>()))
                .Returns(true);
            var loginDm = new LoginDm_Code(mock.Object);

            //Act
            var result = loginDm.Login(ssn, password);

            //Assert
            Assert.IsTrue(result == passing);
        }
        #endregion
    }
}