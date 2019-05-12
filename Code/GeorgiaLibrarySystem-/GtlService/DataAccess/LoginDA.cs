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
        public bool Login(int ssn, string password)
        {
            using (GTLEntities context = new GTLEntities())
            {
                return context.People.Any(x => x.SSN == ssn && x.Password.Equals(password));
            }
        }
    }
}
