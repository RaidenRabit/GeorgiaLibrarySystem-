using Core;
using GTLService.DataAccess.IDataAccess;
using GTLService.DataManagement.IDataManagement;

namespace GTLService.DataManagement.Code
{
    public class LendingDm_Code: ILendingDm
    {
        private readonly ILendingDa _lendingDa;
        private readonly IMemberDa _memberDa;

        //not done
        //member not added to windsor
        public LendingDm_Code(ILendingDa lendingDa, IMemberDa memberDa)
        {
            _lendingDa = lendingDa;
            _memberDa = memberDa;
        }

        public bool LendBook(int ssn, int copyId)
        {
            //todo implement
            //user limit exceeded
            //book not available
            try
            {
                if (_lendingDa.MemberBorrowedBooks(ssn) < _memberDa.GetMember(ssn).MemberType.NrOfBooks //member is allowed to borrow more
                        && _lendingDa.GetBorrow(copyId) == null)//not borrowed at the time
                {
                    return _lendingDa.LendBook(ssn, copyId);
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public bool ReturnBook(int ssn, int copyId)
        {
            try
            {
                //todo information displayed and disable triggers?
                return _lendingDa.ReturnBook(_lendingDa.GetBorrow(copyId));
            }
            catch
            {
                return false;
            }
        }
    }
}