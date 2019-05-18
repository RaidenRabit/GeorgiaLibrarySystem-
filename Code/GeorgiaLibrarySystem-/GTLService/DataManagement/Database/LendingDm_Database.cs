using System;
using Core;
using GTLService.DataAccess.Database;
using GTLService.DataManagement.IDataManagement;

namespace GTLService.DataManagement.Database
{
    public class LendingDm_Database: ILendingDm
    {
        private readonly LendingDa_Database _lendingDa;

        public LendingDm_Database(LendingDa_Database lendingDa)
        {
            _lendingDa = lendingDa;
        }

        public bool LendBook(int ssn, int copyId)
        {
            try
            {
                return _lendingDa.LendBook(new Borrow {SSN = ssn, CopyID = copyId, FromDate = new DateTime()});
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
                return _lendingDa.ReturnBook(copyId);
            }
            catch
            {
                return false;
            }
        }
    }
}