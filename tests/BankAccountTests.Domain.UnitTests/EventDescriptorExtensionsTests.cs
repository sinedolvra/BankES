using System;
using AutoFixture;
using Bank.Domain.Events;
using Bank.Domain.Extensions;
using FluentAssertions;
using Xunit;

namespace BankAccountTests.Domain.UnitTests
{
    public class EventDescriptorExtensionsTests
    {
        private readonly Fixture _fixture = new();
        
        [Fact]
        public void DeserializeEvent_GivenAValidEventDescriptor_ShouldReturnAValidIEvent()
        {
            var accountOpenedEvent = _fixture.Create<AccountOpened>();
            var eventDescriptor = new EventDescriptor(accountOpenedEvent.EventInfo.AggregateId, accountOpenedEvent.EventInfo.EventId, accountOpenedEvent.EventInfo.EventCommittedTimestamp, accountOpenedEvent.Payload(), accountOpenedEvent.EventType, Guid.NewGuid(), Guid.NewGuid());

            var result = eventDescriptor.DeserializeEvent();
            result.Should().BeOfType<AccountOpened>();
        }
    }
}