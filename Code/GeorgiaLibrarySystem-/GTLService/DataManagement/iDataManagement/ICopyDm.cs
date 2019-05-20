
namespace GTLService.DataManagement.IDataManagement
{
    public interface ICopyDm
    {
        int GetAvailableCopyId(int isbn);

        int GetTotalNrCopies(int isbn);

        int GetOutOnLoan(int isbn);
    }
}
