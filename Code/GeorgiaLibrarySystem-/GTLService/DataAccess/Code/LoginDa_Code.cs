using System.Linq;
using Core;

namespace GTLService.DataAccess.Code
{
    public class LoginDa_Code
    {
        private readonly Context _context;
        public LoginDa_Code(Context context)
        {
            _context = context;
        }

        public virtual bool Login(int ssn, string password)
        {
            return _context.People.Any(x => x.SSN == ssn && x.Password == password);
        }
    }
}
