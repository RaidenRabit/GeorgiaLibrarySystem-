﻿using System;
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

    public class MaterialTest
    {

        private IMaterialService _materialService;
        
        [Test]
        //Code approach
        [TestCase("", "", 10, null, "0", "Code", 10)]
        [TestCase("horror book", "", 10, null, "0", "Code", 1)]
        [TestCase("horror book", "Pala", 10, null, "0", "Code", 1)]
        [TestCase("", "", 6, null, "0", "Code", 6)]
        [TestCase("", "", 10, 1, "0", "Code", 4)]
        [TestCase("", "", 10, null, "books", "Code", 9)]
        //Database approach
        [TestCase("", "", 10, null, "0", "Database", 10)]
        [TestCase("horror book", "", 10, null, "0", "Database", 1)]
        [TestCase("horror book", "Pala", 10, null, "0", "Database", 1)]
        [TestCase("", "", 6, null, "0", "Database", 6)]
        [TestCase("", "", 10, 1, "0", "Database", 4)]
        [TestCase("", "", 10, null, "books", "Database", 9)]
        public void GetMaterials(string materialTitle, string author, int numOfRecords, int isbn, string jobStatus,
            string approach, int expectedResult)
        {
            //Arrange
            Setup(approach);
            
            //Act
            var result = _materialService.GetMaterials(materialTitle, author, numOfRecords, isbn.ToString(), jobStatus);

            //Assert
            Assert.AreEqual(result.Count,expectedResult, "Real result: " + result.Count);

        }

        [Test]
        //Code approach
        [TestCase(123456785, 1, "GTL", "books", "Code", true)]
        [TestCase(123456785, 0, "GTL", "books", "Code", true)]    
        [TestCase(123456785, 0, "Non-existent", "Non-existent", "Code", false)]
        [TestCase(0, 0, "GTL", "books", "Code", false)]   
        [TestCase(0, 1, "Non-existent", "books", "Code", false)]    
        [TestCase(0, 1, "GTL", "books", "Code", false)]  
        //Database approach
        [TestCase(123456785, 1, "GTL", "books", "Database", true)]
        [TestCase(123456785, 0, "GTL", "books", "Database", true)]    
        [TestCase(123456785, 0, "Non-existent", "Non-existent", "Database", false)]
        [TestCase(0, 0, "GTL", "books", "Database", false)]   
        [TestCase(0, 1, "Non-existent", "books", "Database", false)]    
        [TestCase(0, 1, "GTL", "books", "Database", false)]  
        public void CreateMaterial(int ssn, int isbn, string library, string typeName, string approach, bool passing)
        {
            //Arrange
            Setup(approach);
            string author = "TestAuthor", description = "TestDescription", title = "testTitle";
            int quantity = 2;

            //Act
            bool result =
                _materialService.CreateMaterial(ssn, isbn.ToString(), library, author, description, title, typeName, quantity);

            //Assert
            Assert.IsTrue(result.Equals(passing));
        }
        
        [Test]
        //Code approach
        [TestCase(0, 0, "Code", false)] //invalid ssn and isbn
        [TestCase(123456785, 0, "Code", false)] //valid ssn, invalid isbn
        [TestCase(0, 1, "Code", false)] //invalid ssn, valid isbn
        [TestCase(123456785, 8, "Code", true)] //valid ssn and isbn
        //Database approach
        [TestCase(0, 0, "Database", false)] //invalid ssn and isbn
        [TestCase(123456785, 0, "Database", false)] //valid ssn, invalid isbn
        [TestCase(0, 1, "Database", false)] //invalid ssn, valid isbn
        [TestCase(123456785, 8, "Database", true)] //valid ssn and isbn
        public void DeleteMaterial(int ssn, int isbn, string approach, bool passing)
        {
            //Arrange
            Setup(approach);

            //Act
            bool result = _materialService.DeleteMaterial(ssn, isbn.ToString());

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
                    MaterialDa_Code materialDa_Code = new MaterialDa_Code();
                    LibraryDa_Code libraryDa_Code = new LibraryDa_Code();
                    LibrarianDa_Code librarianDaCode = new LibrarianDa_Code();
                    CopyDa_Code copyDa_Code = new CopyDa_Code();
                    LoaningDa_Code loaningDaCode = new LoaningDa_Code();
                    MaterialDm_Code materialsDm_Code = new MaterialDm_Code(materialDa_Code, libraryDa_Code, librarianDaCode, copyDa_Code, loaningDaCode, context);
                    _materialService = new MaterialService(materialsDm_Code);
                    break;
                case "Database":
                    MaterialsDa_Database materialDa_Db = new MaterialsDa_Database(context);
                    MaterialsDm_Database materialsDm_Db = new MaterialsDm_Database(materialDa_Db);
                    _materialService = new MaterialService(materialsDm_Db);
                    break;
                default:
                    new NotImplementedException();
                    break;
            }
        }
    }
}
