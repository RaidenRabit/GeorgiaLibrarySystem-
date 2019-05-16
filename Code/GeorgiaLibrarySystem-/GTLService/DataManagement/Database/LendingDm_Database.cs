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
            return _lendingDa.LendBook(ssn, copyId);
        }

        public bool ReturnBook(int ssn, int copyId)
        {
            return _lendingDa.ReturnBook(ssn, copyId);
        }
    }
}