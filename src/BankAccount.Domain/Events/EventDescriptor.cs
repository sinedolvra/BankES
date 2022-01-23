using System;

namespace Bank.Domain.Events
{
    public record EventDescriptor
    {
        public Guid AggregateId { get; set; }
        public Guid EventId { get; set; }
        public DateTime EventCommittedTimestamp { get; set; }
        public string EventPayload { get; set; }
        public string EventType { get; set; }
        public Guid CorrelationId { get; set; }
        public Guid SagaProcessKey { get; set; }
        public int AggregateVersion { get; set; }

        public EventDescriptor(Guid aggregateId, Guid eventId, DateTime eventCommittedTimestamp, string eventPayload,
            string eventType, Guid correlationId, Guid sagaProcessKey)
        {
            AggregateId = aggregateId;
            EventId = eventId;
            EventCommittedTimestamp = eventCommittedTimestamp;
            EventPayload = eventPayload;
            EventType = eventType;
            CorrelationId = correlationId;
            SagaProcessKey = sagaProcessKey;
        }
    }
}