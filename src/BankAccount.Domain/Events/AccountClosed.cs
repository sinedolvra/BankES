using System;

namespace Bank.Domain.Events
{
    public class AccountClosed : Event
    {
        public decimal Balance { get; set; }
        public AccountClosed(Guid aggregateId, Guid sagaProcessKey, int aggregateVersion, decimal balance) 
            : base(new EventInfo(aggregateId, sagaProcessKey), aggregateVersion)
        {
            Balance = balance;
        }
    }
}