using System;
using GTLService.Controller.IController;
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

        public int GetAvailableCopyId(string isbn)
        {
            return _copyDm.GetAvailableCopyId(isbn);
        }

        public int GetTotalNrCopies(string isbn)
        {
            return _copyDm.GetTotalNrCopies(isbn);
        }

        public int GetOutOnLoan(string isbn)
        {
            return _copyDm.GetOutOnLoan(isbn);
        }

        public bool DeleteCopy(int ssn, int copyId)
        {
            return _copyDm.DeleteCopy(ssn, copyId);
        }
    }
}
