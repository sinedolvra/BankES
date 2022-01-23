using System;
using System.Reflection;
using Bank.Domain.Events;
using Newtonsoft.Json;

namespace Bank.Domain.Extensions
{
    public static class EventDescriptorExtensions
    {
        private static readonly JsonSerializerSettings SerializerSettings = new() { TypeNameHandling = TypeNameHandling.All };

        public static IEvent DeserializeEvent(this EventDescriptor data)
        {
            var payload = (EventDescriptor)JsonConvert.DeserializeObject(data.EventPayload, typeof(EventDescriptor), SerializerSettings);
            return (IEvent)JsonConvert.DeserializeObject(payload.EventPayload, Type.GetType(payload.EventType, AssemblyResolver, null), SerializerSettings);
        }

        private static Assembly AssemblyResolver(AssemblyName assemblyName)
        {
            assemblyName.Version = null;
            return System.Reflection.Assembly.Load(assemblyName);
        }
    }
}