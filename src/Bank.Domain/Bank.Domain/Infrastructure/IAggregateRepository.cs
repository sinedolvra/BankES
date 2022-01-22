using System;
using Bank.Domain.Entities;

namespace Bank.Domain.Infrastructure
{
    public interface IAggregateRepository<T>
    {
        public void Save(T aggregateRoot);
        public void Load(Guid aggregateId, T type);
    }
}