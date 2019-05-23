using System;
using Core;
using GTLService.DataAccess.Code;
using GTLService.DataManagement.IDataManagement;

namespace GTLService.DataManagement.Code
{
    public class LoaningDm_Code: ILoaningDm
    {
        private readonly LoaningDa_Code _loaningDa;
        private readonly MemberDa_Code _memberDa;
        
        public LoaningDm_Code(LoaningDa_Code loaningDa, MemberDa_Code memberDa)
        {
            _loaningDa = loaningDa;
            _memberDa = memberDa;
        }

        public bool LoanBook(int ssn, int copyId)
        {
            try
            {
                if (_loaningDa.MemberLoanBooks(ssn) < _memberDa.GetMember(ssn).MemberType.NrOfBooks //member is allowed to Loan more
                        && _loaningDa.GetLoan(copyId) == null)//not Loaned at the time
                {
                    return _loaningDa.LoanBook(new Loan {SSN = ssn, CopyID = copyId, FromDate = DateTime.Now});
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
                Loan loan = _loaningDa.GetLoan(copyId);
                if (loan != null)
                {
                    loan.ToDate = DateTime.Now;
                    return _loaningDa.SaveLoanChanges();
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public bool NoticeFilling()
        {
            foreach (var loan in _loaningDa.GetAllActiveLoans())
            {
                Member member = _memberDa.GetMember(loan.SSN);
                
                if (DateTime.Now >= loan.FromDate.AddDays(member.MemberType.LendingLenght + member.MemberType.GracePeriod) && loan.noticeSent == null)
                {
                    loan.noticeSent = false;
                }
            }
            return _loaningDa.SaveLoanChanges();
        }
    }
}