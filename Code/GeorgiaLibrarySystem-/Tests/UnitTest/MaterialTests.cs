using System;
using System.Collections.Generic;
using Core;
using NUnit.Framework;
using GTLService.DataAccess.Code;
using GTLService.DataManagement.Code;
using Moq;

namespace Tests.UnitTest
{
    public class MaterialTests
    {
        [Test]
        //pass
        [TestCase("", "", 10, null, "0")]
        public void MaterialDmReadMaterialTest(string materialTitle, string author, int numOfRecords = 10, int isbn = 0, string jobStatus = "0")
        {
            //Arrange
            var materialDa_Code_Mock = new Mock<MaterialDa_Code>();
            var libraryDa_Code_Mock = new Mock<LibraryDa_Code>();
            var personDa_Code_Mock = new Mock<LibrarianDa_Code>();
            var copyDa_Code_Mock = new Mock<CopyDa_Code>();
            var lendingDa_Code_Mock = new Mock<LoaningDa_Code>();
            var context_Mock = new Mock<Context>();
            var objects = MaterialsSetUp();

            materialDa_Code_Mock.Setup(x => x.ReadMaterials(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<Context>()))
                .Returns(objects.Item2);
            copyDa_Code_Mock.Setup(x => x.ReadCopies(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Context>()))
                .Returns(objects.Item1);
            
            var materialDm = new MaterialDm_Code(materialDa_Code_Mock.Object, libraryDa_Code_Mock.Object,
                personDa_Code_Mock.Object, copyDa_Code_Mock.Object, lendingDa_Code_Mock.Object, context_Mock.Object);

            //Act
            var result = materialDm.ReadMaterials(materialTitle, author, numOfRecords, isbn.ToString(), jobStatus);

            //Assert
            Assert.IsTrue(result.Count == 2);
            Assert.IsTrue(result[0].Available_Copies == 2);
            Assert.IsTrue(result[1].Available_Copies == 1);
        }
        
        [Test]
        //pass
        [TestCase(true, true, true, true, true)]
        [TestCase(true, false, true, true, true)]    
        //fail
        [TestCase(true, false, false, false, false)]
        [TestCase(false, false, true, false, false)]   
        [TestCase(false, true, false, true, false)]    
        [TestCase(false, true, true, true, false)]    
        public void MaterialDmCreateMaterialTest(bool ssnPassing, bool isbnPassing, bool libraryNamePassing, bool typeNamePassing, bool testPassing)
        {
            //Arrange
            var materialDa_Code_Mock = new Mock<MaterialDa_Code>();
            var libraryDa_Code_Mock = new Mock<LibraryDa_Code>();
            var personDa_Code_Mock = new Mock<LibrarianDa_Code>();
            var copyDa_Code_Mock = new Mock<CopyDa_Code>();
            var lendingDa_Code_Mock = new Mock<LoaningDa_Code>();
            var context_Mock = new Mock<Context>();
            var objects = MaterialsSetUp();
            
            personDa_Code_Mock.Setup(x => x.CheckLibrarianSsn(It.IsAny<int>(), It.IsAny<Context>()))
                .Returns(ssnPassing);
            materialDa_Code_Mock.Setup(x => x.CheckMaterialIsbn(It.IsAny<string>(), It.IsAny<Context>()))
                .Returns(isbnPassing);
            libraryDa_Code_Mock.Setup(x => x.CheckLibraryName(It.IsAny<string>(), It.IsAny<Context>()))
                .Returns(libraryNamePassing);
            copyDa_Code_Mock.Setup(x => x.CheckTypeName(It.IsAny<string>(), It.IsAny<Context>()))
                .Returns(typeNamePassing);
            materialDa_Code_Mock.Setup(x => x.ReadMaterials(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<Context>()))
                .Returns(objects.Item2);
            
            var materialDm = new MaterialDm_Code(materialDa_Code_Mock.Object, libraryDa_Code_Mock.Object,
                personDa_Code_Mock.Object, copyDa_Code_Mock.Object, lendingDa_Code_Mock.Object, context_Mock.Object);

            //Act
            var result = materialDm.CreateMaterial(0, "0",null,null,null,null,null, 0);

            //Assert
            Assert.IsTrue(result == testPassing);
        }

