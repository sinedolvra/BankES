using System;
using System.Threading.Tasks;
using AutoFixture;
using Bank.Domain.Infrastructure;
using Bank.Domain.Entities;
using Bank.Domain.Enumerations;
using Bank.Domain.Events;
using Bank.Domain.ValueObjects;
using FluentAssertions;
using Moq;
using Xunit;

namespace BankAccountTests.Domain.UnitTests.Entities
{
    public class BankAccountTests
    {
        private readonly Mock<IAggregateRepository<BankAccount>> _aggregateRepository = new();
        private BankAccount _bankAccount;
        private readonly Fixture _fixture = new();
        private const string GenericString = "xpto";
        private Guid _genericGuid = Guid.NewGuid();

        public BankAccountTests()
        {
            _bankAccount = new BankAccount(_aggregateRepository.Object);
        }

        [Fact]
        public void AddEvent_GivenAValidEvent_ShouldAddEventAndIncrementVersion()
        {
            var oldVersion = _bankAccount.Version;
            var @event = GetValidEvent();
            
            _bankAccount.AddEvent(@event);

            _bankAccount.Version.Should().Be(oldVersion + 1);
            _bankAccount.UncommittedEvents().Should().Contain(@event);
        }

        [Fact]
        public void AddEvent_GivenAnInvalidEvent_ShouldThrowAggregateException()
        {
            var @event = _fixture.Build<Event>().Create();
            Action result = () => _bankAccount.AddEvent(@event);

            result.Should().Throw<AggregateException>();
        }

        [Fact]
        public void CanAddEvent_GivenAValidEvent_ShouldReturnTrue()
        {
            var @event = GetValidEvent();
            var result = _bankAccount.CanAddEvent(@event);
            result.Should().BeTrue();
        }
        
        [Fact]
        public void CanAddEvent_GivenAnInValidEvent_ShouldReturFalse()
        {
            var @event = _fixture.Build<Event>().Create();
            var result = _bankAccount.CanAddEvent(@event);
            result.Should().BeFalse();
        }

        [Fact]
        public void InitializeId_GivenACall_ShouldInitId()
        {
            _bankAccount.InitializeId();
            _bankAccount.Id.Should().NotBeEmpty();
        }

        [Fact]
        public void UncommittedEvents_GivenACall_ShouldContainUncommittedEvents()
        {
            var @event = GetValidEvent();
            _bankAccount.AddEvent(@event);

            _bankAccount.UncommittedEvents().Should().Contain(@event);
        }

        [Fact]
        public void Commit_GivenACall_ShouldClearUncommittedEventsList()
        {
            var @event = GetValidEvent();
            _bankAccount.AddEvent(@event);

            _bankAccount.Commit();
            _bankAccount.UncommittedEvents().Should().BeEmpty();
        }

        [Fact]
        public async Task Open_GivenACall_ShouldAddAccountOpenedAndSaveIt()
        {
            var accountType = _fixture.Create<AccountType>();
            await _bankAccount.Open(GenericString, GenericString, accountType, new AccountHolder(), _genericGuid);
            _bankAccount.UncommittedEvents().Should().ContainSingle(x => x.EventType == nameof(AccountOpened));
            _aggregateRepository.Verify(x => x.Save(It.IsAny<BankAccount>()), Times.Once);
        }

        [Fact]
        public void TransferMoney_GivenACallAndAmountGreaterThanCurrentBalance_ShouldThrowInvalidOperationException()
        {
            var amount = _bankAccount.CurrentBalance + 10;
            Func<Task> result = async () => await _bankAccount.TransferMoney(amount, GenericString, _genericGuid);
            result.Should().ThrowAsync<InvalidOperationException>();
        }
        
        [Fact]
        public async Task TransferMoney_GivenACall_ShouldAddEventAndSaveIt()
        {
            var amount = _bankAccount.CurrentBalance - 10;
            await _bankAccount.TransferMoney(amount, GenericString, _genericGuid);
            _bankAccount.UncommittedEvents().Should().ContainSingle(x => x.EventType == nameof(MoneyTransferred));
            _aggregateRepository.Verify(x => x.Save(It.IsAny<BankAccount>()), Times.Once);
        }
        
        [Fact]
        public void Withdraw_GivenACallAndAmountGreaterThanCurrentBalance_ShouldThrowInvalidOperationException()
        {
            var amount = _bankAccount.CurrentBalance + 10;
            Func<Task> result = async () => await _bankAccount.Withdraw(amount, _genericGuid);
            result.Should().ThrowAsync<InvalidOperationException>();
        }
        
