using Core;
using GTLService.DataAccess.IDataAccess;

namespace GTLService.DataAccess.Database
{
    public class PersonDa_Code : IPersonDa
    {
        private readonly Context _context;
        public PersonDa_Code(Context context)
        {
            this._context = context;
        }

        public bool CheckLibrarianSsn(int ssn)
        {
            var person = _context.Librarians.Find(ssn);
            return person != null;
        }

        public bool CheckMemberSsn(int ssn)
        {
            var person = _context.Members.Find(ssn);
            return person != null;
        }
    }
}