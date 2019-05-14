﻿using GtlService.DataManagement.iDataManagement;
using GtlService.DataAccess;

namespace GtlService.DataManagement
{
    public class LoginDmCode : ILoginDm
    {
        private readonly LoginDa _loginDa;

        public LoginDmCode(LoginDa loginDa)
        {
            this._loginDa = loginDa;
        }

        public bool Login(int ssn, string password)
        {
            try
            {
                //8 chars in ssn and password from 1-16
                if (ssn > 9999999 && ssn < 100000000 && password.Length <= 16 && password.Length > 0)
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