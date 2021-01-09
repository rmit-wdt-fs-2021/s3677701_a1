using InternetBankingApp.Models;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using InternetBankingApp.Utilities;

namespace InternetBankingApp.Managers
{
    public class LoginManager
    {
        private readonly string _connectionString;

        public LoginManager(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Login> GetLogins(int loginID)
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

        public void InsertLogin(Login login)
        {
            using var connection = _connectionString.CreateConnection();
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = "insert into Login (LoginID, CustomerID, PasswordHash) values (@loginID, @customerID, @passwordHash)";
            command.Parameters.AddWithValue("loginID", login.LoginID);
            command.Parameters.AddWithValue("customerID", login.CustomerID);
            command.Parameters.AddWithValue("passwordHash", login.PasswordHash);

            command.ExecuteNonQuery();
        }
    }
}
