
namespace GTLService.DataManagement.IDataManagement
{
    public interface ICopyDm
    {
        int GetAvailableCopyId(string isbn);

        int GetTotalNrCopies(string isbn);

        int GetOutOnLoan(string isbn);

        bool DeleteCopy(int ssn, int copyId);
    }
}
