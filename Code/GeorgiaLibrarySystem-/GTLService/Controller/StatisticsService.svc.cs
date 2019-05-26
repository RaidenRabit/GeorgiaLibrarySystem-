using System.Collections.Generic;
using Core;
using GTLService.Controller.IController;
using GTLService.DataManagement.IDataManagement;

namespace GTLService.Controller
{
    public class StatisticsService : IStatisticsService
    {
        private readonly IStatisticsDm _statisticsDm;
        public StatisticsService(IStatisticsDm statisticsDmDm)
        {
            _statisticsDm = statisticsDmDm;
        }

        public List<topLoanedBook> TopTenBooks()
        {
            return _statisticsDm.TopTenBooks();
        }

        public double AverageLoaningTime()
        {
            return _statisticsDm.AverageLoaningTime();
        }

        public string MostLoaningLibraries()
        {
            return _statisticsDm.MostLoaningLibraries();
        }
    }
}
