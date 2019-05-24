using System.ServiceModel;

namespace GTLService.Controller.IController
{
    [ServiceContract]
    public interface ILoaningService
    {
        [OperationContract]
        bool LoanBook(int ssn, int copyId);

        [OperationContract]
        bool ReturnBook(int copyId);
    }
}
