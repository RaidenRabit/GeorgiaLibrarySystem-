using System;
using Core;
using GTLService.DataAccess.Database;
using GTLService.DataManagement.IDataManagement;

namespace GTLService.DataManagement.Database
{
    public class LoaningDm_Database: ILoaningDm
    {
        private readonly LoaningDa_Database _loaningDa;

        public LoaningDm_Database(LoaningDa_Database loaningDa)
        {
            _loaningDa = loaningDa;
        }

        public bool LoanBook(int ssn, int copyId)
        {
            try
            {
                return _loaningDa.LoanBook(new Loan {SSN = ssn, CopyID = copyId, FromDate = new DateTime()});
            }
            catch
            {
                return false;
            }
        }

        public bool ReturnBook(int copyId)
        {
            try
            {
                return _loaningDa.ReturnBook(copyId);
            }
            catch
            {
                return false;
            }
        }
    }
}