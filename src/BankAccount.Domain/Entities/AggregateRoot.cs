using System;
using System.Collections.Generic;
using System.Linq;
using Bank.Domain.Events;
using Bank.Domain.Infrastructure;

namespace Bank.Domain.Entities
{
    public abstract class AggregateRoot<T> where T : class
    {
        public Guid Id { get; private set; }
        public int Version { get; protected set; } = -1;
        private ICollection<IEvent> _uncommittedEvents = new List<IEvent>();
        public void Commit() => _uncommittedEvents.Clear();
        public void InitializeId() => Id = Guid.NewGuid();
        public IEnumerable<IEvent> UncommittedEvents() => _uncommittedEvents.ToList();
        protected IAggregateRepository<T> Repository { get; }

        public AggregateRoot(IAggregateRepository<T> repository)
        {
            Repository = repository;
        }
        
        public void AddEvent(IEvent @event)
        {
            if (!CanAddEvent(@event))
                throw new AggregateException("Aggregate state mismatch");
            _uncommittedEvents.Add(@event);
            Version++;
        }

        public bool CanAddEvent(IEvent @event)
        {
            return @event.AggregateVersion == Version && Id == @event.EventInfo.AggregateId;
        }
        
        public void Rehydrate(IEnumerable<IEvent> @events)
        {
            foreach (var @event in @events)
            {
                ((dynamic) this).Apply((dynamic) @event);
            }
        }
    }
}