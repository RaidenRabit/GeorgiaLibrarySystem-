using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;
using GTLService.DataAccess.Code;
using GTLService.DataManagement.Code;
using Moq;
using NUnit.Framework;

namespace Tests.UnitTest
{
    public class NoticeTest
    {
        [Test]
        //pass
        [TestCase(true)]
        //fail
        public void LendingDm_Code_ReturnBook(bool passing)
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

            var lendingDm = new NoticeDm_Code(mockLendingDa.Object,mockMemberDa.Object);

            //Act
            var result = lendingDm.NoticeFilling();

            //Assert
            Assert.IsTrue(result == passing);
        }
    }
}
