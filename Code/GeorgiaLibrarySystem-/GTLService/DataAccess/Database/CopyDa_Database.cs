
using System.Collections.Generic;
using System.Linq;
using Core;

namespace GTLService.DataAccess.Database
{
    public class CopyDa_Database
    {
        private readonly Context _context;
        public CopyDa_Database(Context context)
        {
            _context = context;
        }

        public List<Copy> GetAvailableCopies(string isbn)
        {
            return _context.Copies
                .Where(x => x.ISBN.Equals(isbn.ToString()))
                .ToList();
        }

        public int GetTotalNrCopies(string isbn)
        {
            return _context.Copies.Count(x => x.ISBN.Equals(isbn.ToString()));
        }
    }
}