using System.Collections.Generic;
using Core;
using GTLService.DataAccess.Database;
using GTLService.DataManagement.IDataManagement;

namespace GTLService.DataManagement.Database
{
    public class StatisticsDm_Database : IStatisticsDm
    { 
        private readonly StatisticsDa_Database _statisticsDa;

        public StatisticsDm_Database(StatisticsDa_Database statisticsDa)
        {
            _statisticsDa = statisticsDa;
        }

        public List<topLoanedBook> TopTenBooks()
        {
            return _statisticsDa.TopTenBooks();
        }

        public double AverageLoaningTime()
        {
            return _statisticsDa.AverageLoaningTime();
        }

        public string MostLoaningLibraries()
        {
            return _statisticsDa.MostLoaningLibraries();
        }
    }
}