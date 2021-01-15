using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Productoro.Models.Events;
using DomainEventContract = Productoro.Contracts.DomainEventInformation;

namespace Productoro.Implementation
{
    internal sealed class SqliteEventStore : IEventStore
    {
        private readonly IDatabase database;

        public SqliteEventStore(IDatabase database)
        {
            this.database = database ?? throw new ArgumentNullException(nameof(database));
        }

        public ValueTask<IEnumerable<DomainEventContract>> GetEventsAsync()
        {
            const string sql = @"
                SELECT *
                FROM main.DomainEvents
            ";

            return database.UsingConnectionAsync((connection, transaction) =>
               connection.QueryAsync<DomainEventContract>(sql, transaction: transaction));
        }

        public ValueTask<IEnumerable<DomainEventContract>> GetEventsAsync(AggregateType aggregateType)
        {
            if (aggregateType is null)
            {
                throw new ArgumentNullException(nameof(aggregateType));
            }

            const string sql = @"
                SELECT *
                FROM main.DomainEvents
                WHERE AggregateType = @AggregateType
            ";

            var parameters = new
            {
                AggregateType = aggregateType.Value
            };

            return database.UsingConnectionAsync((connection, transaction) =>
               connection.QueryAsync<DomainEventContract>(sql, parameters, transaction));
        }

        public ValueTask<IEnumerable<DomainEventContract>> GetEventsAsync(AggregateType aggregateType, AggregateId aggregateId)
        {
            if (aggregateType is null)
            {
                throw new ArgumentNullException(nameof(aggregateType));
            }

            if (aggregateId is null)
            {
                throw new ArgumentNullException(nameof(aggregateId));
            }

            const string sql = @"
                SELECT *
                FROM main.DomainEvents
                WHERE AggregateType = @AggregateType
                    AND AggregateId = @AggregateId
            ";

            var parameters = new
            {
                AggregateType = aggregateType.Value,
                AggregateId = aggregateId.Value // TODO: .ToString() ? Depends on what FluentMigrator does...
            };

            return database.UsingConnectionAsync((connection, transaction) =>
               connection.QueryAsync<DomainEventContract>(sql, parameters, transaction));
        }

        public ValueTask WriteAsync(DomainEventContract @event)
        {
            const string sql = @"
                REPLACE INTO main.DomainEvents(Id, AggregateType, AggregateId, Type, Body, Timestamp)
                VALUES (@Id, @AggregateType, @AggregateId, @Type, @Body, @Timestamp);
            ";

            var timestamp = DateTimeOffset.Now; // TODO extract dependency.

            var parameters = new
            {
                Id = @event.Id, // TODO: ToString ? Depends on what fluentmigrator does...
                AggregateType = @event.AggregateType,
                AggregateId = @event.AggregateId,
                Type = @event.GetType().FullName,
                Timestamp = timestamp
            };

            var executeTask = database
                .UsingConnectionAsync((connection, transaction) => connection.ExecuteAsync(sql, parameters, transaction))
                .AsTask();

            return new ValueTask(executeTask);
        }
    }
}