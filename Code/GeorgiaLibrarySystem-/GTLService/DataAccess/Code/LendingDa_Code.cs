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

        public virtual bool LendBook(Borrow borrow)
        {
            _context.Borrows.Add(borrow);
            return _context.SaveChanges() > 0;
        }
        
        public virtual bool ReturnBook(Borrow borrow)
        {
            _context.Borrows.Attach(borrow);//todo is it needed?
            borrow.ToDate = DateTime.Now;
            return _context.SaveChanges() > 0;
        }

        public virtual Borrow GetBorrow(int copyId)
        {
            return _context.Borrows.FirstOrDefault(x => x.CopyID == copyId && x.ToDate == null);
        }

        public virtual int MemberBorrowedBooks(int ssn)
        {
            return _context.Borrows.Count(x => x.SSN == ssn && x.ToDate == null);
        }
    }
}