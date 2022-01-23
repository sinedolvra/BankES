using System;
using System.Threading.Tasks;
using Bank.Domain.Enumerations;
using Bank.Domain.Events;
using Bank.Domain.ValueObjects;

namespace Bank.Domain.Entities
{
    public partial class BankAccount
    {
        public async Task Open(string numberCode, string ispb, string branchNumber, string branchDigit, string accountNumber,
            string accountDigit, AccountType accountType, AccountHolder accountHolder, Guid sagaProcessKey)
        {
            InitializeId();
            var @event = new AccountOpened(Id, sagaProcessKey, Version, accountNumber, accountDigit, accountType, accountHolder);
            AddEvent(@event);
            await Save();
        }

        public async Task TransferMoney(decimal amount, string bankAccountDstId, Guid sagaProcessKey)
        {
            ValidateAmount(amount);
            var @event = new MoneyTransferred(Id, sagaProcessKey, Version, amount, bankAccountDstId);
            AddEvent(@event);
            await Save();
        }

        public async Task Withdraw(decimal amount, Guid sagaProcessKey)
        {
            ValidateAmount(amount);
            var @event = new Withdrawal(Id, sagaProcessKey, Version,  amount);
            AddEvent(@event);
            await Save();
        }

        public async Task Close(Guid sagaProcessKey)
        {
            ValidateState("Invalid state to close account");
            var balance = CurrentBalance > 0 ? 0M : CurrentBalance;
            var @event = new AccountClosed(Id, sagaProcessKey, Version,  balance);
            AddEvent(@event);
            await Save();
        }

        public async Task Deposit(Guid sagaProcessKey, decimal amount)
        {
            ValidateState("Invalid state to deposit");
            var @event = new DepositedAmount(Id, sagaProcessKey, Version,  amount);
            AddEvent(@event);
            await Save();
        }

        private void ValidateState(string msg)
        {
            if (State != BankAccountState.Opened)
                throw new InvalidOperationException(msg);
        }
        
        private void ValidateAmount(decimal amount)
        {
            if(CurrentBalance < amount)
                throw new InvalidOperationException("Not enough money available");
        }

        public void Apply(AccountOpened @event)
        {
            AccountNumber = @event.AccountNumber;
            AccountDigit = @event.AccountDigit;
            AccountType = @event.AccountType;
            AccountHolder = @event.AccountHolder;
            State = BankAccountState.Opened;
            Version = @event.AggregateVersion;
        }
        
        public void Apply(AccountClosed @event)
        {
            CurrentBalance = @event.Balance;
            State = BankAccountState.Closed;
            Version = @event.AggregateVersion;
        }

        public void Apply(MoneyTransferred @event)
        {
            CurrentBalance -= @event.TransferredAmount;
            Version = @event.AggregateVersion;
        }

        public void Apply(Withdrawal @event)
        {
            CurrentBalance -= @event.Amount;
            Version = @event.AggregateVersion;
        }
        
        private async Task Save() => await Repository.Save(this);
    }
}