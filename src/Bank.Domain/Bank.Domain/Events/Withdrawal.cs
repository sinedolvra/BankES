using System;

namespace Bank.Domain.Events
{
    public class Withdrawal : Event
    {
        public decimal Amount { get; set; }
        public Withdrawal(Guid aggregateId, decimal amount) : base(aggregateId)
        {
            Amount = amount;
        }
    }
}