using System;
using System.Linq;
using Core;
using GTLService.DataAccess.IDataAccess;

namespace GTLService.DataAccess.Code
{
    public class LendingDa_Code: ILendingDa
    {
        private readonly Context _context;
        public LendingDa_Code(Context context)
        {
            _context = context;
        }

        public bool LendBook(int ssn, int copyId)
        {
            _context.Borrows.Add(new Borrow {SSN = ssn, CopyID = copyId, FromDate = DateTime.Now});
            return _context.SaveChanges() > 0;
        }

        public bool ReturnBook(Borrow borrow)
        {
            _context.Borrows.Attach(borrow);
            borrow.ToDate = DateTime.Now;
            return _context.SaveChanges() > 0;
        }

        public Borrow GetBorrow(int copyId)
        {
            return _context.Borrows.FirstOrDefault(x => x.CopyID == copyId && x.ToDate == null);
        }

        public int MemberBorrowedBooks(int ssn)
        {
            return _context.Borrows.Count(x => x.SSN == ssn && x.ToDate == null);
        }
    }
}