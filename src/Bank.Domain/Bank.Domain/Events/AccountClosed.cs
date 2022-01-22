using System;

namespace Bank.Domain.Events
{
    public class AccountClosed : Event
    {
        public decimal Balance { get; set; }
        public AccountClosed(Guid aggregateId, decimal balance) : base(aggregateId)
        {
            Balance = balance;
        }
    }
}