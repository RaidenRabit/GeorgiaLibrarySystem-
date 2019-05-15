using GtlService.Model;
using System.Linq;
using GtlService.DataAccess.IDataAccess;

namespace GtlService.DataAccess
{
    public class LoginDaCode: ILoginDa
    {
        private readonly GTLEntities _context;
        public LoginDaCode(GTLEntities context)
        {
            this._context = context;
        }

        public virtual bool Login(int ssn, string password)
        {
            return _context.People.Any(x => x.SSN == ssn && x.Password.Equals(password));
        }
    }
}
