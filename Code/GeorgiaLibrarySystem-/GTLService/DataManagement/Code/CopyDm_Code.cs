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

        public int GetAvailableCopyId(string isbn)
        {
            var copies = _copyDa.GetAvailableCopyId(isbn);
            int i = 0;
            if(copies.Count > 0)
                while (!false)
                {
                    if (_lendingDa.GetBorrow(copies[i].CopyID) == null)
                    {
                        return copies[i].CopyID;
                    }
                    else
                    {
                        i++;
                    }
                }

            return 0;
        }

        public int GetTotalNrCopies(string isbn)
        {
            return _copyDa.ReadCopies(isbn, "0").Count;
        }

        public int GetOutOnLoan(string isbn)
        {
            var copies = _copyDa.ReadCopies(isbn, "0");
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