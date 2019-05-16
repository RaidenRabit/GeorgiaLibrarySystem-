using GTLService.DataManagement.IDataManagement;
using GTLService.DataAccess.IDataAccess;

namespace GTLService.DataManagement.Code
{
    public class LoginDm_Code : ILoginDm
    {
        private readonly ILoginDa _loginDa;

        public LoginDm_Code(ILoginDa loginDa)
        {
            this._loginDa = loginDa;
        }

        public bool Login(int ssn, string password)
        {
            try
            {
                if (ssn.ToString().Length == 9 && password.Length <= 16 && password.Length > 0)
                    return _loginDa.Login(ssn, password);
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }
    }
}
