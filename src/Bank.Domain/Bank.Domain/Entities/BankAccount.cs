using Bank.Domain.Enumerations;
using Bank.Domain.ValueObjects;

namespace Bank.Domain.Entities
{
    public partial class BankAccount : AggregateRoot
    {
        public string AccountNumber { get; set; }
        public string AccountDigit { get; set; }
        public AccountType AccountType { get; set; }
        public AccountHolder AccountHolder { get; set; }
        public decimal CurrentBalance { get; set; }
        public BankAccountState State { get; set; }
    }
}