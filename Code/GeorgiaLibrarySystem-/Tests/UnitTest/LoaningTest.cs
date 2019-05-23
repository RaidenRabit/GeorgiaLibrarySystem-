using System.Collections.Generic;
using Core;
using GTLService.DataAccess.Code;
using GTLService.DataManagement.Code;
using Moq;
using NUnit.Framework;

namespace Tests.UnitTest
{
    public class LoaningTest
    {
        #region Code
        [Test]
        //pass
        [TestCase(5, 4, false, 1, 2, true)]
        [TestCase(5, 0, false, 1, 2, true)]
        //fail
        [TestCase(5, 5, false, 1, 2, false)]
        [TestCase(0, 5, false, 1, 2, false)]
        [TestCase(5,5,true,1,2,false)]
        [TestCase(5,4,true,1,2,false)]
        public void LoaningDm_Code_LendBook(int allowedNumberOfBooks, int currentNumberOfBooks, bool loanExists, int ssn, int copyId, bool passing)
        {
            //Arrange
            var mockLendingDa = new Mock<LoaningDa_Code>();
            var context_Mock = new Mock<Context>();

            mockLendingDa.Setup(x => x.MemberLoanBooks(It.IsAny<int>(), It.IsAny<Context>()))
                .Returns(currentNumberOfBooks);
            Loan loan = null;
            if (loanExists)
            {
                loan = new Loan();
            }
            mockLendingDa.Setup(x => x.GetLoan(It.IsAny<int>(), It.IsAny<Context>()))
                .Returns(loan);
            mockLendingDa.Setup(x => x.LoanBook(It.IsAny<Loan>(), It.IsAny<Context>()))
                .Returns(true);

            var mockMemberDa = new Mock<MemberDa_Code>();
            mockMemberDa.Setup(x => x.GetMember(It.IsAny<int>(), It.IsAny<Context>()))
                .Returns(new Member{MemberType = new MemberType{NrOfBooks = allowedNumberOfBooks}});

            var lendingDm = new LoaningDm_Code(mockLendingDa.Object,mockMemberDa.Object,context_Mock.Object);

            //Act
            var result = lendingDm.LoanBook(ssn,copyId);

            //Assert
            Assert.IsTrue(result == passing);
        }

        [Test]
        //pass
        [TestCase(true, 1, true)]
        //fail
        [TestCase(false, 1, false)]
        public void LoaningDm_Code_ReturnBook(bool loanExists, int copyId, bool passing)
        {
            //Arrange
            var mockLendingDa = new Mock<LoaningDa_Code>();
            var context_Mock = new Mock<Context>();

            Loan loan = null;
            if (loanExists)
            {
                loan = new Loan();
            }
            mockLendingDa.Setup(x => x.GetLoan(It.IsAny<int>(), It.IsAny<Context>()))
                .Returns(loan);
            mockLendingDa.Setup(x => x.UpdateLoan(It.IsAny<Loan>(), It.IsAny<Context>()))
                .Returns(true);

            var mockMemberDa = new Mock<MemberDa_Code>();

            var lendingDm = new LoaningDm_Code(mockLendingDa.Object,mockMemberDa.Object,context_Mock.Object);

            //Act
            var result = lendingDm.ReturnBook(copyId);

            //Assert
            Assert.IsTrue(result == passing);
        }

        [Test]
        //pass
        [TestCase(true)]
        //fail
        public void LoaningDm_Code_NoticeFilling(bool passing)
        {
            //Arrange
            var mockLendingDa = new Mock<LoaningDa_Code>();
            var context_Mock = new Mock<Context>();

            List<Loan> loans = new List<Loan>();
            mockLendingDa.Setup(x => x.GetAllActiveLoans(It.IsAny<Context>()))
                .Returns(loans);
            mockLendingDa.Setup(x => x.UpdateLoan(It.IsAny<Loan>(), It.IsAny<Context>()))
                .Returns(true);

            var mockMemberDa = new Mock<MemberDa_Code>();
            Member member = new Member();
            mockMemberDa.Setup(x => x.GetMember(It.IsAny<int>(), It.IsAny<Context>()))
                .Returns(member);

            var lendingDm = new LoaningDm_Code(mockLendingDa.Object, mockMemberDa.Object, context_Mock.Object);

            //Act
            var result = lendingDm.NoticeFilling();

            //Assert
            Assert.IsTrue(result == passing);
        }
        #endregion
    }
}
