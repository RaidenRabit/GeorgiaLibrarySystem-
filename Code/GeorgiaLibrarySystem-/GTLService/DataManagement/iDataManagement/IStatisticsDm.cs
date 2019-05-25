
using System.Collections.Generic;
using Core;

namespace GTLService.DataManagement.IDataManagement
{
    public interface IStatisticsDm
    {
        
        List<topLoanedBook> TopTenBooks();
        
        double AverageLoaningTime();
        
        string MostLoaningLibraries();
    }
}
