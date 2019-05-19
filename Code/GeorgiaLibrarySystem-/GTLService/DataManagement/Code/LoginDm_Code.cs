using GTLService.DataAccess.Code;
using GTLService.DataManagement.IDataManagement;

namespace GTLService.DataManagement.Code
{
    public class LoginDm_Code : ILoginDm
    {
        private readonly LoginDa_Code _loginDa;

        public LoginDm_Code(LoginDa_Code loginDa)
        {
            _loginDa = loginDa;
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
