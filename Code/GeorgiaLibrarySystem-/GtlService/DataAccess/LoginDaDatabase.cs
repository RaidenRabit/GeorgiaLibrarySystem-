using System;
using System.Linq;
using GtlService.DataAccess.IDataAccess;
using GtlService.Model;

namespace GtlService.DataAccess
{
    public class LoginDaDatabase: ILoginDa
    {
        private readonly GTLEntities _context;
        public LoginDaDatabase(GTLEntities context)
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
