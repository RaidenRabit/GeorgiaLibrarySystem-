using System;
using GTLService.DataAccess.Database;
using GTLService.DataManagement.IDataManagement;

namespace GTLService.DataManagement.Database
{
    public class CopyDm_Database : ICopyDm
    {
        
        private readonly CopyDa_Database _copyDa;
        private readonly LoaningDa_Database _loaningDa;

        public CopyDm_Database(CopyDa_Database copyDa, LoaningDa_Database loaningDa)
        {
            _copyDa = copyDa;
            _loaningDa = loaningDa;
        }

        public int GetAvailableCopyId(string isbn)
        {
            var copies = _copyDa.GetAvailableCopies(isbn);
            foreach (var copy in copies)
            {
                if (_loaningDa.CheckAvailable(copy.CopyID))
                    return copy.CopyID;
            }

            return 0;
        }

        public int GetTotalNrCopies(string isbn)
        {
            return _copyDa.GetTotalNrCopies(isbn);
        }

        public int GetOutOnLoan(string isbn)
        {
            int count = 0;
            var copies = _copyDa.GetAvailableCopies(isbn);
            foreach (var copy in copies)
            {
                if (!_loaningDa.CheckAvailable(copy.CopyID))
                    count++;
            }

            return count;
        }
    }
}