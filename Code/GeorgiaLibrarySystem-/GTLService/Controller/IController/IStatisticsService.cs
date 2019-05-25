using System.Collections.Generic;
using System.ServiceModel;
using Core;

namespace GTLService.Controller
{
    [ServiceContract]
    public interface IStatisticsService
    {
        [OperationContract]
        List<topLoanedBook> TopTenBooks();

        [OperationContract]
        double AverageLoaningTime();

        [OperationContract]
        string MostLoaningLibraries();

    }
}
