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
            var loginService = new LoginService(new LoginDm_Database(new LoginDa_Database(FillPersonDatabase())));

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
            var loginService = new LoginService(new LoginDm_Code(new LoginDa_Code(FillPersonDatabase())));

            //Act
            var result = loginService.Login(ssn,password);

            //Assert
            Assert.IsTrue(result == passing);
        }
        #endregion

        private static Context FillPersonDatabase()
        {
            Context context = new Context();
            context.Database.ExecuteSqlCommand("IF EXISTS(select * from Person where SSN=123456789) "+
                "update Person set Password='test' where SSN=123456789 "+
            "ELSE "+
                "INSERT INTO Person (SSN, AddressID, CampusID, Phone, Password) "+
                "VALUES (123456789, 5, 5, 20202020, 'test');");
            context.SaveChanges();
            return context;
        }
    }
}