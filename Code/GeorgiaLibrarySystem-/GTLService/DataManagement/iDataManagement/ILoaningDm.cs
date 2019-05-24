namespace GTLService.DataManagement.IDataManagement
{
    public interface ILoaningDm
    {
        bool LoanBook(int ssn, int copyId);
        bool ReturnBook(int copyId);
    }
}
