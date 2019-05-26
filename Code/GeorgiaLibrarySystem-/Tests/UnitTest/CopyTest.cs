using Core;
using GTLService.DataAccess.Code;
using GTLService.DataManagement.Code;
using Moq;
using NUnit.Framework;

namespace Tests.UnitTest
{
    class CopyTest
    {
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
            var personDa_Code_Mock = new Mock<LibrarianDa_Code>();
            var copyDa_Code_Mock = new Mock<CopyDa_Code>();
            var lendingDa_Code_Mock = new Mock<LoaningDa_Code>();
            var context_Mock = new Mock<Context>();
            
            personDa_Code_Mock.Setup(x => x.CheckLibrarianSsn(It.IsAny<int>(), It.IsAny<Context>()))
                .Returns(ssnPassing);
            Copy copy = null;
            if (idPassing)
            {
                copy = new Copy();
            }
            copyDa_Code_Mock.Setup(x => x.GetCopy(It.IsAny<int>(), It.IsAny<Context>()))
                .Returns(copy);
            copyDa_Code_Mock.Setup(x => x.DeleteCopy(It.IsAny<Copy>(), It.IsAny<Context>()))
                .Returns(deleteResult);

            var copyDM = new CopyDm_Code(copyDa_Code_Mock.Object, lendingDa_Code_Mock.Object, personDa_Code_Mock.Object, context_Mock.Object);

            //Act
            var result = copyDM.DeleteCopy(0, 0);

            //Assert
            Assert.IsTrue(result == testPassing);
        }
    }
}
