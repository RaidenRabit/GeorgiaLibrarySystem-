using GtlService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GtlService.DataAccess
{
    public class LoginDa
    {
        private readonly GTLEntities _context;
        public LoginDa(GTLEntities context)
        {
            this._context = context;
        }

        public virtual bool Login(int ssn, string password)
        {
            return _context.People.Any(x => x.SSN == ssn && x.Password.Equals(password));
        }

        public virtual bool LoginDB(int ssn, string password)
        {
            //better?
            return bool.Parse(_context.Login(ssn,password).ToString());
        }
    }
}
