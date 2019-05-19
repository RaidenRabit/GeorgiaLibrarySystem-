using Core;
using GTLService.Controller;
using GTLService.DataAccess.Code;
using GTLService.DataAccess.Database;
using GTLService.DataManagement.Code;
using GTLService.DataManagement.Database;
using NUnit.Framework;

namespace Tests.IntegrationTest
{
    public class LendingTest
    {
        #region Database
        [Test]
        //pass
        [TestCase(123456789, 10, false, true)]//member with 4 books borrows 5th
        //fail
        [TestCase(123456789, 10, true, false)]//Too many books borrowed
        [TestCase(123456789, 11, false, false)]//book already lent to you
        [TestCase(123456789, 4, false, false)]//book already lent to other member
        [TestCase(1, 10, false, false)]//Person doesn't exist
        public void LendingService_Database_LendBook(int ssn, int copyId, bool maxNumberOfBooks, bool passing)
        {
            //Arrange
            var loginService = new LendingService(new LendingDm_Database(new LendingDa_Database(FillBorrowDatabase(maxNumberOfBooks))));

            //Act
            var result = loginService.LendBook(ssn, copyId);

            //Assert
            Assert.IsTrue(result == passing);
        }

        [Test]
        //pass
        [TestCase(5, true)]//return taken book
        //fail
        [TestCase(0, false)]//wrong copyId
        [TestCase(9, false)]//copy already returned
        public void LendingService_Database_ReturnBook(int copyId, bool passing)
        {
            //Arrange
            var loginService = new LendingService(new LendingDm_Database(new LendingDa_Database(FillBorrowDatabase(false))));

            //Act
            var result = loginService.ReturnBook(copyId);

            //Assert
            Assert.IsTrue(result == passing);
        }

        #endregion

        #region Code

        [Test]
        //pass
        [TestCase(123456789, 10, false, true)]//member with 4 books borrows 5th
        //fail
        [TestCase(123456789, 10, true, false)]//Too many books borrowed
        [TestCase(123456789, 11, false, false)]//book already lent to you
        [TestCase(123456789, 4, false, false)]//book already lent to other member
        [TestCase(1, 10, false, false)]//Person doesn't exist
        public void LendingService_Code_LendBook(int ssn, int copyId, bool maxNumberOfBooks, bool passing)
        {
            //Arrange
            var context = FillBorrowDatabase(maxNumberOfBooks);
            var loginService = new LendingService(new LendingDm_Code(new LendingDa_Code(context), new MemberDa_Code(context)));

            //Act
            var result = loginService.LendBook(ssn, copyId);

            //Assert
            Assert.IsTrue(result == passing);
        }

        [Test]
        //pass
        [TestCase(5, true)]//return taken book
        //fail
        [TestCase(0, false)]//wrong copyId
        [TestCase(9, false)]//copy already returned
        public void LendingService_Code_ReturnBook(int copyId, bool passing)
        {
            //Arrange
            var context = FillBorrowDatabase(false);
            var loginService = new LendingService(new LendingDm_Code(new LendingDa_Code(context), new MemberDa_Code(context)));

            //Act
            var result = loginService.ReturnBook(copyId);

            //Assert
            Assert.IsTrue(result == passing);
        }

        #endregion

        private static Context FillBorrowDatabase(bool maxNumberOfBooks)
        {
            string text = "GETDATE()";
            if(maxNumberOfBooks)
            {
                text = "null";
            }
            Context context = new Context();
            context.Database.ExecuteSqlCommand("TRUNCATE TABLE Borrow");
            context.Database.ExecuteSqlCommand("INSERT INTO Borrow (CopyID, SSN, FromDate, ToDate)"+
                                               "VALUES (5,123456786,GETDATE(),null),"+
                                               "(3,123456788,GETDATE(),null),"+
                                               "(2,123456786,GETDATE(),GETDATE()),"+
                                               "(6,123456789,GETDATE(),null),"+
                                               "(1,123456789,GETDATE(),GETDATE()),"+
                                               "(4,123456786,GETDATE(),null),"+
                                               "(7,123456789,GETDATE(),null),"+
                                               "(3,123456789,GETDATE(),"+text+"),"+
                                               "(9,123456786,GETDATE(),GETDATE()),"+
                                               "(2,123456789,GETDATE(),null),"+
                                               "(11,123456789,GETDATE(),null);");
            context.SaveChanges();
            return context;
        }
    }
}
