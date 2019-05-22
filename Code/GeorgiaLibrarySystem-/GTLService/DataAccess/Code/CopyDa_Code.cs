using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Core;

namespace GTLService.DataAccess.Code
{
    public class CopyDa_Code
    {
        private readonly Context _context;
        public CopyDa_Code(Context context)
        {
            _context = context;
        }

        public virtual bool CheckCopyId(int id)
        {
            return _context.Copies.Find(id) != null;
        }

        public virtual bool CheckTypeName(string typeName)
        {
            return _context.Copies.FirstOrDefault(x => x.TypeName.Equals(typeName)) != null;
        }
        
        public virtual bool CreateCopy(string isbn, string libraryName, string typeName)
        {
            try
            {
                Copy copy = new Copy {ISBN = isbn, LibraryName = libraryName, TypeName = typeName};
                _context.Copies.Add(copy);
                return _context.SaveChanges() > 0;
            }
            catch (SqlException)
            {
                return false;
            }
        }
        
        public virtual List<Copy> ReadCopies(string isbn, string typeName)
        {
            var a = _context.Copies
                .Where(x => (isbn.Equals("0") || x.ISBN.Equals(isbn)) &&
                            (typeName.Contains("0") || x.TypeName.Equals(typeName))
                )
                .ToList();
            return a;
        }
        
        public virtual bool DeleteCopy(int copyId)
        {
            try
            {
                var copy = _context.Copies.Single(o => o.CopyID == copyId);
                _context.Copies.Remove(copy);
                return _context.SaveChanges() > 0;
            }
            catch (SqlException)
            {
                return false;
            }
        }

        public virtual List<Copy> GetAvailableCopyId(string isbn)
        {
            try
            {
                return _context.Copies.Where(x => x.ISBN.Equals(isbn)).ToList();
            }
            catch (SqlException)
            {
                return null;
            }
        }
    }
}