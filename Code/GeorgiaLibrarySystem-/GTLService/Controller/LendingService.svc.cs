using System.ServiceModel;
using GTLService.Controller.IController;
using GTLService.DataManagement.IDataManagement;

namespace GTLService.Controller
{
    [ServiceBehavior(IncludeExceptionDetailInFaults = true, InstanceContextMode = InstanceContextMode.PerCall)] 
    public class LendingService : ILendingService
    {
        private readonly ILendingDm _lendingDm;
        public LendingService(ILendingDm lendingDm)
        {
            _lendingDm = lendingDm;
        }

        public bool LendBook(int ssn, int copyId)
        {
            return _lendingDm.LendBook(ssn, copyId);
        }

        public bool ReturnBook(int ssn, int copyId)
        {
            return _lendingDm.ReturnBook(ssn, copyId);
        }
    }
}
