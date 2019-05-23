using System.Collections.Generic;
using System.Linq;
using Core;

namespace GTLService.DataAccess.Code
{
    public class LoaningDa_Code
    {
        private readonly Context _context;
        public LoaningDa_Code(Context context)
        {
            _context = context;
        }

        public virtual bool LoanBook(Loan loan)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                bool result = false;
                try
                {
                    _context.Database.ExecuteSqlCommand("ALTER TABLE Loan DISABLE TRIGGER Lending");//so trigger wouldn't be run
                    _context.SaveChanges();
                    _context.Loans.Add(loan);
                    result = _context.SaveChanges() > 0;
                    _context.Database.ExecuteSqlCommand("ALTER TABLE Loan ENABLE TRIGGER Lending");
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
        
        public virtual bool SaveLoanChanges()
        {
            return _context.SaveChanges() > 0;
        }

        public virtual Loan GetLoan(int copyId)
        {
            return _context.Loans.FirstOrDefault(x => x.CopyID == copyId && x.ToDate == null);
        }

        public virtual List<Loan> GetAllActiveLoans()
        {
            return _context.Loans.Where(x => x.ToDate == null && x.noticeSent == null).ToList();
        }

        public virtual int MemberLoanBooks(int ssn)
        {
            return _context.Loans.Count(x => x.SSN == ssn && x.ToDate == null);
        }
    }
}