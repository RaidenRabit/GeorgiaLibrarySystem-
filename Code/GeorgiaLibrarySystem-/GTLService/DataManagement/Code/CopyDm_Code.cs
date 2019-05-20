using System;
using GTLService.DataAccess.Code;
using GTLService.DataManagement.IDataManagement;

namespace GTLService.DataManagement.Code
{
    public class CopyDm_Code : ICopyDm
    {
        private CopyDa_Code _copyDa;
        private LendingDa_Code _lendingDa;

        public CopyDm_Code(CopyDa_Code copyDa, LendingDa_Code lendingDa)
        {
            _copyDa = copyDa;
            _lendingDa = lendingDa;
        }

        public int GetAvailableCopyId(int isbn)
        {
            var copies = _copyDa.GetAvailableCopyId(isbn);
            bool found = false;
            int i = 0;
            while (!found)
            {
                if (_lendingDa.GetBorrow(copies[i].CopyID) == null)
                    found = true;
                else
                {
                    i++;
                }
            }

            return copies[i].CopyID;
        }

        public int GetTotalNrCopies(int isbn)
        {
            return _copyDa.ReadCopies(isbn, "books").Count;
        }

        public int GetOutOnLoan(int isbn)
        {
            var copies = _copyDa.ReadCopies(isbn, "books");
            int count = 0;
            foreach (var copy in copies)
            {
                if (_lendingDa.GetBorrow(copy.CopyID) != null)
                    count++;
            }

            return count;
        }
    }
}