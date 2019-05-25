using System;
using Core;
using GTLService.Controller;
using GTLService.Controller.IController;
using GTLService.DataAccess.Code;
using GTLService.DataAccess.Database;
using GTLService.DataManagement.Code;
using GTLService.DataManagement.Database;
using NUnit.Framework;

namespace Tests.IntegrationTest
{
    public class CopyTest
    {
        private ICopyService _copyService;
        
        [Test]
        //Code approach
        [TestCase("1" , "Code", 1)]
        [TestCase("8" , "Code", 0)]
        //Database approach
        [TestCase("1" , "Database", 1)]
        [TestCase("8" , "Database", 0)]
        public void GetAvailableCopyId(string isbn, string approach, int expectedResult)
        {
            //Arrange
            Setup(approach);

            //Act
            int result = _copyService.GetAvailableCopyId(isbn);

            //Assert
            Assert.IsTrue(result == expectedResult, "actual result: " + result);
        }
        
        [Test]
        //Code approach
        [TestCase("1", "Code", 9)]
        [TestCase("8", "Code", 0)]
        //Database approach
        [TestCase("1", "Database", 9)]
        [TestCase("8", "Database", 0)]
        public void GetTotalNrCopies(string isbn, string approach, int expectedResult)
        {
            //Arrange
            Setup(approach);

            //Act
            int result = _copyService.GetTotalNrCopies(isbn);
            
            //Assert
            Assert.IsTrue(result.Equals(expectedResult), "actual result: " + result);
        }
        
        [Test]
        //Code approach
        [TestCase("3" , "Code", 1)] //all on loan
        [TestCase("7" , "Code", 1)] //some on loan
        [TestCase("2" , "Code", 0)] //no copies on loan
        [TestCase("8" , "Code", 0)] //no copies
        //Database approach
        [TestCase("3" , "Database", 1)] //all on loan
        [TestCase("7" , "Database", 1)] //some on loan
        [TestCase("2" , "Database", 0)] //no copies on loan
        [TestCase("8" , "Database", 0)] //no copies
        public void GetOutOnLoan(string isbn, string approach, int expectedResult)
        {
            //Arrange
            Setup(approach);

            //Act
            int result = _copyService.GetOutOnLoan(isbn);
            
            //Assert
            Assert.IsTrue(result.Equals(expectedResult), "actual result: " + result);
        }

        [Test]
        //Code approach
        [TestCase(0,0, "Code", false)] //invalid ssn and id
        [TestCase(123456785,0, "Code", false)] //valid ssn, invalid id
        [TestCase(0,1, "Code", false)] //invalid ssn, valid id
        [TestCase(123456785,12, "Code", true)] //valid ssn and id
        //Db approach
        [TestCase(0,0, "Database", false)] //invalid ssn and id
        [TestCase(123456785,0, "Database", false)] //valid ssn, invalid id
        [TestCase(0,1, "Database", false)] //invalid ssn, valid id
        [TestCase(123456785,13, "Database", true)] //valid ssn and id
        public void DeleteCopy(int ssn, int copyId, string approach, bool passing)
        {
            //Arrange
            Setup(approach);

            //Act
            bool result = _copyService.DeleteCopy(ssn, copyId);

            //Assert
            Assert.IsTrue(result.Equals(passing));
        }

        private void Setup(string approach)
        {
            DatabaseTesting.ResetDatabase();
            Context context = new Context();
            switch (approach)
            {
                case "Code":
                    CopyDa_Code copDa_Code = new CopyDa_Code();
                    LoaningDa_Code loaningDaCode = new LoaningDa_Code();
                    LibrarianDa_Code librarianDa = new LibrarianDa_Code();
                    CopyDm_Code copyDm_Code = new CopyDm_Code(copDa_Code, loaningDaCode, librarianDa, context);
                    _copyService = new CopyService(copyDm_Code);
                    break;
                case "Database":
                    CopyDa_Database copyDa_Database = new CopyDa_Database(context);
                    LoaningDa_Database loaningDaDatabase = new LoaningDa_Database(context);
                    CopyDm_Database copyDm_Database = new CopyDm_Database(copyDa_Database, loaningDaDatabase);
                    _copyService = new CopyService(copyDm_Database);
                    break;
                default:
                    new NotImplementedException();
                    break;
            }
        }
    }
}
