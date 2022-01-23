using System;

namespace Bank.Domain.Events
{
    public class Withdrawal : Event
    {
        public decimal Amount { get; set; }
        public Withdrawal(Guid aggregateId, Guid sagaProcessKey, int aggregateVersion, decimal amount)
            : base(new EventInfo(aggregateId, sagaProcessKey), aggregateVersion)
        {
            Amount = amount;
        }
    }
}