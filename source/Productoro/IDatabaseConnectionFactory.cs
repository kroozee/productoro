using System.Data;

namespace Productoro
{
    internal interface IDatabaseConnectionFactory
    {
        IDbConnection Create();
    }
}