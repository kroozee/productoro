using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Productoro.Models.Events;

namespace Productoro
{
    internal sealed record Aggregate(string Value);
    internal sealed record InstanceId(Guid Value);

    internal interface IEventStore
    {
        Task<IReadOnlyCollection<DomainEvent>> GetEventsAsync();
        Task<IReadOnlyCollection<DomainEvent>> GetEventsAsync(Aggregate aggregate);
        Task<IReadOnlyCollection<DomainEvent>> GetEventsAsync(Aggregate aggregate, InstanceId instance);
    }
}