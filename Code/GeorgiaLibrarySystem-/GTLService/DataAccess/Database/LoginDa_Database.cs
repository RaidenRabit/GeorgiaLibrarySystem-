using System;
using Core;
using System.Linq;

namespace GTLService.DataAccess.Database
{
    public class LoginDa_Database
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
