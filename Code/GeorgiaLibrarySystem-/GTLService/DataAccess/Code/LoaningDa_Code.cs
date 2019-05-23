using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using Core;

namespace GTLService.DataAccess.Code
{
    public class LoaningDa_Code
    {
        public virtual bool LoanBook(Loan loan, Context context)
        {
            context.Database.ExecuteSqlCommand("ALTER TABLE Loan DISABLE TRIGGER Lending");//so trigger wouldn't be run
            context.SaveChanges();
            context.Loans.Add(loan);
            bool result = context.SaveChanges() > 0;
            context.Database.ExecuteSqlCommand("ALTER TABLE Loan ENABLE TRIGGER Lending");
            context.SaveChanges();
            return result;
        }

        public virtual Loan GetLoan(int copyId, Context context)
        {
            return context.Loans.FirstOrDefault(x => x.CopyID == copyId && x.ToDate == null);
        }

        public virtual bool UpdateLoan(Loan loan, Context context)
        {
            context.Loans.AddOrUpdate(loan);
            return context.SaveChanges() > 0;
        }

        public virtual List<Loan> GetAllActiveLoans(Context context)
        {
            return context.Loans.Where(x => x.ToDate == null && x.noticeSent == null).ToList();
        }

        public virtual int MemberLoanBooks(int ssn, Context context)
        {
            return context.Loans.Count(x => x.SSN == ssn && x.ToDate == null);
        }
    }
}