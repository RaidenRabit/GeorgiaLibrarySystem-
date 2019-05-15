using GtlService.Controller.iController;
using GtlService.DataAccess;
using GtlService.DataManagement;
using GtlService.DataManagement.iDataManagement;
using GtlService.Model;

namespace GtlService.Controller
{
    public class LoginController : ILoginController
    {
        private ILoginDm _loginDm;
        public LoginController(ILoginDm loginDm)
        {
            _loginDm = loginDm;
        }
        //fix here!
        public bool Login(int ssn, string password, int id)
        {
            //ILoginDm loginDm = new LoginDmFactory().Get(id);
            return _loginDm.Login(ssn, password);
        }
    }
    
    //new file?
    public class LoginDmFactory
    {
        public ILoginDm Get(int id)
        {
            switch (id)
            {
                case 0:
                    return new LoginDmDatabase(new LoginDaDatabase(new GTLEntities()));
                case 1:
                    return new LoginDmCode(new LoginDaCode(new GTLEntities()));;
                default:
                    return null;            
            }
        }
    }
}
