namespace InternetBankingApp.Managers
{
    internal class AccountManager
    {
        private string _connectionString;

        public AccountManager(string connectionString)
        {
            _connectionString = connectionString;
        }
    }
}