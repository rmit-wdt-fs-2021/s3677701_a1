using Authentication.Interfaces;
using Authentication.Model;
using Authentication.Utilities;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authentication.Manager
{
    public class LoginManager : ILoginManager
    {
        private readonly string _connectionString;

        public LoginManager(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Login> GetLogin(int loginID)
        {
            using var connection = new SqlConnection(_connectionString);
            var command = connection.CreateCommand();
            command.CommandText = "select * from Login where LoginID = @loginID";
            command.Parameters.AddWithValue("loginId", loginID);

            return command.GetDataTable().Select().Select(x => new Login
            {
                LoginID = (string)x["LoginID"],
                CustomerID = (int)x["CustomerID"],
                PasswordHash = (string)x["PasswordHash"]
            }).ToList();
        }

        public Login GetLogin(string loginID)
        {
            using var connection = _connectionString.CreateConnection();
            var command = connection.CreateCommand();
            command.CommandText = "select * from Login";

            return command.GetDataTable().Select().Select(x => new Login
            {
                LoginID = (string)x["LoginID"],
                CustomerID = (int)x["CustomerID"],
                PasswordHash = (string)x["PasswordHash"]
            }).ToList().FirstOrDefault(x => x.LoginID == loginID);
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
