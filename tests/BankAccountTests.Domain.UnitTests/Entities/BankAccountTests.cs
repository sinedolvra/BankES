using System;
using AutoFixture;
using Bank.Domain.Infrastructure;
using Bank.Domain.Entities;
using Bank.Domain.Events;
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
        public void Open_GivenACall_ShouldAddAccountOpenedAndSaveIt()
        {
            
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