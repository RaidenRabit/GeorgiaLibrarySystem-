using System;
using System.Collections.Generic;
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
        [TestCase(123456789, 10, false, true)]//member with 4 books borrows 5th
        //fail
        [TestCase(123456789, 10, true, false)]//Too many books borrowed
        [TestCase(123456789, 11, false, false)]//book already lent to you
        [TestCase(123456789, 4, false, false)]//book already lent to other member
        [TestCase(1, 10, false, false)]//Person doesn't exist
        public void LendingService_Database_LendBook(int ssn, int copyId, bool maxNumberOfBooks, bool passing)
        {
            //Arrange
            var loginService = new LendingService(new LendingDm_Database(new LendingDa_Database(FillBorrowDatabase(maxNumberOfBooks))));

            //Act
            var result = loginService.LendBook(ssn, copyId);

            //Assert
            Assert.IsTrue(result == passing);
        }

        [Test]
        //pass
        [TestCase(5, true)]//return taken book
        //fail
        [TestCase(0, false)]//wrong copyId
        [TestCase(9, false)]//copy already returned
        public void LendingService_Database_ReturnBook(int copyId, bool passing)
        {
            //Arrange
            var loginService = new LendingService(new LendingDm_Database(new LendingDa_Database(FillBorrowDatabase(false))));

            //Act
            var result = loginService.ReturnBook(copyId);

            //Assert
            Assert.IsTrue(result == passing);
        }

        #endregion

        #region Code

        [Test]
        //pass
        [TestCase(5, 1, 1, 1, true)] //member with 4 books borrows 5th
        //fail
        [TestCase(5, 0, 1, 1, false)] //no changes in database
        [TestCase(4, 1, 1, 1, false)] //Too many books borrowed
        [TestCase(5, 1, 1, 2, false)] //book already lent to you
        [TestCase(5, 1, 2, 2, false)] //book already lent to other member
        [TestCase(5, 1, 0, 1, false)] //Person doesn't exist
        public void LendingService_Code_LendBook(int memberBookLimit, int nrOfChangesInDb, int ssn, int copyId,
            bool passing)
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

            var dbBorrowsSet = DbContextMock.CreateDbSetMock(borrows);
            var dbMembersSet = DbContextMock.CreateDbSetMock(members);

            var mock = new Mock<Context>();
            mock.Setup(x => x.Borrows).Returns(dbBorrowsSet.Object);
            mock.Setup(x => x.Members.Include(It.IsAny<string>())).Returns(dbMembersSet.Object);
            mock.Setup(x => x.SaveChanges())
                .Returns(nrOfChangesInDb);

            var loginService =
                new LendingService(new LendingDm_Code(new LendingDa_Code(mock.Object), new MemberDa_Code(mock.Object)));

            //Act
            var result = loginService.LendBook(ssn, copyId);

            //Assert
            Assert.IsTrue(result == passing);
        }

        [Test]
        //pass
        [TestCase(1, 2, true)]
        //fail
        [TestCase(0, 2, false)] //no changes in database
        [TestCase(1, 1, false)] //Book not lent out at the moment
        [TestCase(1, 0, false)] //Book doesn't exist
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

            var dbBorrowsSet = DbContextMock.CreateDbSetMock(borrows);

            var mock = new Mock<Context>();
            mock.Setup(x => x.Borrows).Returns(dbBorrowsSet.Object);
            mock.Setup(x => x.SaveChanges())
                .Returns(nrOfChangesInDb);

            var loginService =
                new LendingService(new LendingDm_Code(new LendingDa_Code(mock.Object), new MemberDa_Code(mock.Object)));

            //Act
            var result = loginService.ReturnBook(copyId);

            //Assert
            Assert.IsTrue(result == passing);
        }

        #endregion

        private static Context FillBorrowDatabase(bool maxNumberOfBooks)
        {
            string text = "GETDATE()";
            if(maxNumberOfBooks)
            {
                text = "null";
            }
            Context context = new Context();
            context.Database.ExecuteSqlCommand("TRUNCATE TABLE Borrow");
            context.Database.ExecuteSqlCommand("INSERT INTO Borrow (CopyID, SSN, FromDate, ToDate)"+
                                               "VALUES (5,123456786,GETDATE(),null),"+
                                               "(3,123456788,GETDATE(),null),"+
                                               "(2,123456786,GETDATE(),GETDATE()),"+
                                               "(6,123456789,GETDATE(),null),"+
                                               "(1,123456789,GETDATE(),GETDATE()),"+
                                               "(4,123456786,GETDATE(),null),"+
                                               "(7,123456789,GETDATE(),null),"+
                                               "(3,123456789,GETDATE(),"+text+"),"+
                                               "(9,123456786,GETDATE(),GETDATE()),"+
                                               "(2,123456789,GETDATE(),null),"+
                                               "(11,123456789,GETDATE(),null);");
            context.SaveChanges();
            return context;
        }
    }
}
