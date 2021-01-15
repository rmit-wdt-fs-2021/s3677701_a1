using Authentication.Interfaces;
using Authentication.Model;
using Authentication.Utilities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authentication.Manager
{
    public class LoginManagerProxy : ILoginManager
    {
        private readonly string _connectionString;
        private readonly LoginManager _loginManager;
        public List<Login> Logins { get; }

        public LoginManagerProxy(string connectionString)
        {
            _connectionString = connectionString;
            _loginManager = new LoginManager(connectionString);

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
            var login = Logins.FirstOrDefault(x => x.LoginID == loginID);
            if (login is null)
            {
                login = _loginManager.GetLogin(loginID);
            }
            return login;
        }

        public async Task InsertLoginAsync(Login login)
        {
            if (Logins.Any())
            {
                // If logins are cached do not talk to DB.
                return;
            }

            await _loginManager.InsertLoginAsync(login);
        }
    }
}

