using Core;

namespace GTLService.DataAccess.IDataAccess
{
    public interface ILendingDa
    {
        bool LendBook(int ssn, int copyId);
        bool ReturnBook(Borrow borrow);
        Borrow GetBorrow(int ssn, int copyId);
    }
}
