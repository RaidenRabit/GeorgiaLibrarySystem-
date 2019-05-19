using System;
using System.Collections.Generic;
using Core;
using NUnit.Framework;
using GTLService.DataAccess.Code;
using GTLService.DataAccess.Database;
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
            var materialDa_Code_Mock = new Mock<MaterialDa_Code>(null);
            var libraryDa_Code_Mock = new LibraryDa_Code(null);
            var personDa_Code_Mock = new PersonDa_Code(null);
            var objects = MaterialsSetUp();

            materialDa_Code_Mock.Setup(x => x.ReadMaterials(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()))
                .Returns(objects.Item2);
            materialDa_Code_Mock.Setup(x => x.ReadCopies(It.IsAny<int>(), It.IsAny<string>()))
                .Returns(objects.Item1);

            var materialDm = new MaterialDm_Code(materialDa_Code_Mock.Object, libraryDa_Code_Mock, personDa_Code_Mock);

            //Act
            var result = materialDm.ReadMaterials(materialTitle, author, numOfRecords, isbn, jobStatus);

            //Assert
            Assert.IsTrue(result.Count == 2);
            Assert.IsTrue(result[0].Available_Copies == 2);
            Assert.IsTrue(result[1].Available_Copies == 1);
        }
        
        [Test]
        [TestCase(true, true, true, true, true)]
        [TestCase(true, false, false, false, false)]
        [TestCase(true, false, true, true, true)]    
        [TestCase(false, false, true, false, false)]   
        [TestCase(false, true, false, true, false)]    
        [TestCase(false, true, true, true, false)]    
        public void MaterialDmCreateMaterialTest(bool ssnPassing, bool isbnPassing, bool libraryNamePassing, bool typeNamePassing, bool testPassing)
        {
            //Arrange
            var materialDa_Code_Mock = new Mock<MaterialDa_Code>(null);
            var libraryDa_Code_Mock = new Mock<LibraryDa_Code>(null);
            var personDa_Code_Mock = new Mock<PersonDa_Code>(null);
            var objects = MaterialsSetUp();
            
            personDa_Code_Mock.Setup(x => x.CheckLibrarianSsn(It.IsAny<int>()))
                .Returns(ssnPassing);
            materialDa_Code_Mock.Setup(x => x.CheckMaterialIsbn(It.IsAny<int>()))
                .Returns(isbnPassing);
            libraryDa_Code_Mock.Setup(x => x.CheckLibraryName(It.IsAny<string>()))
                .Returns(libraryNamePassing);
            materialDa_Code_Mock.Setup(x => x.CheckTypeName(It.IsAny<string>()))
                .Returns(typeNamePassing);
            materialDa_Code_Mock.Setup(x => x.ReadMaterials(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()))
                .Returns(objects.Item2);

            var materialDm = new MaterialDm_Code(materialDa_Code_Mock.Object, libraryDa_Code_Mock.Object, personDa_Code_Mock.Object);

            //Act
            var result = materialDm.CreateMaterial(0, 0,null,null,null,null,null, 0);

            //Assert
            Assert.IsTrue(result == testPassing);
        }

        [Test]
        [TestCase(true, true, true)]
        [TestCase(true, false, false)]
        [TestCase(false, true, false)]
        [TestCase(false, false, false)]
        public void MaterialDmDeleteMaterialTest(bool ssnPassing, bool isbnPassing, bool testPassing)
        {
            //Arrange
            var materialDa_Code_Mock = new Mock<MaterialDa_Code>(null);
            var libraryDa_Code_Mock = new Mock<LibraryDa_Code>(null);
            var personDa_Code_Mock = new Mock<PersonDa_Code>(null);
            
            personDa_Code_Mock.Setup(x => x.CheckLibrarianSsn(It.IsAny<int>()))
                .Returns(ssnPassing);
            materialDa_Code_Mock.Setup(x => x.CheckMaterialIsbn(It.IsAny<int>()))
                .Returns(isbnPassing);

            var materialDm = new MaterialDm_Code(materialDa_Code_Mock.Object, libraryDa_Code_Mock.Object, personDa_Code_Mock.Object);

            //Act
            var result = materialDm.DeleteMaterial(0, 0);

            //Assert
            Assert.IsTrue(result == testPassing);
        }

        [Test]
        [TestCase(true, true, true)]
        [TestCase(true, false, false)]
        [TestCase(false, true, false)]
        [TestCase(false, false, false)]
        public void MaterialDmDeleteCopyTest(bool ssnPassing, bool idPassing, bool testPassing)
        {
            //Arrange
            var materialDa_Code_Mock = new Mock<MaterialDa_Code>(null);
            var libraryDa_Code_Mock = new Mock<LibraryDa_Code>(null);
            var personDa_Code_Mock = new Mock<PersonDa_Code>(null);
            
            personDa_Code_Mock.Setup(x => x.CheckLibrarianSsn(It.IsAny<int>()))
                .Returns(ssnPassing);
            materialDa_Code_Mock.Setup(x => x.CheckCopyId(It.IsAny<int>()))
                .Returns(idPassing);

            var materialDm = new MaterialDm_Code(materialDa_Code_Mock.Object, libraryDa_Code_Mock.Object, personDa_Code_Mock.Object);

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
