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
        public List<Login> Logins { get; }

        public LoginManagerProxy(string connectionString)
        {
            _connectionString = connectionString;

            using var connection = _connectionString.CreateConnection();
            var command = connection.CreateCommand();
            command.CommandText = "select * from Login";

            Logins = command.GetDataTable().Select().Select(x => new Login
            {
                LoginID = (string)x["LoginID"],
                CustomerID = (int)x["CustomerID"],
                PasswordHash = (string)x["PasswordHash"]
            }).ToList();
        }

        public Login GetLogin(string loginID)
        {
            return Logins.FirstOrDefault(x => x.LoginID == loginID);
        }

        public async Task InsertLoginAsync(Login login)
        {
            if (Logins.Any())
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
