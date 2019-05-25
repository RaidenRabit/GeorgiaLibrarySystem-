using System.Linq;
using Core;

namespace GTLService.DataAccess.Database
{
    public class LoaningDa_Database
    {
        private readonly Context _context;
        public LoaningDa_Database(Context context)
        {
            _context = context;
        }

        public bool LoanBook(Loan loan)
        {
            _context.Loans.Add(loan);
            return _context.SaveChanges() > 0;
        }

        public bool ReturnBook(int copyId)
        {
            return _context.Returning(copyId) > 0;
        }

        public bool CheckAvailable(int copyId)
        {
            return _context.Loans.FirstOrDefault(x => x.CopyID == copyId && x.ToDate == null) == null;
        }
    }
}