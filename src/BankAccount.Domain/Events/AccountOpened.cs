using System;
using Bank.Domain.Enumerations;
using Bank.Domain.ValueObjects;

namespace Bank.Domain.Events
{
    public class AccountOpened : Event
    {
        public string AccountNumber { get; set; }
        public string AccountDigit { get; set; }
        public AccountType AccountType { get; set; }
        public AccountHolder AccountHolder { get; set; }

        public AccountOpened(Guid aggregateId, Guid sagaProcessKey, int aggregateVersion, string accountNumber, string accountDigit, AccountType accountType, 
            AccountHolder accountHolder) : base(new EventInfo(aggregateId, sagaProcessKey), aggregateVersion)
        {
            AccountNumber = accountNumber;
            AccountDigit = accountDigit;
            AccountType = accountType;
            AccountHolder = accountHolder;
        }
    }
}