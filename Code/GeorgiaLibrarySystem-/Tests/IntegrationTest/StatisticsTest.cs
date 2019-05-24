using System;
using Core;
using GTLService.Controller;
using GTLService.DataAccess.Database;
using GTLService.DataManagement.Database;
using NUnit.Framework;

namespace Tests.IntegrationTest
{
    public class StatisticsTest
    {
        private IStatisticsService _statisticsService;


        [Test]
        //Database approach
        [TestCase("Database", 8, 5)]
        public void TopTenBooks(string approach, int loanedCopies, int totalCount)
        {
            //Arrange
            Setup(approach);

            //Act
            var result = _statisticsService.TopTenBooks();

            //Assert
            Assert.AreEqual(result.Count, totalCount);
            Assert.AreEqual(result[0].loaned_count, loanedCopies, "in reality it was: " + result[0].loaned_count);
        }

        [Test]
        //Database approach
        [TestCase("Database", 3)]
        public void AverageLoaningTime(string approach, double average)
        {
            
            //Arrange
            Setup(approach);
            

            //Act
            double result = _statisticsService.AverageLoaningTime();

            //Assert
            Assert.AreEqual(result, average);
        }

        [Test]
        //Database approach
        [TestCase("Database", "Pikalo")]
        public void MostLoaningLibraries(string approach, string library)
        {
            
            //Arrange
            Setup(approach);

            //Act
            string result = _statisticsService.MostLoaningLibraries();

            //Assert
            Assert.AreEqual(result,library);
        }

        private void Setup(string approach)
        {
            DatabaseTesting.ResetDatabase();
            Context context = new Context();
            switch (approach)
            {
                
                case "Database":
                    StatisticsDa_Database statisticsDa_Db = new StatisticsDa_Database(context);
                    StatisticsDm_Database statisticsDm_Db = new StatisticsDm_Database(statisticsDa_Db);
                    _statisticsService = new StatisticsService(statisticsDm_Db);
                    break;
                default:
                    new NotImplementedException();
                    break;
            }
        }

    }
}
