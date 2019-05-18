using Core;

namespace GTLService.DataAccess.Code
{
    public class LibraryDa_Code
    {
        private readonly Context _context;
        public LibraryDa_Code(Context context)
        {
            this._context = context;
        }

        public virtual bool CheckLibraryName(string libraryName)
        {
            var library = _context.Libraries.Find(libraryName);
            return library != null;
        }
    }
}