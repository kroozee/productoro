using System;
using System.Data;
using System.Threading.Tasks;
using Productoro;

namespace Productoro.Implementation
{
    internal sealed class Database : IDatabase
    {
        private readonly IDatabaseConnectionFactory _connectionFactory;

        public Database(IDatabaseConnectionFactory connectionFactory) =>
            _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));

        public ValueTask<T> UsingConnectionAsync<T>(Func<IDbConnection, IDbTransaction, Task<T>> func)
        {
            if (func is null)
            {
                throw new ArgumentNullException(nameof(func));
            }

            return InvokeAsync();

            async ValueTask<T> InvokeAsync()
            {
                using var connection = _connectionFactory.Create();
                connection.Open();
                using var transaction = connection.BeginTransaction();
                var result = await func(connection, transaction).ConfigureAwait(false);
                transaction.Commit();
                connection.Close();
                return result;
            }
        }

        public ValueTask UsingConnectionAsync(Func<IDbConnection, IDbTransaction, Task> func)
        {
            if (func is null)
            {
                throw new ArgumentNullException(nameof(func));
            }

            var task = UsingConnectionAsync(async (conn, tx) =>
            {
                await func(conn, tx).ConfigureAwait(false);
                return 0; // ignored
            }).AsTask();

            return new ValueTask(task);
        }
    }
}