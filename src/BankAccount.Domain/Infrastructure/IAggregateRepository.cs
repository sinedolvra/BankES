using System;
using System.Threading.Tasks;

namespace Bank.Domain.Infrastructure
{
    public interface IAggregateRepository<T>
    {
        Task Save(T aggregateRoot);
        Task Load(Guid aggregateId, T type);
    }
}