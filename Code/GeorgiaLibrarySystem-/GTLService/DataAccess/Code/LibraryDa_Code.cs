using Core;

namespace GTLService.DataAccess.Code
{
    public class LibraryDa_Code
    {
        private readonly Context _context;
        public LibraryDa_Code(Context context)
        {
            _context = context;
        }

        public virtual bool CheckLibraryName(string libraryName)
        {
            return _context.Libraries.Find(libraryName) != null;
        }
    }
}