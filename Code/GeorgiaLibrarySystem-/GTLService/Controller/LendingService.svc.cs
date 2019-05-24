using System.ServiceModel;
using GTLService.Controller.IController;
using GTLService.DataManagement.IDataManagement;

namespace GTLService.Controller
{
    [ServiceBehavior(IncludeExceptionDetailInFaults = true, InstanceContextMode = InstanceContextMode.PerCall)] 
    public class LoaningService : ILoaningService
    {
        private readonly ILoaningDm _loaningDm;
        public LoaningService(ILoaningDm loaningDm)
        {
            _loaningDm = loaningDm;
        }

        public bool LoanBook(int ssn, int copyId)
        {
            return _loaningDm.LoanBook(ssn, copyId);
        }

        public bool ReturnBook(int copyId)
        {
            return _loaningDm.ReturnBook(copyId);
        }
    }
}
