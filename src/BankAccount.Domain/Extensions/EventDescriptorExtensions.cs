using Bank.Domain.Events;
using Newtonsoft.Json;

namespace Bank.Domain.Extensions
{
    public static class EventDescriptorExtensions
    {
        private static readonly JsonSerializerSettings SerializerSettings = new() { TypeNameHandling = TypeNameHandling.All };

        public static IEvent DeserializeEvent(this EventDescriptor data)
        {
            return (IEvent) JsonConvert.DeserializeObject(data.EventPayload, SerializerSettings);
        }
    }
}