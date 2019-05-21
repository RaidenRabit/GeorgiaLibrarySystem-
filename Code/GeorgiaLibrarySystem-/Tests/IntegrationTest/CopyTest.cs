﻿using System;
using Core;
using GTLService.Controller;
using GTLService.DataAccess.Code;
using GTLService.DataAccess.Database;
using GTLService.DataManagement.Code;
using GTLService.DataManagement.Database;
using GTLService.DataManagement.IDataManagement;
using NUnit.Framework;

namespace Tests.IntegrationTest
{
    public class CopyTest
    {
        private ICopyService _copyService;
        
        [Test]
        //Code approach
        [TestCase(1 , "Code", 1)]
        [TestCase(8 , "Code", 0)]
        //Database approach
        [TestCase(1 , "Database", 1)]
        [TestCase(8 , "Database", 0)]
        public void GetAvailableCopyId(int isbn, string approach, int expectedResult)
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
        [TestCase(1 , "Code", 9)]
        [TestCase(8 , "Code", 0)]
        //Database approach
        [TestCase(1 , "Database", 9)]
        [TestCase(8 , "Database", 0)]
        public void GetTotalNrCopies(int isbn, string approach, int expectedResult)
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
        [TestCase(3 , "Code", 1)] //all on loan
        [TestCase(7 , "Code", 1)] //some on loan
        [TestCase(2 , "Code", 0)] //no copies on loan
        [TestCase(8 , "Code", 0)] //no copies
        //Database approach
        [TestCase(3 , "Database", 1)] //all on loan
        [TestCase(7 , "Database", 1)] //some on loan
        [TestCase(2 , "Database", 0)] //no copies on loan
        [TestCase(8 , "Database", 0)] //no copies
        public void GetOutOnLoan(int isbn, string approach, int expectedResult)
        {
            //Arrange
            Setup(approach);

            //Act
            int result = _copyService.GetOutOnLoan(isbn);
            
            //Assert
            Assert.IsTrue(result.Equals(expectedResult), "actual result: " + result);
        }

        private void Setup(string approach)
        {
            DatabaseTesting.ResetDatabase();
            Context context = new Context();
            switch (approach)
            {
                case "Code":
                    CopyDa_Code copDa_Code = new CopyDa_Code(context);
                    LendingDa_Code lendingDa_Code = new LendingDa_Code(context);
                    CopyDm_Code copyDm_Code = new CopyDm_Code(copDa_Code, lendingDa_Code);
                    _copyService = new CopyService(copyDm_Code);
                    break;
                case "Database":
                    CopyDa_Database copyDa_Database = new CopyDa_Database(context);
                    LendingDa_Database lendingDa_Database = new LendingDa_Database(context);
                    CopyDm_Database copyDm_Database = new CopyDm_Database(copyDa_Database, lendingDa_Database);
                    _copyService = new CopyService(copyDm_Database);
                    break;
                default:
                    new NotImplementedException();
                    break;
            }
        }
    }
}
