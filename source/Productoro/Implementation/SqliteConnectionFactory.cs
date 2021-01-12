using System;
using System.Data;
using System.IO;
using Microsoft.Data.Sqlite;
using Productoro.Models;

namespace Productoro.Implementation
{
    internal sealed class SqliteConnectionFactory : IDatabaseConnectionFactory
    {
        private readonly DatabaseConnectionString _connectionString;

        public SqliteConnectionFactory(DatabaseConnectionString connectionString) =>
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));

        public IDbConnection Create()
        {
            var connectionString = _connectionString.Value;
            var dataSource = new SqliteConnectionStringBuilder(connectionString).DataSource;
            Directory.CreateDirectory(Path.GetDirectoryName(dataSource)
                ?? throw new InvalidOperationException($"Failed to get the Sqlite data soure directory (data source from connection string: '{dataSource}')."));
            var connection = new SqliteConnection(connectionString);
            return connection;
        }
    }
}
