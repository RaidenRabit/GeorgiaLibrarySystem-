using System.ServiceModel;
using GTLService.DataManagement.IDataManagement;
using GTLService.Controller.IController;

namespace GTLService.Controller
{
    [ServiceBehavior(IncludeExceptionDetailInFaults = true, InstanceContextMode = InstanceContextMode.PerCall)] 
    public class LoginService : ILoginService
    {
        private readonly ILoginDm _loginDm;
        public LoginService(ILoginDm loginDm)
        {
            _loginDm = loginDm;
        }
        
        public bool Login(int ssn, string password)
        {
            return _loginDm.Login(ssn, password);
        }
    }
}