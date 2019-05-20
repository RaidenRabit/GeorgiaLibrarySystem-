using System.ServiceModel;

namespace GTLService.Controller
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ICopyService" in both code and config file together.
    [ServiceContract]
    public interface ICopyService
    {
        [OperationContract]
        int GetAvailableCopyId(int isbn);
        
        [OperationContract]
        int GetTotalNrCopies(int isbn);

        [OperationContract]
        int GetOutOnLoan(int isbn);
    }
}
