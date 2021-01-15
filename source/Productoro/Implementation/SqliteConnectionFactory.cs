using System;
using System.Data;
using System.IO;
using Microsoft.Data.Sqlite;
using Productoro.Models;

namespace Productoro.Implementation
{
    internal sealed class SqliteConnectionFactory : IDatabaseConnectionFactory
    {
        private readonly DatabaseConnectionString connectionString;

        public SqliteConnectionFactory(DatabaseConnectionString connectionString) =>
            this.connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));

        public IDbConnection Create()
        {
            var connectionString = this.connectionString.Value;
            var dataSource = new SqliteConnectionStringBuilder(connectionString).DataSource;
            Directory.CreateDirectory(Path.GetDirectoryName(dataSource)
                ?? throw new InvalidOperationException($"Failed to get the directory of the Sqlite data soure '{dataSource}')."));
            var connection = new SqliteConnection(connectionString);
            return connection;
        }
    }
}
