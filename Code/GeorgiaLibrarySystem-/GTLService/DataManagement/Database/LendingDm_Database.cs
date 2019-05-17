using System;
using Core;
using GTLService.DataAccess.IDataAccess;
using GTLService.DataManagement.IDataManagement;

namespace GTLService.DataManagement.Database
{
    public class LendingDm_Database: ILendingDm
    {
        private readonly ILendingDa _lendingDa;

        public LendingDm_Database(ILendingDa lendingDa)
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
                return _lendingDa.ReturnBook(new Borrow{CopyID = copyId});
            }
            catch
            {
                return false;
            }
        }
    }
}