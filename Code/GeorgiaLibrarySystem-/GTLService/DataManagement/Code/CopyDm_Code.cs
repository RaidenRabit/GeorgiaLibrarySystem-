using System.Data;
using Core;
using GTLService.DataAccess.Code;
using GTLService.DataManagement.IDataManagement;

namespace GTLService.DataManagement.Code
{
    public class CopyDm_Code : ICopyDm
    {
        private CopyDa_Code _copyDa;
        private LoaningDa_Code _loaningDa;
        private readonly LibrarianDa_Code _librarianDa;
        private readonly Context _context;

        public CopyDm_Code(CopyDa_Code copyDa, LoaningDa_Code loaningDa, LibrarianDa_Code librarianDa, Context context)
        {
            _copyDa = copyDa;
            _loaningDa = loaningDa;
            _librarianDa = librarianDa;
            _context = context;
        }

        public int GetAvailableCopyId(string isbn)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction(IsolationLevel.ReadUncommitted))
            {
                try
                {
                    var copies = _copyDa.GetAvailableCopyId(isbn, _context);
                    int i = 0;
                    if (copies.Count > 0)
                    {
                        while (!false)
                        {
                            if (_loaningDa.GetLoan(copies[i].CopyID, _context) == null)
                            {
                                dbContextTransaction.Commit();
                                return copies[i].CopyID;
                            }
                            else
                            {
                                i++;
                            }
                        }
                    }
                    dbContextTransaction.Rollback();
                    return 0;
                }
                catch
                {
                    dbContextTransaction.Rollback();
                    return 0;
                }
            }
        }

        public int GetTotalNrCopies(string isbn)
        {
            try
            {
                return _copyDa.ReadCopies(isbn, "0", _context).Count;
            }
            catch
            {
                return -1;
            }
        }

        public int GetOutOnLoan(string isbn)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                try
                {
                    var copies = _copyDa.ReadCopies(isbn, "0", _context);
                    int count = 0;
                    foreach (var copy in copies)
                    {
                        if (_loaningDa.GetLoan(copy.CopyID, _context) != null)
                        {
                            count++;
                        }
                    }

                    dbContextTransaction.Commit();
                    return count;
                }
                catch
                {
                    dbContextTransaction.Rollback();
                    return 0;
                }
            }
        }

        public bool DeleteCopy(int ssn, int copyId)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                try
                {
                    Copy copy = _copyDa.GetCopy(copyId, _context);
                    if (_librarianDa.CheckLibrarianSsn(ssn, _context) && copy != null)
                    {
                        var result = _copyDa.DeleteCopy(copy, _context);
                        dbContextTransaction.Commit();
                        return result;
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
    }
}