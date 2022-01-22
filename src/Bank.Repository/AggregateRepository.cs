using System;
using System.Collections.Generic;
using System.Linq;
using Bank.Domain.Entities;
using Bank.Domain.Events;
using Bank.Domain.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Bank.Repository
{
    public class AggregateRepository<T> : IAggregateRepository<T> where T : AggregateRoot, new()
    {
        private AggregateContext Context { get; set; }
        
        public AggregateRepository(AggregateContext context)
        {
            Context = context;
        }
        
        public void Save(T aggregateRoot)
        {
            var @events = aggregateRoot.UncommittedEvents();
            var eventDescriptors = @events.Select(x => new EventDescriptor(aggregateRoot.Id, x.EventId, x.EventCommittedTimestamp, x.Payload(),
                x.EventType, x.CorrelationId, x.SagaProcessKey));
            Context.AddRange(eventDescriptors);
            aggregateRoot.Commit();
        }

        public void Load(Guid aggregateId, T type)
        {
            var @events = Context.Events.AsNoTracking()
                .Where(x => x.AggregateId == aggregateId).ToList();
            var listOfEvents = @events.Select(x => DeserializeEvent())
            var aggregate = new T();
            aggregate.Rehydrate(@events);
        }
        
        public static IEvent DeserializeEvent(this EventDescriptor data)
        {
            var payload = (EventDescriptor)JsonConvert.DeserializeObject(StringCompressor.DecompressString(data.EventPayload), typeof(EventPayload), SerializerSettings);
            return (IEvent)JsonConvert.DeserializeObject(payload.EventData, Type.GetType(payload.EventType, AssemblyResolver, null), SerializerSettings);
        }

    }
}