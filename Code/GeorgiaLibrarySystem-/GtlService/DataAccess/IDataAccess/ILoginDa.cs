namespace GTLService.DataAccess.IDataAccess
{
    public interface ILoginDa
    {
        bool Login(int ssn, string password);
    }
}
