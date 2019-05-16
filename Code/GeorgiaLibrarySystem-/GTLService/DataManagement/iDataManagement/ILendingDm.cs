namespace GTLService.DataManagement.IDataManagement
{
    public interface ILendingDm
    {
        bool LendBook(int ssn, int copyId);
        bool ReturnBook(int ssn, int copyId);
    }
}
