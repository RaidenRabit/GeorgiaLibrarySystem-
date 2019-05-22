using System.ServiceModel;

namespace GTLService.Controller
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ICopyService" in both code and config file together.
    [ServiceContract]
    public interface ICopyService
    {
        [OperationContract]
        int GetAvailableCopyId(string isbn);
        
        [OperationContract]
        int GetTotalNrCopies(string isbn);

        [OperationContract]
        int GetOutOnLoan(string isbn);
    }
}
