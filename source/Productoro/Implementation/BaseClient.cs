using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Optional;
using Productoro.Extensions;
using Productoro.Models.Aggregates;
using Productoro.Models.Events;
using DomainEventContract = Productoro.Contracts.DomainEventInformation;

namespace Productoro.Implementation
{
    internal abstract class BaseClient<TAggregate, TState>
        where TAggregate : IAggregate<TState>
    {
        private readonly IEventStore store;

        public BaseClient(IEventStore store)
        {
            this.store = store ?? throw new ArgumentNullException(nameof(store));
        }

        protected abstract AggregateType AggregateType { get; }

        protected abstract Func<TAggregate> AggregateFactory { get; }

        public async ValueTask<IReadOnlyCollection<TState>> GetAsync()
        {
            var events = await store.GetEventsAsync(AggregateTypes.Project).ConfigureAwait(false);
            var domainEvents = DeserializeEvents(events);
            var aggregates = new ConcurrentDictionary<AggregateId, TAggregate>();

            return domainEvents
                .GroupBy(@event => @event.AggregateId)
                .Select(eventGroup =>
                    ApplyEvents(
                        aggregates.GetOrAdd(eventGroup.Key, _ => AggregateFactory()),
                        eventGroup.Select(@event => @event.Event)))
                .Choose()
                .ToReadOnlyCollection();
        }

        public async ValueTask<Option<TState>> GetAsync(AggregateId id)
        {
            var events = await store.GetEventsAsync(AggregateTypes.Project, id).ConfigureAwait(false);
            var domainEvents = DeserializeEvents(events).Select(@event => @event.Event);
            return ApplyEvents(AggregateFactory(), domainEvents);
        }

        private static IReadOnlyCollection<DomainEventInformation<TAggregate, TState>> DeserializeEvents(IEnumerable<DomainEventContract> events)
        {
            return events
                .Select(Deserialize)
                .ToReadOnlyCollection();

            static DomainEventInformation<TAggregate, TState> Deserialize(DomainEventContract @event)
            {
                var eventType = Type.GetType(@event.Type) ?? throw new InvalidOperationException($"Could not deserialize unknown domain event type '{@event.Type}'.");
                var body = JsonConvert.DeserializeObject(@event.Body, eventType) ?? throw new InvalidOperationException($"Failed to deserialize event because it did not have a body (event type: '{@event.Type}').");
                return @event.ToModel<TAggregate, TState>(eventType, body);
            }
        }

        private static Option<TState> ApplyEvents(TAggregate aggregate, IEnumerable<IDomainEvent<TAggregate, TState>> events)
        {
            events.ToList().ForEach(@event => @event.Accept(aggregate));
            return aggregate.CurrentState;
        }
    }
}