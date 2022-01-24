using Newtonsoft.Json;

namespace Bank.Domain.Events
{
    public class Event : IEvent
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
}