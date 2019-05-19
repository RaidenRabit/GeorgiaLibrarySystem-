using System.ServiceModel;

namespace GTLService.Controller.IController
{
    [ServiceContract]
    public interface ILendingService
    {
        [OperationContract]
        bool LendBook(int ssn, int copyId);

        [OperationContract]
        bool ReturnBook(int copyId);
    }
}
