using System.ComponentModel;

namespace InternetBankingApp.Enums
{
    public enum TransactionType
    {
        [Description("Credit (Deposit Money)")]
        Deposit,

        [Description("Debit (Withdrawal Money)")]
        Withdrawal,

        [Description("Debit (Transfer money between accounts")]
        Transfer,

        [Description("Debit (Service Charge)")]
        ServiceCharge
    }
}
