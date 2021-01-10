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
        }

        public List<Login> GetLogin(int loginID)
        {
            if (_logins.Any())
            {
                return _logins;
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
            return _logins;
        }

        public async Task InsertLoginAsync(Login login)
        {
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
