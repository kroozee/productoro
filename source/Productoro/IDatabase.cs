using System;
using System.Data;
using System.Threading.Tasks;

namespace Productoro
{
    internal interface IDatabase
    {
        ValueTask<T> UsingConnectionAsync<T>(Func<IDbConnection, IDbTransaction, Task<T>> func);
        ValueTask UsingConnectionAsync(Func<IDbConnection, IDbTransaction, Task> func);
    }
}