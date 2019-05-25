using System.Data;
using Core;
using GTLService.DataAccess.Code;
using GTLService.DataManagement.IDataManagement;

namespace GTLService.DataManagement.Code
{
    public class LoginDm_Code : ILoginDm
    {
        private readonly LoginDa_Code _loginDa;
        private readonly Context _context;

        public LoginDm_Code(LoginDa_Code loginDa, Context context)
        {
            _loginDa = loginDa;
            _context = context;
        }

        public bool Login(int ssn, string password)
        {
            try
            {
                if (ssn.ToString().Length == 9 && password.Length <= 16 && password.Length > 0)
                {
                    return _loginDa.Login(ssn, password, _context);
                }

                return false;
            }
            catch
            {
                return false;
            }
        }
    }
}
