using System;

namespace Bank.Domain.Events
{
    public interface IEvent
    {
        Guid AggregateId { get; set; }
        Guid EventId { get; set; }
        DateTime EventCommittedTimestamp { get; set; }
        int AggregateVersion { get; set; }
        string EventType { get; set; }
        Guid CorrelationId { get; set; }
        Guid SagaProcessKey { get; set; }
        string Payload();
    }
}