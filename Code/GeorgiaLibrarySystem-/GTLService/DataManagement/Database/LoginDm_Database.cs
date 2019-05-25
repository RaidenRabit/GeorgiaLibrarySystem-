using GTLService.DataAccess.Database;
using GTLService.DataManagement.IDataManagement;

namespace GTLService.DataManagement.Database
{
    public class LoginDm_Database : ILoginDm
    {
        private readonly LoginDa_Database _loginDa;

        public LoginDm_Database(LoginDa_Database loginDa)
        {
            _loginDa = loginDa;
        }

        public bool Login(int ssn, string password)
        {
            try
            {
                return _loginDa.Login(ssn, password);
            }
            catch
            {
                return false;
            }
        }
    }
}

