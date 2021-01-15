using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Optional;
using Productoro.Extensions;
using Productoro.Models;
using Productoro.Models.Aggregates;
using Productoro.Models.Events;
using DomainEventContract = Productoro.Contracts.DomainEventInformation;

namespace Productoro.Implementation
{
    internal sealed class ProjectClient : IProjectClient
    {
        private readonly IEventStore store;

        public ProjectClient(IEventStore store)
        {
            this.store = store ?? throw new ArgumentNullException(nameof(store));
        }

        public async ValueTask<IReadOnlyCollection<Project>> GetAsync()
        {
            var events = await store.GetEventsAsync(AggregateTypes.Project).ConfigureAwait(false);
            var domainEvents = DeserializeEvents(events);
            var aggregates = new ConcurrentDictionary<AggregateId, IProjectAggregate>();

            return domainEvents
                .GroupBy(@event => @event.AggregateId)
                .Select(eventGroup =>
                    ApplyEvents(
                        aggregates.GetOrAdd(eventGroup.Key, _ => new ProjectAggregate()),
                        eventGroup.Select(@event => @event.Event)))
                .Choose()
                .ToReadOnlyCollection();
        }

        public async ValueTask<Option<Project>> GetAsync(ProjectId id)
        {
            var events = await store.GetEventsAsync(AggregateTypes.Project, id).ConfigureAwait(false);
            var domainEvents = DeserializeEvents(events).Select(@event => @event.Event);
            return ApplyEvents(new ProjectAggregate(), domainEvents);
        }

        private static Option<Project> ApplyEvents(IProjectAggregate aggregate, IEnumerable<IDomainEvent<IProjectAggregate, Project>> events)
        {
            events.ToList().ForEach(@event => @event.Accept(aggregate));
            return aggregate.CurrentState;
        }

        private static IReadOnlyCollection<DomainEventInformation<IProjectAggregate, Project>> DeserializeEvents(IEnumerable<DomainEventContract> events)
        {
            return events
                .Select(Deserialize)
                .ToReadOnlyCollection();

            static DomainEventInformation<IProjectAggregate, Project> Deserialize(DomainEventContract @event)
            {
                var eventType = Type.GetType(@event.Type) ?? throw new InvalidOperationException($"Could not deserialize unknown domain event type '{@event.Type}'.");
                var body = JsonConvert.DeserializeObject(@event.Body, eventType) ?? throw new InvalidOperationException($"Failed to deserialize event because it did not have a body (event type: '{@event.Type}').");
                return @event.ToModel<IProjectAggregate, Project>(eventType, body);
            }
        }
    }

    /*internal sealed class AggregateReadClient : IAggregateReadClient
    {
        private readonly IEventStore _store;

        public AggregateReadClient(IEventStore store)
        {
            _store = store ?? throw new ArgumentNullException(nameof(store));
        }

        public async ValueTask<IReadOnlyCollection<TState>> GetAsync<TAggregate, TState>()
            where TAggregate : IAggregate<TState>, new()
        {
            var aggregateType = new AggregateType(typeof(TAggregate).FullName);
            var events = await _store.GetEventsAsync(aggregateType).ConfigureAwait(false);
            var domainEvents = DeserializeEvents<TAggregate, TState>(events);
            var aggregates = new ConcurrentDictionary<AggregateId, TAggregate>();

            return domainEvents
                .GroupBy(@event => @event.AggregateId)
                .Select(eventGroup =>
                    ApplyEvents(
                        aggregates.GetOrAdd(eventGroup.Key, _ => new TAggregate()),
                        eventGroup.Select(@event => @event.Event)))
                .Choose()
                .ToReadOnlyCollection();
        }

        public async ValueTask<Option<TState>> GetAsync<TAggregate, TState>(AggregateId id)
            where TAggregate : IAggregate<TState>, new()
        {
            var aggregateType = new AggregateType(typeof(TAggregate).FullName);
            var events = await _store.GetEventsAsync(aggregateType, id).ConfigureAwait(false);
            var domainEvents = DeserializeEvents<TAggregate, TState>(events).Select(@event => @event.Event);
            return ApplyEvents(new TAggregate(), domainEvents);
        }

        private static Option<TState> ApplyEvents<TAggregate, TState>(TAggregate aggregate, IEnumerable<IDomainEvent<TAggregate, TState>> events)
            where TAggregate : IAggregate<TState>, new()
        {
            events.ToList().ForEach(@event => @event.Accept(aggregate));
            return aggregate.CurrentState;
        }

        private static IReadOnlyCollection<DomainEventInformation<TAggregate, TState>> DeserializeEvents<TAggregate, TState>(IEnumerable<DomainEventContract> events)
            where TAggregate : IAggregate<TState>, new()
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
    }*/
}