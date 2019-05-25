using System.Collections.Generic;
using System.Linq;
using Core;

namespace GTLService.DataAccess.Database
{
    public class StatisticsDa_Database
    {
        private readonly Context _context;
        public StatisticsDa_Database(Context context)
        {
            _context = context;
        }

        public List<topLoanedBook> TopTenBooks()
        {
            return _context.topLoanedBooks.ToList();
        }

        public double AverageLoaningTime()
        {
            return (double)_context.AverageLoanTime().FirstOrDefault();
        }

        public string MostLoaningLibraries()
        {
            return _context.TopLoaningLibrary().OrderByDescending(x => x.loaned_count).FirstOrDefault().Location;
        }
    }
}