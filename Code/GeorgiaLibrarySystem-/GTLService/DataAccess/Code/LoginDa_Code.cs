using Core;
using GTLService.DataAccess.IDataAccess;

namespace GTLService.DataAccess.Code
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
            Person person = _context.People.Find(ssn);
            return person != null && person.Password.Equals(password);
        }
    }
}
