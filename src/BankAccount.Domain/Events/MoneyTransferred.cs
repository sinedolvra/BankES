using System;
using Bank.Domain.Entities;

namespace Bank.Domain.Events
{
    public class MoneyTransferred : Event
    {
        public string BankAccountDstId { get; set; }
        public decimal TransferredAmount { get; set; }

        public MoneyTransferred(Guid aggregateId, Guid sagaProcessKey, int aggregateVersion, decimal transferredAmount, string bankAccountDstId)
            : base(new EventInfo(aggregateId, sagaProcessKey), aggregateVersion)
        {
            BankAccountDstId = bankAccountDstId;
            TransferredAmount = transferredAmount;
        }
    }
}