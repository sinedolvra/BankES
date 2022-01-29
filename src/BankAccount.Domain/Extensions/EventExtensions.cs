using System;
using System.Reflection;
using Bank.Domain.Entities;
using Bank.Domain.Events;

namespace Bank.Domain.Extensions
{
    public static class EventExtensions
    {
        public static void InvokeOnAggregate<T>(this IEvent @event, AggregateRoot<T> aggregate, string methodName) where T : class
        {
            var method = aggregate.GetType().GetRuntimeMethod(methodName, new[] { @event.GetType() }); //Find the right method

            if (method == null)
            {
                throw new AggregateEventOnApplyMethodMissingException(
                    $"No event Apply method found on {aggregate.GetType()} for {@event.GetType()}");
            }

            method.Invoke(aggregate, new object[] {@event});
        }
    }
}