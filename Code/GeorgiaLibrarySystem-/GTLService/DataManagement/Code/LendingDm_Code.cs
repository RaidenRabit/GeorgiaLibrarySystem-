using System;
using Core;
using GTLService.DataAccess.Code;
using GTLService.DataManagement.IDataManagement;

namespace GTLService.DataManagement.Code
{
    public class LendingDm_Code: ILendingDm
    {
        private readonly LendingDa_Code _lendingDa;
        private readonly MemberDa_Code _memberDa;
        
        public LendingDm_Code(LendingDa_Code lendingDa, MemberDa_Code memberDa)
        {
            _lendingDa = lendingDa;
            _memberDa = memberDa;
        }

        public bool LendBook(int ssn, int copyId)
        {
            try
            {
                if (_lendingDa.MemberBorrowedBooks(ssn) < _memberDa.GetMember(ssn).MemberType.NrOfBooks //member is allowed to borrow more
                        && _lendingDa.GetBorrow(copyId) == null)//not borrowed at the time
                {
                    return _lendingDa.LendBook(new Borrow {SSN = ssn, CopyID = copyId, FromDate = DateTime.Now});
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public bool ReturnBook(int copyId)
        {
            try
            {
                Borrow borrow = _lendingDa.GetBorrow(copyId);
                if (borrow != null)
                {
                    borrow.ToDate = DateTime.Now;
                    return _lendingDa.SaveBorrowChanges();
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
    }
}