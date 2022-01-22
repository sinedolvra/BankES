using System;
using Bank.Domain.Enumerations;
using Bank.Domain.Events;
using Bank.Domain.ValueObjects;

namespace Bank.Domain.Entities
{
    public partial class BankAccount
    {
        public void Open(string numberCode, string ispb, string branchNumber, string branchDigit, string accountNumber, string accountDigit, AccountType accountType, AccountHolder accountHolder)
        {
            InitializeId();
            var @event = new AccountOpened(Id, accountNumber, accountDigit, accountType, accountHolder);
            Add(@event);
        }

        public void TransferMoney(decimal amount, string bankAccountDstId)
        {
            ValidateAmount(amount);
            var @event = new MoneyTransferred(Id, amount, bankAccountDstId);
            Add(@event);
        }

        public void Withdraw(decimal amount)
        {
            ValidateAmount(amount);
            var @event = new Withdrawal(Id, amount);
            Add(@event);
        }

        public void Close()
        {
            ValidateState("Invalid state to close account");
            var balance = CurrentBalance > 0 ? 0M : CurrentBalance;
            var @event = new AccountClosed(Id, balance);
            Add(@event);
        }

        public void Deposit(decimal amount)
        {
            ValidateState("Invalid state to deposit");
            var @event = new DepositedAmount(Id, amount);
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
        }
        
        public void Apply(AccountClosed @event)
        {
            CurrentBalance = @event.Balance;
            State = BankAccountState.Closed;
        }

        public void Apply(MoneyTransferred @event) => CurrentBalance -= @event.TransferredAmount;
        
        public void Apply(Withdrawal @event) => CurrentBalance -= @event.Amount;
    }
}