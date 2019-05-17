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
                return _lendingDa.LendBook(ssn, copyId);
            }
            catch
            {
                return false;
            }
        }

        public bool ReturnBook(int ssn, int copyId)
        {
            try
            {
                return _lendingDa.ReturnBook(_lendingDa.GetBorrow(copyId));
            }
            catch
            {
                return false;
            }
        }
    }
}