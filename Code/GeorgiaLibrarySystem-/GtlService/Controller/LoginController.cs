using GtlService.Controller.iController;
using GtlService.DataAccess;
using GtlService.DataManagement;
using GtlService.DataManagement.iDataManagement;
using GtlService.Model;

namespace GtlService.Controller
{
    public class LoginController : ILoginController
    {
        //dependency injection!
        private readonly ILoginDm _loginDm = new LoginDmCode(new LoginDa(new GTLEntities()));
        //public LoginController(LoginDmCode loginDm)
        //{
        //    loginDM = loginDm;
        //}

        public bool Login(int ssn, string password)
        {
            return _loginDm.Login(ssn, password);
        }
    }

    //work here
    public class LoginDmFactory
    {
        public ILoginDm Get(int id)
        {
            switch (id)
            {
                case 0:
                    return new LoginDmCode(new LoginDa(new GTLEntities()));
                case 1:
                    return null;
                default:
                    return null;            
            }
        }
    }
}
