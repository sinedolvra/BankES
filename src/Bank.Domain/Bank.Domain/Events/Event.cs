using System;
using Newtonsoft.Json;

namespace Bank.Domain.Events
{
    public abstract class Event : IEvent
    {
        public int AggregateVersion { get; set; }
        public string EventType { get; set; }
        public EventInfo EventInfo { get; set; }

        public Event(EventInfo eventInfo, int aggregateVersion)
        {
            AggregateVersion = aggregateVersion;
            EventType = GetType().Name;
            EventInfo = eventInfo;
        }

        public string Payload() => JsonConvert.SerializeObject(this, new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All
        });
    }

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