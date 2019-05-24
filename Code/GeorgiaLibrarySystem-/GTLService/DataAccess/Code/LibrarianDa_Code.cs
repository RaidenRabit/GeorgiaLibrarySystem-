using Core;

namespace GTLService.DataAccess.Code
{
    public class LibrarianDa_Code
    {
        public virtual bool CheckLibrarianSsn(int ssn, Context context)
        {
            return context.Librarians.Find(ssn) != null;
        }
    }
}