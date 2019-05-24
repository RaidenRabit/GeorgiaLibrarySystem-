using Core;
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
            var mock = new Mock<LoginDa_Code>();
            var context_Mock = new Mock<Context>();

            mock.Setup(x => x.Login(It.IsAny<int>(),It.IsAny<string>(), It.IsAny<Context>()))
                .Returns(true);
            var loginDm = new LoginDm_Code(mock.Object, context_Mock.Object);

            //Act
            var result = loginDm.Login(ssn, password);

            //Assert
            Assert.IsTrue(result == passing);
        }
        #endregion
    }
}