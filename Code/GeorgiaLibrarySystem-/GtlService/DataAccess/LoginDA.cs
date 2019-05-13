using GtlService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GtlService.DataAccess
{
    public class LoginDA
    {
        private GTLEntities context;
        public LoginDA(GTLEntities context)
        {
            context = this.context;
        }

        public bool Login(int ssn, string password)
        {
            return context.People.Any(x => x.SSN == ssn && x.Password.Equals(password));
        }
    }
}
