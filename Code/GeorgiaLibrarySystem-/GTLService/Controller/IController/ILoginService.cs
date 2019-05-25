using System.ServiceModel;

namespace GTLService.Controller.IController
{
    [ServiceContract]
    public interface ILoginService
    {
        [OperationContract]
        bool Login(int ssn, string password);
    }
}
