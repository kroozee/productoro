using System;

namespace Productoro
{
    internal interface ITimeProvider
    {
        DateTimeOffset Now();
        DateTimeOffset NowUtc();
    }
}