using System;
using Core;
using GTLService.DataAccess.IDataAccess;
using GTLService.DataManagement.IDataManagement;

namespace GTLService.DataManagement.Code
{
    public class LendingDm_Code: ILendingDm
    {
        private readonly ILendingDa _lendingDa;
        private readonly IMemberDa _memberDa;
        
        public LendingDm_Code(ILendingDa lendingDa, IMemberDa memberDa)
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
                    return _lendingDa.LendBook(ssn, copyId);
                }
                return false;
            }
            catch(Exception e)
            {
                return false;
            }
        }

        public bool ReturnBook(int ssn, int copyId)
        {
            try
            {
                //todo information displayed and disable triggers?
                Borrow borrow = _lendingDa.GetBorrow(copyId);
                if (borrow != null)
                {
                    return _lendingDa.ReturnBook(_lendingDa.GetBorrow(copyId));
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