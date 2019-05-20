using Core;

namespace GTLService.DataAccess.Code
{
    public class PersonDa_Code
    {
        private readonly Context _context;
        public PersonDa_Code(Context context)
        {
            _context = context;
        }

        public virtual bool CheckLibrarianSsn(int ssn)
        {
            var person = _context.Librarians.Find(ssn);
            return person != null;
        }
    }
}