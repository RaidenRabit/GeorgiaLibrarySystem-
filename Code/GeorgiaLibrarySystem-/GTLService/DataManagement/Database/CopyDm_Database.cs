using System;
using GTLService.DataAccess.Database;
using GTLService.DataManagement.IDataManagement;

namespace GTLService.DataManagement.Database
{
    public class CopyDm_Database : ICopyDm
    {
        
        private readonly CopyDa_Database _copyDa;
        private readonly LendingDa_Database _lendingDa;

        public CopyDm_Database(CopyDa_Database copyDa, LendingDa_Database lendingDa)
        {
            _copyDa = copyDa;
            _lendingDa = lendingDa;
        }

        public int GetAvailableCopyId(int isbn)
        {
            var copies = _copyDa.GetAvailableCopies(isbn);
            foreach (var copy in copies)
            {
                if (_lendingDa.CheckAvailable(copy.CopyID))
                    return copy.CopyID;
            }

            return 0;
        }

        public int GetTotalNrCopies(int isbn)
        {
            return _copyDa.GetTotalNrCopies(isbn);
        }

        public int GetOutOnLoan(int isbn)
        {
            int count = 0;
            var copies = _copyDa.GetAvailableCopies(isbn);
            foreach (var copy in copies)
            {
                if (!_lendingDa.CheckAvailable(copy.CopyID))
                    count++;
            }

            return count;
        }
    }
}