using System;

namespace Bank.Domain.Events
{
    public abstract class Event : IEvent
    {
        public Guid AggregateId { get; set; }
        public Guid EventId { get; set; }
        public DateTime EventCommittedTimestamp { get; set; }
        public int AggregateVersion { get; set; }

        public Event(Guid aggregateId)
        {
            AggregateId = aggregateId;
            EventId = Guid.NewGuid();
            EventCommittedTimestamp = DateTime.UtcNow;
            //AggregateVersion = aggregateVersion;
        }
    }
}