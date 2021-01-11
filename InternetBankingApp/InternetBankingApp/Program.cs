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
            customerService.InsertCustomers(connectionString);
            var loginService = new LoginService(connectionString);
            loginService.InsertLogins();
            var menu = new Menu(loginService, customerService);
        }
    }
}
