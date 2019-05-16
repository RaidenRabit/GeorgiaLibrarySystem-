namespace GTLService.DataAccess.IDataAccess
{
    public interface ILendingDa
    {
        bool LendBook(int ssn, int copyId);
        bool ReturnBook(int ssn, int copyId);
    }
}
