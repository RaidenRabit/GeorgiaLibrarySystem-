using System.Linq;
using Core;
using GtlService.DataAccess.IDataAccess;

namespace GtlService.DataAccess.Code
{
    public class LoginDa_Code: ILoginDa
    {
        private readonly Context _context;
        public LoginDa_Code(Context context)
        {
            this._context = context;
        }

        public virtual bool Login(int ssn, string password)
        {
            return _context.People.Any(x => x.SSN == ssn && x.Password.Equals(password));
        }
    }
}
