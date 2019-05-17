using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
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
        #region Database
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
        [TestCase(1, 1, true)]
        //fail
        [TestCase(0, 1, false)]//no changes in database
        public void LendingService_Database_ReturnBook(int nrOfChangesInDb, int copyId, bool passing)
        {
            var mock = new Mock<Context>();
            mock.Setup(x => x.Returning(It.IsAny<int>())).Returns(nrOfChangesInDb);

            var loginService = new LendingService(new LendingDm_Database(new LendingDa_Database(mock.Object)));

            //Act
            var result = loginService.ReturnBook(copyId);

            //Assert
            Assert.IsTrue(result == passing);
        }
        #endregion

        #region Code
        [Test]
        //pass
        [TestCase(5, 1, 1, 1, true)]//member with 4 books borrows 5th
        //fail
        [TestCase(5, 0, 1, 1, false)]//no changes in database
        [TestCase(4, 1, 1, 1, false)]//Too many books borrowed
        [TestCase(5, 1, 1, 2, false)]//book already lent to you
        [TestCase(5, 1, 2, 2, false)]//book already lent to other member
        [TestCase(5, 1, 0, 1, false)]//Person doesn't exist
        public void LendingService_Code_LendBook(int memberBookLimit, int nrOfChangesInDb, int ssn, int copyId, bool passing)
        {
            var borrows = new List<Borrow>
            {
                new Borrow {CopyID = 1, ToDate = new DateTime(), SSN = 1},
                new Borrow {CopyID = 2, ToDate = null, SSN = 1},
                new Borrow {CopyID = 3, ToDate = null, SSN = 1},
                new Borrow {CopyID = 4, ToDate = null, SSN = 1},
                new Borrow {CopyID = 5, ToDate = null, SSN = 1}
            }.AsQueryable();

            var members = new List<Member>
            {
                new Member {SSN = 1, MemberType = new MemberType {NrOfBooks = memberBookLimit}},
                new Member {SSN = 2, MemberType = new MemberType {NrOfBooks = memberBookLimit}}
            }.AsQueryable();
 
            var dbBorrowsSet = Create(borrows);
            var dbMembersSet = Create(members);
 
            var mock = new Mock<Context>();
            mock.Setup(x => x.Borrows).Returns(dbBorrowsSet.Object);
            mock.Setup(x => x.Members.Include(It.IsAny<string>())).Returns(dbMembersSet.Object);
            mock.Setup(x => x.SaveChanges())
                .Returns(nrOfChangesInDb);

            var loginService = new LendingService(new LendingDm_Code(new LendingDa_Code(mock.Object), new MemberDa_Code(mock.Object)));

            //Act
            var result = loginService.LendBook(ssn,copyId);

            //Assert
            Assert.IsTrue(result == passing);
        }
        
        [Test]
        //pass
        [TestCase(1, 2, true)]
        //fail
        [TestCase(0, 2, false)]//no changes in database
        [TestCase(1, 1, false)]//Book not lent out at the moment
        [TestCase(1, 0, false)]//Book doesn't exist
        public void LendingService_Code_ReturnBook(int nrOfChangesInDb, int copyId, bool passing)
        {
            var borrows = new List<Borrow>
            {
                new Borrow {CopyID = 1, ToDate = new DateTime(), SSN = 1},
                new Borrow {CopyID = 2, ToDate = null, SSN = 1},
                new Borrow {CopyID = 3, ToDate = null, SSN = 1},
                new Borrow {CopyID = 4, ToDate = null, SSN = 1},
                new Borrow {CopyID = 5, ToDate = null, SSN = 1}
            }.AsQueryable();
 
            var dbBorrowsSet = Create(borrows);
 
            var mock = new Mock<Context>();
            mock.Setup(x => x.Borrows).Returns(dbBorrowsSet.Object);
            mock.Setup(x => x.SaveChanges())
                .Returns(nrOfChangesInDb);

            var loginService = new LendingService(new LendingDm_Code(new LendingDa_Code(mock.Object), new MemberDa_Code(mock.Object)));

            //Act
            var result = loginService.ReturnBook(copyId);

            //Assert
            Assert.IsTrue(result == passing);
        }
        #endregion

        private static Mock<DbSet<T>> Create<T>(IEnumerable<T> data) where T : class
        {
            var queryable = data.AsQueryable();
            var mock = new Mock<DbSet<T>>();
            mock.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
            mock.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            mock.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            mock.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());
 
            return mock;
        }
    }
}
