﻿using System;
using System.Data;
using Core;
using GTLService.DataAccess.Code;
using GTLService.DataManagement.IDataManagement;

namespace GTLService.DataManagement.Code
{
    public class LoaningDm_Code: ILoaningDm
    {
        private readonly LoaningDa_Code _loaningDa;
        private readonly MemberDa_Code _memberDa;
        private readonly Context _context;
        
        public LoaningDm_Code(LoaningDa_Code loaningDa, MemberDa_Code memberDa, Context context)
        {
            _loaningDa = loaningDa;
            _memberDa = memberDa;
            _context = context;
        }

        public bool LoanBook(int ssn, int copyId)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction(IsolationLevel.Serializable))
            {
                try
                {
                    if (_loaningDa.MemberLoanBooks(ssn, _context) < _memberDa.GetMember(ssn, _context).MemberType.NrOfBooks //member is allowed to Loan more
                        && _loaningDa.GetLoan(copyId, _context) == null)//not Loaned at the time
                    {
                        _context.Database.ExecuteSqlCommand("ALTER TABLE Loan DISABLE TRIGGER Lending");//so trigger wouldn't be run
                        _context.SaveChanges();
                        var result = _loaningDa.LoanBook(new Loan {SSN = ssn, CopyID = copyId, FromDate = DateTime.Now}, _context);
                        _context.Database.ExecuteSqlCommand("ALTER TABLE Loan ENABLE TRIGGER Lending");
                        _context.SaveChanges();

                        if (result)
                        {
                            dbContextTransaction.Commit();
                            return true;
                        }
                    }
                    dbContextTransaction.Rollback();
                    return false;
                }
                catch
                {
                    dbContextTransaction.Rollback();
                    return false;
                }
            }
        }

        public bool ReturnBook(int copyId)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                try
                {
                    Loan loan = _loaningDa.GetLoan(copyId, _context);
                    loan.ToDate = DateTime.Now;
                    var result = _loaningDa.UpdateLoan(loan, _context);
                    dbContextTransaction.Commit();
                    return result;
                }
                catch
                {
                    dbContextTransaction.Rollback();
                    return false;
                }
            }
        }

        public bool NoticeFilling()
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction(IsolationLevel.ReadUncommitted))
            {
                try
                {
                    foreach (var loan in _loaningDa.GetAllActiveLoans(_context))
                    {
                        Member member = _memberDa.GetMember(loan.SSN, _context);

                        if (DateTime.Now >=
                            loan.FromDate.AddDays(member.MemberType.LendingLenght + member.MemberType.GracePeriod) &&
                            loan.noticeSent == null)
                        {
                            loan.noticeSent = false;
                            _loaningDa.UpdateLoan(loan, _context);
                        }
                    }

                    dbContextTransaction.Commit();
                    return true;
                }
                catch
                {
                    dbContextTransaction.Rollback();
                    return false;
                }
            }
        }
    }
}