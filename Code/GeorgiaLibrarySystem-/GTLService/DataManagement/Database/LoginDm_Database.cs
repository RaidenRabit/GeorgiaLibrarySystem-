using GtlService.DataManagement.iDataManagement;
using GtlService.DataAccess.IDataAccess;

namespace GtlService.DataManagement.Database
{
    public class LoginDm_Database : ILoginDm
    {
        private readonly ILoginDa _loginDa;

        public LoginDm_Database(ILoginDa loginDa)
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

