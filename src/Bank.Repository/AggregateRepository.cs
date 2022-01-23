using System;
using System.Linq;
using System.Threading.Tasks;
using Bank.Domain.Entities;
using Bank.Domain.Events;
using Bank.Domain.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Bank.Domain.Extensions;

namespace Bank.Repository
{
    public class AggregateRepository<T> : IAggregateRepository<T> where T : AggregateRoot<T>, new()
    {
        private AggregateContext Context { get; }
        
        public AggregateRepository(AggregateContext context)
        {
            Context = context;
        }
        
        public async Task Save(T aggregateRoot)
        {
            var @events = aggregateRoot.UncommittedEvents();
            var eventDescriptors = @events.Select(x => new EventDescriptor(aggregateRoot.Id, x.EventId, x.EventCommittedTimestamp, x.Payload(),
                x.EventType, x.CorrelationId, x.SagaProcessKey));
            await Context.AddRangeAsync(eventDescriptors);
            aggregateRoot.Commit();
        }

        public Task Load(Guid aggregateId, T type)
        {
            var @events =  Context.Events.AsNoTracking()
                .Where(x => x.AggregateId == aggregateId)
                .OrderBy(x => x.AggregateVersion)
                .ToList();
            var listOfEvents = @events.Select(x => x.DeserializeEvent());
            var aggregate = new T();
            aggregate.Rehydrate(@listOfEvents);
            return Task.CompletedTask;
        }
    }
}