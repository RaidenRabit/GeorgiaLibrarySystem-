using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Core;
using GTLService.Controller;
using GTLService.DataAccess.Code;
using GTLService.DataAccess.Database;
using GTLService.DataManagement.Code;
using GTLService.DataManagement.Database;
using Moq;
using NUnit.Framework;

namespace Tests.IntegrationTest
{
    public class LendingTest
    {
        [Test]
        //pass
        [TestCase(1, 1, 1, true)]
        [TestCase(2, 1, 1, true)]
        //fail
        [TestCase(0, 1, 1, false)]
        public void LendingService_Database_LendBook(int nrOfChangesInDb, int ssn, int copyId, bool passing)
        {
            //Arrange
            var mock = new Mock<Context>();

            mock.Setup(x => x.Borrows.Add(It.IsAny<Borrow>()))
                .Returns(new Borrow());
            mock.Setup(x => x.SaveChanges())
                .Returns(nrOfChangesInDb);


            var loginService = new LendingService(new LendingDm_Database(new LendingDa_Database(mock.Object)));

            //Act
            var result = loginService.LendBook(ssn,copyId);

            //Assert
            Assert.IsTrue(result == passing);
        }

        [Test]
        //pass
        [TestCase(2, 5, 1, 1, 1, true)]
        //fail
        public void LendingService_Code_LendBook(int entriesInDb,int numberOfMemberBooks, int nrOfChangesInDb, int ssn, int copyId, bool passing)
        {
            //MemberBorrowedBooks 4
            var data = new List<Borrow>
            {
                new Borrow {CopyID = copyId, ToDate = new DateTime()},
                new Borrow {CopyID = 10, ToDate = null, SSN = ssn},
                new Borrow {CopyID = 11, ToDate = null, SSN = ssn},
                new Borrow {CopyID = 12, ToDate = null, SSN = ssn},
                new Borrow {CopyID = 13, ToDate = null, SSN = ssn}

            }.AsQueryable();
            var mockSet = new Mock<DbSet<Borrow>>();

            //GetMember
            var memberdata = new List<Member>
            {
                new Member {SSN = ssn, MemberType = new MemberType{ NrOfBooks = numberOfMemberBooks}}

            }.AsQueryable();
            var mockSet2 = new Mock<DbSet<Member>>();

            //Define the mock Repository as databaseEf
            var mock = new Mock<Context>();

            mockSet.As<IQueryable<Borrow>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Borrow>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Borrow>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Borrow>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            mockSet2.As<IQueryable<Member>>().Setup(m => m.Provider).Returns(memberdata.Provider);
            mockSet2.As<IQueryable<Member>>().Setup(m => m.Expression).Returns(memberdata.Expression);
            mockSet2.As<IQueryable<Member>>().Setup(m => m.ElementType).Returns(memberdata.ElementType);
            mockSet2.As<IQueryable<Member>>().Setup(m => m.GetEnumerator()).Returns(memberdata.GetEnumerator());

            //Setting up the mockSet to mockContext
            mock.Setup(c => c.Borrows).Returns(mockSet.Object);
            mock.Setup(c => c.Members).Returns(mockSet2.Object);

            //////////////////////////////////////////////////////////////////////////////////////////////////////
            
            //Arrange
                //LendBook
            mock.Setup(x => x.Borrows.Add(It.IsAny<Borrow>()))
                .Returns(new Borrow());
            mock.Setup(x => x.SaveChanges())
                .Returns(nrOfChangesInDb);

            var loginService = new LendingService(new LendingDm_Code(new LendingDa_Code(mock.Object), new MemberDa_Code(mock.Object)));

            //Act
            var result = loginService.LendBook(ssn,copyId);

            //Assert
            Assert.IsTrue(result == passing);
        }
    }
}
