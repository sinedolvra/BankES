using System;

namespace Bank.Domain.Events
{
    public class DepositedAmount : Event
    {
        public decimal Amount { get; set; }
        public DepositedAmount(Guid aggregateId, decimal amount) : base(aggregateId)
        {
            Amount = amount;
        }
    }
}