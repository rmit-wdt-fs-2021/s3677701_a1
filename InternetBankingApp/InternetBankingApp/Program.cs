using InternetBankingApp.Managers;
using InternetBankingApp.Services;
using Microsoft.Extensions.Configuration;
using System;

namespace InternetBankingApp
{
    class Program
    {
        private static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var connectionString = configuration["ConnectionString"];
            var customerService = new CustomerService(connectionString);
            customerService.InsertCustomersAsync(connectionString).Wait();
            var loginService = new LoginService(connectionString);
            loginService.InsertLoginsAsync().Wait();
            //var menu = new Menu(loginService, customerService);
        }
    }
}
