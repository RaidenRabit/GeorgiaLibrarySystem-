using System;
using System.Collections.Generic;
using Core;
using NUnit.Framework;
using GtlService.DataAccess.Code;
using GtlService.DataManagement.Code;
using GTLService.DataAccess.Code;
using GTLService.DataAccess.Database;
using GTLService.DataAccess.IDataAccess;
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
            Assert.IsTrue(result[1].Available_Copies == 2);
            Assert.IsTrue(result[2].Available_Copies == 1);
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
