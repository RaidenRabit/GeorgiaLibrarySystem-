using System;
using Core;
using GTLService.Controller;
using GTLService.DataAccess.Code;
using GTLService.DataAccess.Database;
using GTLService.DataManagement.Code;
using GTLService.DataManagement.Database;
using NUnit.Framework;

namespace Tests.IntegrationTest
{
    public class LoaningTest
    {
        private LoaningService _loaningService;
        
        [Test]
        //Database
        //pass
        [TestCase(123456786, 10, false, true, "Database")]//member with less the max amount of books
        //fail
        [TestCase(123456789, 10, true, false, "Database")]//Too many books borrowed
        [TestCase(123456789, 11, false, false, "Database")]//book already lent to you
        [TestCase(123456789, 4, false, false, "Database")]//book already lent to other member
        [TestCase(1, 10, false, false, "Database")]//Person doesn't exist
        //Code
        //pass
        [TestCase(123456786, 10, false, true, "Code")]//member with less the max amount of books
        //fail
        [TestCase(123456789, 10, true, false, "Code")]//Too many books borrowed
        [TestCase(123456789, 11, false, false, "Code")]//book already lent to you
        [TestCase(123456789, 4, false, false, "Code")]//book already lent to other member
        [TestCase(1, 10, false, false, "Code")]//Person doesn't exist
        public void LoanBook(int ssn, int copyId, bool maxNumberOfBooks, bool passing, string approach)
        {
            //Arrange
            Setup(approach);

            //Act
            var result = _loaningService.LoanBook(ssn, copyId);

            //Assert
            Assert.IsTrue(result == passing);
        }

        [Test]
        //Database
        //pass
        [TestCase(5, true, "Database")]//return taken book
        //fail
        [TestCase(0, false, "Database")]//wrong copyId
        [TestCase(9, false, "Database")]//copy already returned
        //Code
        //pass
        [TestCase(5, true, "Code")]//return taken book
        //fail
        [TestCase(0, false, "Code")]//wrong copyId
        [TestCase(9, false, "Code")]//copy already returned
        public void ReturnBook(int copyId, bool passing, string approach)
        {
            //Arrange
            Setup(approach);

            //Act
            var result = _loaningService.ReturnBook(copyId);

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
                    _loaningService =  new LoaningService(new LoaningDm_Code(new LoaningDa_Code(context), new MemberDa_Code(context)));
                    break;
                case "Database":
                    _loaningService =  new LoaningService(new LoaningDm_Database(new LoaningDa_Database(context)));
                    break;
                default:
                    new NotImplementedException();
                    break;
            }
        }
    }
}
