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
            //new LoginService().InsertLoginsAsync(connectionString);
            new CustomerService().InsertCustomers(connectionString);
            //new Menu().Run();
        }
    }
}
