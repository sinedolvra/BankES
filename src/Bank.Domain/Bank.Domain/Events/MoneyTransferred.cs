using System;
using Bank.Domain.Entities;

namespace Bank.Domain.Events
{
    public class MoneyTransferred : Event
    {
        public string BankAccountDstId { get; set; }
        public decimal TransferredAmount { get; set; }

        public MoneyTransferred(Guid aggregateId, decimal transferredAmount, string bankAccountDstId) : base(aggregateId)
        {
            BankAccountDstId = bankAccountDstId;
            TransferredAmount = transferredAmount;
        }
    }
}