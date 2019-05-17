using Core;

namespace GTLService.DataAccess.IDataAccess
{
    public interface ILendingDa
    {
        bool LendBook(Borrow borrow);
        bool ReturnBook(Borrow borrow);
        Borrow GetBorrow(int copyId);
        int MemberBorrowedBooks(int ssn);
    }
}
