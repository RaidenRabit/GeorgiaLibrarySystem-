using System;
using GTLService.DataManagement.IDataManagement;

namespace GTLService.Controller
{

    public class CopyService : ICopyService
    {
        private ICopyDm _copyDm;

        public CopyService(ICopyDm copyDm)
        {
            _copyDm = copyDm;
        }

        public int GetAvailableCopyId(int isbn)
        {
            return _copyDm.GetAvailableCopyId(isbn);
        }

        public int GetTotalNrCopies(int isbn)
        {
            return _copyDm.GetTotalNrCopies(isbn);
        }

        public int GetOutOnLoan(int isbn)
        {
            return _copyDm.GetOutOnLoan(isbn);
        }
    }
}
