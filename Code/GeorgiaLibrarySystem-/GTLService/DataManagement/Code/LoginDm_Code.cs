using System.Data;
using Core;
using GTLService.DataAccess.Code;
using GTLService.DataManagement.IDataManagement;

namespace GTLService.DataManagement.Code
{
    public class LoginDm_Code : ILoginDm
    {
        private readonly LoginDa_Code _loginDa;
        private readonly Context _context;

        public LoginDm_Code(LoginDa_Code loginDa, Context context)
        {
            _loginDa = loginDa;
            _context = context;
        }

        public bool Login(int ssn, string password)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction(IsolationLevel.ReadUncommitted))
            {
                try
                {
                    if (ssn.ToString().Length == 9 && password.Length <= 16 && password.Length > 0)
                    {
                        dbContextTransaction.Commit();
                        return _loginDa.Login(ssn, password, _context);
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
