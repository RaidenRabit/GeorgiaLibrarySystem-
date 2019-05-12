using GtlService.DataManagement.iDataManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GtlService.DataAccess;

namespace GtlService.DataManagement
{
    public class LoginDM : ILoginDM
    {
        private LoginDA loginDA;

        public LoginDM(LoginDA loginDa)
        {
            loginDA = loginDa;
        }

        public bool Login(int ssn, string password)
        {
            try
            {
                return loginDA.Login(ssn, password);
            }
            catch
            {
                return false;
            }
        }
    }
}
