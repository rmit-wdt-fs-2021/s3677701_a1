using InternetBankingApp.Enums;

namespace InternetBankingApp.Models
{
    public class Transaction
    {
        public int TransactionID { get; set; }
        public TransactionType TransactionType { get; set; }
        public int AccountNumber { get; set; }
        public int DestinationAccountNumber { get; set; }
        public decimal Amount { get; set; }
        public object Comment { get; set; }
        public string TransactionTimeUtc { get; set; }
    }
}
