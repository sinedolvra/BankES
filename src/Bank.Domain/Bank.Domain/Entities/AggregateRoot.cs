﻿using System;
using System.Collections.Generic;
using System.Linq;
using Bank.Domain.Events;

namespace Bank.Domain.Entities
{
    public class AggregateRoot
    {
        public Guid Id { get; private set; }
        public int Version { get; set; } = -1;
        private ICollection<IEvent> _uncommittedEvents = new List<IEvent>();
        protected void Add(Event @event) => _uncommittedEvents.Add(@event);
        public void Commit() => _uncommittedEvents.Clear();
        protected void InitializeId() => Guid.NewGuid();
        public List<IEvent> UncommittedEvents() => _uncommittedEvents.ToList();

        public void Rehydrate(IEnumerable<IEvent> @events)
        {
            foreach (var @event in @events)
            {
                ((dynamic) this).Apply((dynamic) @event);
            }
        }
    }
}