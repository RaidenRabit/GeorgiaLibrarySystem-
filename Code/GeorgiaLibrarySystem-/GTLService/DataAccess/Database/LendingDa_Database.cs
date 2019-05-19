using Core;

namespace GTLService.DataAccess.Database
{
    public class LendingDa_Database
    {
        private readonly Context _context;
        public LendingDa_Database(Context context)
        {
            _context = context;
        }

        public bool LendBook(Borrow borrow)
        {
            _context.Borrows.Add(borrow);
            return _context.SaveChanges() > 0;
        }

        public bool ReturnBook(int copyId)
        {
            return _context.Returning(copyId) > 0;
        }
    }
}