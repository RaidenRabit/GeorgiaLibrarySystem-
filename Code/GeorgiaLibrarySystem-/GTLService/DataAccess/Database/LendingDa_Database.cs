using Core;
using GTLService.DataAccess.IDataAccess;

namespace GTLService.DataAccess.Database
{
    public class LendingDa_Database: ILendingDa
    {
        private readonly Context _context;
        public LendingDa_Database(Context context)
        {
            _context = context;
        }
        public bool LendBook(int ssn, int copyId)
        {
            //todo implement
            throw new System.NotImplementedException();
        }

        public bool ReturnBook(int ssn, int copyId)
        {
            //todo implement
            throw new System.NotImplementedException();
        }
    }
}