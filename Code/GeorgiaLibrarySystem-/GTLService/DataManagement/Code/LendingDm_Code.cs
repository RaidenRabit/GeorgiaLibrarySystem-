using GTLService.DataManagement.IDataManagement;

namespace GTLService.DataManagement.Code
{
    public class LendingDm_Code: ILendingDm
    {
        //private readonly ILoginDa _loginDa;

        //public LoginDm_Code(ILoginDa loginDa)
        //{
        //    this._loginDa = loginDa;
        //}

        public bool LendBook(int ssn, int copyId)
        {
            //todo implement
            //Cant lend if more than 5 books lent (depends on users)
            //book not lent already

            //sending notice after some time
            
            throw new System.NotImplementedException();
        }

        public bool ReturnBook(int ssn, int copyId)
        {
            //todo implement
            throw new System.NotImplementedException();
        }
    }
}