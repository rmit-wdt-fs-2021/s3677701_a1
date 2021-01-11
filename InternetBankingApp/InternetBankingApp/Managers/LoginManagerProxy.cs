using InternetBankingApp.Interfaces;
using InternetBankingApp.Models;
using InternetBankingApp.Utilities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternetBankingApp.Managers
{
    public class LoginManagerProxy : ILoginManager
    {
        private readonly string _connectionString;
        private List<Login> _logins;

        public LoginManagerProxy(string connectionString)
        {
            _connectionString = connectionString;
            _logins = new List<Login>();
        }

        public Login GetLogin(string loginID)
        {
            if (_logins.Any())
            {
                return _logins.FirstOrDefault(x => x.LoginID == loginID);
            }

            using var connection = _connectionString.CreateConnection();
            var command = connection.CreateCommand();
            command.CommandText = "select * from Login where LoginID = @loginID";
            command.Parameters.AddWithValue("loginId", loginID);

            _logins = command.GetDataTable().Select().Select(x => new Login
            {
                LoginID = (string)x["LoginID"],
                CustomerID = (int)x["CustomerID"],
                PasswordHash = (string)x["PasswordHash"]
            }).ToList();
            return _logins.FirstOrDefault(x => x.LoginID == loginID);
        }

        public async Task InsertLoginAsync(Login login)
        {
            if (_logins.Any())
            {
                // If logins are cached do not talk to DB.
                return;
            }

            using var connection = _connectionString.CreateConnection();
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = "insert into Login (LoginID, CustomerID, PasswordHash) values (@loginID, @customerID, @passwordHash)";
            command.Parameters.AddWithValue("loginID", login.LoginID);
            command.Parameters.AddWithValue("customerID", login.CustomerID);
            command.Parameters.AddWithValue("passwordHash", login.PasswordHash);

            await command.ExecuteNonQueryAsync();
        }
    }
}
