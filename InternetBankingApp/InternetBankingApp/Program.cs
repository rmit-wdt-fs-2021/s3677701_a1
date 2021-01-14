using Authentication;
using InternetBankingApp.Services;
using Microsoft.Extensions.Configuration;

namespace InternetBankingApp
{
    class Program
    {
        private static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var connectionString = configuration["ConnectionString"];
            var customerService = new CustomerService(connectionString);
            customerService.InsertCustomerDataAsync().Wait();
            var loginService = new LoginService(connectionString);
            loginService.InsertLoginsAsync().Wait();

            var accountService = new AccountService(connectionString);
            var transactionService = new TransactionService(connectionString);
            var menu = new Menu(loginService, customerService, accountService, transactionService);
        }
    }
}
