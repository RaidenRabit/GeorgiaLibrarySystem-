using System;
using System.Data.Entity;
using System.Linq;
using Core;
using GTLService.DataAccess.IDataAccess;

namespace GTLService.DataAccess.Database
{
    public class LendingDa_Database: ILendingDa
    {
        private readonly Context _context;
        public LendingDa_Database(Context context)
        {
            _context = context;
        }

        public bool LendBook(Borrow borrow)
        {
            _context.Borrows.Add(borrow);
            return _context.SaveChanges() > 0;
        }

        public bool ReturnBook(Borrow borrow)
        {
            return _context.Returning(borrow.CopyID) > 0;
        }

        public Borrow GetBorrow(int copyId)
        {
            throw new NotImplementedException();
        }

        public int MemberBorrowedBooks(int ssn)
        {
            throw new NotImplementedException();
        }
    }
}