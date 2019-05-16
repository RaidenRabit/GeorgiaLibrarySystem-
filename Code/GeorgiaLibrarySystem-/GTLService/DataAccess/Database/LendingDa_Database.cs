using System;
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
        public bool LendBook(int ssn, int copyId)
        {
            //todo test
            _context.Borrows.Add(new Borrow {SSN = ssn, CopyID = copyId, FromDate = new DateTime()});
            return _context.SaveChanges() > 0;
        }

        public bool ReturnBook(Borrow borrow)
        {
            //todo test
            //_context.Borrows.Attach(borrow);
            _context.Entry(borrow).State = System.Data.Entity.EntityState.Modified;
            return _context.SaveChanges() > 0;
        }

        public Borrow GetBorrow(int ssn, int copyId)
        {
            return _context.Borrows.FirstOrDefault(x => x.SSN == ssn && x.CopyID == copyId && x.ToDate == null);
        }
    }
}