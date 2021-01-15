using System;
using System.Collections.Generic;
using Optional.Collections;
using Productoro.Models.Aggregates;
using Productoro.Models.Events;

namespace Productoro.Extensions
{
    internal static class ContractExtensions
    {
        private static IReadOnlyDictionary<Type, Func<object, object?>> SupportedEvents { get; } =
            new Dictionary<Type, Func<object, object?>>()
            {
                { typeof(Contracts.ProjectCreated), @event => (@event as Contracts.ProjectCreated)?.ToModel() },
            };

        public static DomainEventInformation<TAggregate, TState> ToModel<TAggregate, TState>(this Contracts.DomainEventInformation @this, Type type, object body)
            where TAggregate : IAggregate<TState>
        {
            if (@this is null)
            {
                throw new ArgumentNullException(nameof(@this));
            }

            if (body is null)
            {
                throw new ArgumentNullException(nameof(body));
            }

            var @event = SupportedEvents
                .GetValueOrNone(type)
                .FlatMap(cast => cast(body).AsOption())
                .FlatMap(@event => (@event as IDomainEvent<TAggregate, TState>).AsOption())
                .Match(
                    some: _ => _,
                    none: () => throw new InvalidOperationException($"Failed to convert event to desired domain event type '{@this.Type}'."));

            // TODO: more validation?
            return new DomainEventInformation<TAggregate, TState>(
                new DomainEventId(@this.Id),
                new AggregateId(@this.AggregateId),
                @event,
                new Timestamp(@this.Timestamp));
        }

        public static ProjectCreated ToModel(this Contracts.ProjectCreated @this)
        {
            if (@this is null)
            {
                throw new ArgumentNullException(nameof(@this));
            }

            return new ProjectCreated(new Models.ProjectId(@this.Id), new Models.ProjectName(@this.Name));
        }
    }
}