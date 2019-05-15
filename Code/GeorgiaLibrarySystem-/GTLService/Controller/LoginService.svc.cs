using Core;
using GtlService.DataAccess.Database;
using GtlService.DataManagement.Database;
using GtlService.DataManagement.iDataManagement;
using GTLService.Controller.IController;

namespace GTLService.Controller
{
    public class LoginService : ILoginService
    {
        private ILoginDm _loginDm;
        public LoginService()
        {
            _loginDm = new LoginDm_Database(new LoginDa_Database(new Context()));
        }
        public LoginService(ILoginDm loginDm)
        {
            if(loginDm != null)
                _loginDm = loginDm;
        }

        //todo fix here!
        public bool Login(int ssn, string password)
        {
            //ILoginDm loginDm = new LoginDmFactory().Get(id);
            return _loginDm.Login(ssn, password);
        }
    }
}

//new file?
//public class LoginDmFactory
//{
//    public ILoginDm Get(int id)
//    {
//        switch (id)
//        {
//            case 0:
//                return new LoginDmDatabase(new LoginDaDatabase(new GTLEntities()));
//            case 1:
//                return new LoginDmCode(new LoginDa_Code(new GTLEntities()));;
//            default:
//                return null;            
//        }
//    }
//}