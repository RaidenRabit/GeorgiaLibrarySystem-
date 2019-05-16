using System;
using Core;
using System.Linq;
using GTLService.DataAccess.IDataAccess;

namespace GTLService.DataAccess.Database
{
    public class LoginDa_Database: ILoginDa
    {
        private readonly Context _context;
        public LoginDa_Database(Context context)
        {
            this._context = context;
        }

        public virtual bool Login(int ssn, string password)
        {
            var value = _context.Login(ssn, password).First();
            return Convert.ToBoolean(value);
        }
    }
}