        [Fact]
        public async Task Withdraw_GivenACall_ShouldAddEventAndSaveIt()
        {
            var amount = _bankAccount.CurrentBalance - 10;
            await _bankAccount.Withdraw(amount, _genericGuid);
            _bankAccount.UncommittedEvents().Should().ContainSingle(x => x.EventType == nameof(Withdrawal));
            _aggregateRepository.Verify(x => x.Save(It.IsAny<BankAccount>()), Times.Once);
        }

        [Theory]
        [InlineData(BankAccountState.Closed)]
        [InlineData(BankAccountState.NotSet)]
        public async Task Close_GivenAnInvalidState_ShouldThrowInvalidOperationException(BankAccountState state)
        {
            _bankAccount.State = state;
            Func<Task> result = async () => await _bankAccount.Close(_genericGuid);
            await result.Should().ThrowAsync<InvalidOperationException>();
        }
        
        [Fact]
        public async Task Close_GivenAValidState_ShouldAddEventAndSaveIt()
        {
            _bankAccount.State = BankAccountState.Opened;
            await _bankAccount.Close(_genericGuid);
            _bankAccount.UncommittedEvents().Should().ContainSingle(x => x.EventType == nameof(AccountClosed));
            _aggregateRepository.Verify(x => x.Save(It.IsAny<BankAccount>()), Times.Once);
        }

        [Theory]
        [InlineData(BankAccountState.Closed)]
        [InlineData(BankAccountState.NotSet)]
        public async Task Deposit_GivenAnInvalidState_ShouldThrowInvalidOperationException(BankAccountState state)
        {
            _bankAccount.State = state;
            Func<Task> result = async () => await _bankAccount.Deposit(_genericGuid, _fixture.Create<decimal>());
            await result.Should().ThrowAsync<InvalidOperationException>();
        }
        
        [Fact]
        public async Task Deposit_GivenAValidState_ShouldAddEventAndSaveIt()
        {
            _bankAccount.State = BankAccountState.Opened;
            await _bankAccount.Deposit(_genericGuid, _fixture.Create<decimal>());
            _bankAccount.UncommittedEvents().Should().ContainSingle(x => x.EventType == nameof(DepositedAmount));
            _aggregateRepository.Verify(x => x.Save(It.IsAny<BankAccount>()), Times.Once);
        }

        [Fact]
        public void ApplyAccountOpened_GivenAValidEvent_ShouldMatchValues()
        {
            var @event = _fixture.Create<AccountOpened>();
            _bankAccount.Apply(@event);
            _bankAccount.State.Should().Be(BankAccountState.Opened);
            _bankAccount.AccountNumber.Should().Be(@event.AccountNumber);
            _bankAccount.AccountDigit.Should().Be(@event.AccountDigit);
            _bankAccount.AccountType.Should().Be(@event.AccountType);
            _bankAccount.Version.Should().Be(@event.AggregateVersion);
        }
        
        [Fact]
        public void ApplyAccountClosed_GivenAValidEvent_ShouldMatchValues()
        {
            var @event = _fixture.Create<AccountClosed>();
            _bankAccount.Apply(@event);
            _bankAccount.State.Should().Be(BankAccountState.Closed);
            _bankAccount.Version.Should().Be(@event.AggregateVersion);
            _bankAccount.CurrentBalance.Should().Be(@event.Balance);
        }
        
        [Fact]
        public void ApplyMoneyTransferred_GivenAValidEvent_ShouldMatchValues()
        {
            var oldCurrentBalance = _bankAccount.CurrentBalance;
            var @event = _fixture.Create<MoneyTransferred>();
            _bankAccount.Apply(@event);
            
            _bankAccount.Version.Should().Be(@event.AggregateVersion);
            _bankAccount.CurrentBalance.Should().BeLessThan(oldCurrentBalance);
        }
        
        [Fact]
        public void ApplyWithdrawal_GivenAValidEvent_ShouldMatchValues()
        {
            var oldCurrentBalance = _bankAccount.CurrentBalance;
            var @event = _fixture.Create<Withdrawal>();
            _bankAccount.Apply(@event);
            
            _bankAccount.Version.Should().Be(@event.AggregateVersion);
            _bankAccount.CurrentBalance.Should().BeLessThan(oldCurrentBalance);
        }
        
        private IEvent GetValidEvent()
        {
            var eventInfo = _fixture.Build<EventInfo>()
                .With(x => x.AggregateId, _bankAccount.Id)
                .Create();
            return _fixture.Build<Event>()
                .With(x => x.AggregateVersion, _bankAccount.Version)
                .With(x => x.EventInfo, eventInfo)
                .Create();
        }
    }
}