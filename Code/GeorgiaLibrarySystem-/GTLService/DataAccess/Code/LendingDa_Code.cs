﻿using System.Linq;
using Core;

namespace GTLService.DataAccess.Code
{
    public class LendingDa_Code
    {
        private readonly Context _context;
        public LendingDa_Code(Context context)
        {
            _context = context;
        }

        public virtual bool LendBook(Borrow borrow)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                bool result = false;
                try
                {
                    _context.Database.ExecuteSqlCommand("ALTER TABLE Borrow DISABLE TRIGGER Lending");//so trigger wouldn't be run
                    _context.SaveChanges();
                    _context.Borrows.Add(borrow);
                    result = _context.SaveChanges() > 0;
                    _context.Database.ExecuteSqlCommand("ALTER TABLE Borrow ENABLE TRIGGER Lending");
                    _context.SaveChanges();

                    dbContextTransaction.Commit();
                }
                catch
                {
                    dbContextTransaction.Rollback();
                }
                return result;
            }
        }
        
        public virtual bool SaveBorrowChanges()
        {
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