        [Test]
        //pass
        [TestCase(true, true, true, true)]
        //fail
        [TestCase(true, true, false, false)]
        [TestCase(true, false, true, false)]
        [TestCase(false, true, true, false)]
        [TestCase(false, false, true, false)]
        [TestCase(false, true, false, false)]
        [TestCase(true, false, false, false)]
        [TestCase(false, false, false, false)]
        public void MaterialDmDeleteMaterialTest(bool ssnPassing, bool isbnPassing, bool deleteResult, bool testPassing)
        {
            //Arrange
            var materialDa_Code_Mock = new Mock<MaterialDa_Code>();
            var libraryDa_Code_Mock = new Mock<LibraryDa_Code>();
            var personDa_Code_Mock = new Mock<LibrarianDa_Code>();
            var copyDa_Code_Mock = new Mock<CopyDa_Code>();
            var lendingDa_Code_Mock = new Mock<LoaningDa_Code>();
            var context_Mock = new Mock<Context>();
            
            personDa_Code_Mock.Setup(x => x.CheckLibrarianSsn(It.IsAny<int>(), It.IsAny<Context>()))
                .Returns(ssnPassing);
            materialDa_Code_Mock.Setup(x => x.CheckMaterialIsbn(It.IsAny<string>(), It.IsAny<Context>()))
                .Returns(isbnPassing);
            materialDa_Code_Mock.Setup(x => x.DeleteMaterial(It.IsAny<string>(), It.IsAny<Context>()))
                .Returns(deleteResult);
            
            var materialDm = new MaterialDm_Code(materialDa_Code_Mock.Object, libraryDa_Code_Mock.Object,
                personDa_Code_Mock.Object, copyDa_Code_Mock.Object, lendingDa_Code_Mock.Object, context_Mock.Object);

            //Act
            var result = materialDm.DeleteMaterial(0, "0");

            //Assert
            Assert.IsTrue(result == testPassing);
        }

        [Test]
        //pass
        [TestCase(true, true, true, true)]
        //fail
        [TestCase(true, true, false, false)]
        [TestCase(true, false, true, false)]
        [TestCase(false, true, true, false)]
        [TestCase(false, false, true, false)]
        [TestCase(false, true, false, false)]
        [TestCase(true, false, false, false)]
        [TestCase(false, false, false, false)]
        public void MaterialDmDeleteCopyTest(bool ssnPassing, bool idPassing, bool deleteResult, bool testPassing)
        {
            //Arrange
            var materialDa_Code_Mock = new Mock<MaterialDa_Code>();
            var libraryDa_Code_Mock = new Mock<LibraryDa_Code>();
            var personDa_Code_Mock = new Mock<LibrarianDa_Code>();
            var copyDa_Code_Mock = new Mock<CopyDa_Code>();
            var lendingDa_Code_Mock = new Mock<LoaningDa_Code>();
            var context_Mock = new Mock<Context>();
            
            personDa_Code_Mock.Setup(x => x.CheckLibrarianSsn(It.IsAny<int>(), It.IsAny<Context>()))
                .Returns(ssnPassing);
            copyDa_Code_Mock.Setup(x => x.CheckCopyId(It.IsAny<int>(), It.IsAny<Context>()))
                .Returns(idPassing);
            copyDa_Code_Mock.Setup(x => x.DeleteCopy(It.IsAny<int>(), It.IsAny<Context>()))
                .Returns(deleteResult);

            var materialDm = new MaterialDm_Code(materialDa_Code_Mock.Object, libraryDa_Code_Mock.Object,
                personDa_Code_Mock.Object, copyDa_Code_Mock.Object, lendingDa_Code_Mock.Object, context_Mock.Object);

            //Act
            var result = materialDm.DeleteCopy(0, 0);

            //Assert
            Assert.IsTrue(result == testPassing);
        }

        private Tuple<List<Copy>, List<Material>> MaterialsSetUp()
        {
            Material material = new Material {ISBN = "1", Title = "test book", Description = "TEST++", Author = "Hala"};
            Material material2 = new Material {ISBN = "2", Title = "horror book", Description = "TEST++", Author = "Pala"};
            
            Copy copy = new Copy { CopyID = 1, ISBN = "1", TypeName = "books", LibraryName = "GTL"};
            Copy copy2 = new Copy { CopyID = 2, ISBN = "1", TypeName = "books", LibraryName = "GTL"};
            Copy copy3 = new Copy { CopyID = 3, ISBN = "2", TypeName = "books", LibraryName = "GTL"};

            List<Copy> copies = new List<Copy> {copy, copy2, copy3};
            List<Material> materials = new List<Material> {material, material2 };
            return new Tuple<List<Copy>, List<Material>>(copies, materials);
        }
    }
}
