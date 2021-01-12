using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Newtonsoft.Json;
using Productoro;
using Productoro.Models;
using Productoro.Extensions;
using DomainEventContract = Productoro.Contracts.DomainEvent;
using System.Linq;
using Productoro.Models.Events;

namespace Productoro.Implementation
{

    internal sealed class SqliteEventStore : IEventStore
    {
        private readonly IDatabase _database;

        public SqliteEventStore(IDatabase database)
        {
            _database = database ?? throw new ArgumentNullException(nameof(database));
        }

        public async Task<IReadOnlyCollection<DomainEvent>> GetEventsAsync()
        {
            const string sql = "SELECT * FROM main.DomainEvents";

            var events = await _database.UsingConnectionAsync((connection, transaction) =>
               connection.QueryAsync<DomainEventContract>(sql, transaction: transaction));

            return DeserializeEvents(events);
        }

        public async Task<IReadOnlyCollection<DomainEvent>> GetEventsAsync(Aggregate aggregate)
        {
            const string sql = @"
                SELECT *
                FROM main.DomainEvents
                WHERE Aggregate = @Aggregate
            ";

            var parameters = new
            {
                Aggregate = aggregate.Value
            };

            var events = await _database.UsingConnectionAsync((connection, transaction) =>
               connection.QueryAsync<DomainEventContract>(sql, parameters, transaction));

            return DeserializeEvents(events);
        }

        public async Task<IReadOnlyCollection<DomainEvent>> GetEventsAsync(Aggregate aggregate, InstanceId instance)
        {
            const string sql = @"
                SELECT *
                FROM main.DomainEvents
                WHERE Aggregate = @Aggregate
                    AND InstanceId = @InstanceId
            ";

            var parameters = new
            {
                Aggregate = aggregate.Value,
                InstanceId = instance.Value // TODO: .ToString() ? Depends on what FluentMigrator does...
            };

            var events = await _database.UsingConnectionAsync((connection, transaction) =>
               connection.QueryAsync<DomainEventContract>(sql, parameters, transaction));

            return DeserializeEvents(events);
        }

        private static IReadOnlyCollection<DomainEvent> DeserializeEvents(IEnumerable<DomainEventContract> events)
        {
            return events
                .Select(Deserialize)
                .ToReadOnlyCollection();

            static DomainEvent Deserialize(DomainEventContract @event)
            {
                var eventType = Type.GetType(@event.Type) ?? throw new InvalidOperationException($"Could not deserialize unknown domain event type '{@event.Type}'.");
                var body = JsonConvert.DeserializeObject(@event.Body, eventType) ?? throw new InvalidOperationException($"Failed to deserialize event because it did not have a body (event type: '{@event.Type}').");
                return @event.ToModel(body);
            }
        }
    }
}