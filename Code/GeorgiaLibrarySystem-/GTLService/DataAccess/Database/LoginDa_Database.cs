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
            _context = context;
        }

        public virtual bool Login(int ssn, string password)
        {
            return Convert.ToBoolean(_context.Login(ssn, password).First());
        }
    }
}
