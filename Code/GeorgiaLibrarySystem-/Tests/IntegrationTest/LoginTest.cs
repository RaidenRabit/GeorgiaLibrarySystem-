using System;
using NUnit.Framework;
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
        private LoginService _loginService;
        
        [Test]
        //Database
        //pass
        [TestCase(123456789, "test", true, "Database")]
        //fail
        [TestCase(123456789, "", false, "Database")]//too short password
        [TestCase(1, "test", false, "Database")]//too short ssn
        [TestCase(1, "", false, "Database")]//too short ssn and password
        [TestCase(123456789, "testnasdfnsndfnsdfjnsdnas", false, "Database")]//too long password
        [TestCase(1000000000, "", false, "Database")]//too long ssn
        [TestCase(1000000000, "testnasdfnsndfnsdfjnsdnas", false, "Database")]//too long ssn and password
        //Code
        //pass
        [TestCase(123456789, "test", true, "Code")]
        //fail
        [TestCase(123456789, "", false, "Code")]//too short password
        [TestCase(1, "test", false, "Code")]//too short ssn
        [TestCase(1, "", false, "Code")]//too short ssn and password
        [TestCase(123456789, "testnasdfnsndfnsdfjnsdnas", false, "Code")]//too long password
        [TestCase(1000000000, "", false, "Code")]//too long ssn
        [TestCase(1000000000, "testnasdfnsndfnsdfjnsdnas", false, "Code")]//too long ssn and password
        public void Login(int ssn, string password, bool passing, string approach)
        {
            //Arrange
            Setup(approach);

            //Act
            var result = _loginService.Login(ssn,password);

            //Assert
            Assert.IsTrue(result == passing);
        }

        private void Setup(string approach)
        {
            DatabaseTesting.ResetDatabase();
            Context context = new Context();
            switch (approach)
            {
                case "Code":
                    _loginService = new LoginService(new LoginDm_Code(new LoginDa_Code(),context));
                    break;
                case "Database":
                    _loginService = new LoginService(new LoginDm_Database(new LoginDa_Database(context)));
                    break;
                default:
                    new NotImplementedException();
                    break;
            }
        }
    }
}