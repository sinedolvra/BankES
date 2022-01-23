using System;
using Newtonsoft.Json;

namespace Bank.Domain.Events
{
    public abstract class Event : IEvent
    {
        public Guid AggregateId { get; set; }
        public Guid EventId { get; set; }
        public DateTime EventCommittedTimestamp { get; set; }
        public int AggregateVersion { get; set; }
        public string EventType { get; set; }
        public Guid CorrelationId { get; set; }
        public Guid SagaProcessKey { get; set; }

        public Event(Guid aggregateId, Guid correlationId, Guid sagaProcessKey, int aggregateVersion)
        {
            AggregateId = aggregateId;
            EventCommittedTimestamp = DateTime.UtcNow;
            AggregateVersion = aggregateVersion;
            EventType = GetType().Name;
            CorrelationId = correlationId;
            SagaProcessKey = sagaProcessKey;
        }

        public string Payload() => JsonConvert.SerializeObject(this, new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All
        });
    }
}