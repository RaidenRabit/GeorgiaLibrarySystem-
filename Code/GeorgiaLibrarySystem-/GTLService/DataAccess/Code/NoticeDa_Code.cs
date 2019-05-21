using System.Linq;
using Core;

namespace GTLService.DataAccess.Code
{
    public class NoticeDa_Code
    {
        private readonly Context _context;
        public NoticeDa_Code(Context context)
        {
            _context = context;
        }

        public virtual Notice GetNotice(Borrow borrow)
        {
            return _context.Notices.FirstOrDefault(x => x.SSN == borrow.SSN && x.CopyID == borrow.CopyID && x.FromDate == borrow.FromDate);
        }

        public virtual bool CreateNotice(Notice notice)
        {
            _context.Notices.Add(notice);
            return _context.SaveChanges() > 0;
        }
    }
}