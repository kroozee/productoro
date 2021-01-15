using System.Collections.Generic;
using System.Threading.Tasks;
using Productoro.Models.Events;
using DomainEventContract = Productoro.Contracts.DomainEventInformation;
namespace Productoro
{
    internal interface IEventStore
    {
        ValueTask<IEnumerable<DomainEventContract>> GetEventsAsync();
        ValueTask<IEnumerable<DomainEventContract>> GetEventsAsync(AggregateType aggregateType);
        ValueTask<IEnumerable<DomainEventContract>> GetEventsAsync(AggregateType aggregateType, AggregateId aggregateId);
        ValueTask WriteAsync(DomainEventContract eventInformation);
    }
}