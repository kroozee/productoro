using System;

namespace Productoro.Models
{
    public sealed record Session(DateTimeOffset Start, TimeSpan Elapsed);
}