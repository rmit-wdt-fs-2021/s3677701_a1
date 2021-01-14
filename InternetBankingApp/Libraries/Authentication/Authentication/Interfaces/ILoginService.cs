namespace Authentication.Interfaces
{
    public interface ILoginService
    {
        public bool AuthenticateUser(string loginId, string password);
        public int GetCustomerIDFromLogin(string loginID);
    }
}
