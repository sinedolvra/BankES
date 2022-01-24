using System;

namespace Bank.Domain.Events
{
    public class EventInfo
    {
        public Guid AggregateId { get; set; }
        public Guid EventId { get; set; }
        public DateTime EventCommittedTimestamp { get; set; }
        public Guid CorrelationId { get; set; }
        public Guid SagaProcessKey { get; set; }

        public EventInfo(Guid aggregateId, Guid correlationId, Guid sagaProcessKey)
        {
            AggregateId = aggregateId;
            CorrelationId = correlationId;
            SagaProcessKey = sagaProcessKey;
            EventId = Guid.NewGuid();
            EventCommittedTimestamp = DateTime.UtcNow;
        }
        
        public EventInfo(Guid aggregateId, Guid sagaProcessKey)
        {
            AggregateId = aggregateId;
            CorrelationId = Guid.NewGuid();
            SagaProcessKey = sagaProcessKey;
            EventId = Guid.NewGuid();
            EventCommittedTimestamp = DateTime.UtcNow;
        }
    }
}