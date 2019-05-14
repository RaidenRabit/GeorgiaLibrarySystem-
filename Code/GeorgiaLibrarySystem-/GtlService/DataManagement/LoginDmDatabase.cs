using GtlService.DataManagement.iDataManagement;
using GtlService.DataAccess;

namespace GtlService.DataManagement
{
    public class LoginDmDatabase : ILoginDm
    {
        private readonly LoginDa _loginDa;

        public LoginDmDatabase(LoginDa loginDa)
        {
            this._loginDa = loginDa;
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

