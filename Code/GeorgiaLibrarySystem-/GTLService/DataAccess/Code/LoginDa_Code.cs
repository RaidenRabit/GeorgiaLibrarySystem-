using System.Linq;
using Core;

namespace GTLService.DataAccess.Code
{
    public class LoginDa_Code
    {
        public virtual bool Login(int ssn, string password, Context context)
        {
            return context.People.Any(x => x.SSN == ssn && x.Password.Equals(password));
        }
    }
}
