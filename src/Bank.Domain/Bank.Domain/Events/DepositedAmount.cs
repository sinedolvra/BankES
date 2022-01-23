using System;

namespace Bank.Domain.Events
{
    public class DepositedAmount : Event
    {
        public decimal Amount { get; set; }
        public DepositedAmount(Guid aggregateId, Guid sagaProcessKey, int aggregateVersion, decimal amount) 
            : base(new EventInfo(aggregateId, sagaProcessKey), aggregateVersion)
        {
            Amount = amount;
        }
    }
}