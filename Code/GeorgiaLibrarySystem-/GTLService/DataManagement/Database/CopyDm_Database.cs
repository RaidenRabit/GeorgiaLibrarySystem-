using System;
using GTLService.DataManagement.IDataManagement;

namespace GTLService.DataManagement.Database
{
    public class CopyDm_Database : ICopyDm
    {
        public int GetAvailableCopyId(int isbn)
        {
            throw new NotImplementedException();
        }

        public int GetTotalNrCopies(int isbn)
        {
            throw new NotImplementedException();
        }

        public int GetOutOnLoan(int isbn)
        {
            throw new NotImplementedException();
        }
    }
}