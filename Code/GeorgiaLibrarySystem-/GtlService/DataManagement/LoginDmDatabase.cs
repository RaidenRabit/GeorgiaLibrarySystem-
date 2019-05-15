using System;
using GtlService.DataManagement.iDataManagement;
using GtlService.DataAccess.IDataAccess;

namespace GtlService.DataManagement
{
    public class LoginDmDatabase : ILoginDm
    {
        private readonly ILoginDa _loginDa;

        public LoginDmDatabase(ILoginDa loginDa)
        {
            this._loginDa = loginDa;
        }

        public bool Login(int ssn, string password)
        {
            try
            {
                return _loginDa.Login(ssn, password);
            }
            catch(Exception e)
            {
                return false;
            }
        }
    }
}

