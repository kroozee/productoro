using System.Collections.Generic;
using System.Linq;

namespace Productoro.Extensions
{
    internal static class EnumerableExtensions
    {
        public static IReadOnlyCollection<T> ToReadOnlyCollection<T>(this IEnumerable<T> @this) =>
            @this.ToList().AsReadOnly();
    }
}