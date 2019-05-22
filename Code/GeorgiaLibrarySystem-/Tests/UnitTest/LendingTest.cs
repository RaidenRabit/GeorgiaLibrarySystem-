using System.Collections.Generic;
using Core;
using GTLService.DataAccess.Code;
using GTLService.DataManagement.Code;
using Moq;
using NUnit.Framework;

namespace Tests.UnitTest
{
    public class LendingTest
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
        public void LendingDm_Code_LendBook(int allowedNumberOfBooks, int currentNumberOfBooks, bool borrowExists, int ssn, int copyId, bool passing)
        {
            //Arrange
            var mockLendingDa = new Mock<LendingDa_Code>(null);
            mockLendingDa.Setup(x => x.MemberBorrowedBooks(It.IsAny<int>()))
                .Returns(currentNumberOfBooks);
            Borrow borrow = null;
            if (borrowExists)
            {
                borrow = new Borrow();
            }
            mockLendingDa.Setup(x => x.GetBorrow(It.IsAny<int>()))
                .Returns(borrow);
            mockLendingDa.Setup(x => x.LendBook(It.IsAny<Borrow>()))
                .Returns(true);

            var mockMemberDa = new Mock<MemberDa_Code>(null);
            mockMemberDa.Setup(x => x.GetMember(It.IsAny<int>()))
                .Returns(new Member{MemberType = new MemberType{NrOfBooks = allowedNumberOfBooks}});

            var lendingDm = new LendingDm_Code(mockLendingDa.Object,mockMemberDa.Object);

            //Act
            var result = lendingDm.LendBook(ssn,copyId);

            //Assert
            Assert.IsTrue(result == passing);
        }

        [Test]
        //pass
        [TestCase(true, 1, true)]
        //fail
        [TestCase(false, 1, false)]
        public void LendingDm_Code_ReturnBook(bool borrowExists, int copyId, bool passing)
        {
            //Arrange
            var mockLendingDa = new Mock<LendingDa_Code>(null);
            Borrow borrow = null;
            if (borrowExists)
            {
                borrow = new Borrow();
            }
            mockLendingDa.Setup(x => x.GetBorrow(It.IsAny<int>()))
                .Returns(borrow);
            mockLendingDa.Setup(x => x.SaveBorrowChanges())
                .Returns(true);

            var mockMemberDa = new Mock<MemberDa_Code>(null);

            var lendingDm = new LendingDm_Code(mockLendingDa.Object,mockMemberDa.Object);

            //Act
            var result = lendingDm.ReturnBook(copyId);

            //Assert
            Assert.IsTrue(result == passing);
        }

        [Test]
        //pass
        [TestCase(true)]
        //fail
        public void LendingDm_Code_NoticeFilling(bool passing)
        {
            //Arrange
            var mockLendingDa = new Mock<LendingDa_Code>(null);
            List<Borrow> borrows = new List<Borrow>();
            mockLendingDa.Setup(x => x.GetAllActiveBorrows())
                .Returns(borrows);
            mockLendingDa.Setup(x => x.SaveBorrowChanges())
                .Returns(true);

            var mockMemberDa = new Mock<MemberDa_Code>(null);
            Member member = new Member();
            mockMemberDa.Setup(x => x.GetMember(It.IsAny<int>()))
                .Returns(member);

            var lendingDm = new LendingDm_Code(mockLendingDa.Object,mockMemberDa.Object);

            //Act
            var result = lendingDm.NoticeFilling();

            //Assert
            Assert.IsTrue(result == passing);
        }
        #endregion
    }
}
