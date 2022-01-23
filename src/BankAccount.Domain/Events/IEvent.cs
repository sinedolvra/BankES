using System;

namespace Bank.Domain.Events
{
    public interface IEvent
    {
        EventInfo EventInfo { get; set; }
        int AggregateVersion { get; set; }
        string EventType { get; set; }
        string Payload();
    }
}