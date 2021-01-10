﻿using InternetBankingApp.Interfaces;
using InternetBankingApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using InternetBankingApp.Managers;

namespace InternetBankingApp.Services
{
    public class LoginService
    {
        public bool AuthenticateUser(string loginId)
        {
            // Call to db ?
            return false;
        }

        public async Task<IList<Login>> GetLoginsAsync()
        {
            using var client = new HttpClient();
            var loginResponse = await client.GetStringAsync("https://coreteaching01.csit.rmit.edu.au/~e87149/wdt/services/logins/");
            var logins = JsonConvert.DeserializeObject<List<Login>>(loginResponse);

            return logins;
        }

        public void InsertLogins(string connectionString)
        {
            var logins = GetLoginsAsync().Result;
            var loginManager = new LoginManager(connectionString);
            foreach (var login in logins)
            {
                loginManager.InsertLoginAsync(login);
            }
        }
    }
}
