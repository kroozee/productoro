using System;
using System.Collections.Generic;
using System.Linq;
using Optional;
using Optional.Unsafe;

namespace Productoro.Extensions
{
    internal static class EnumerableExtensions
    {
        public static IReadOnlyCollection<T> ToReadOnlyCollection<T>(this IEnumerable<T> @this) =>
            @this.ToList().AsReadOnly();

        public static IEnumerable<TResult> Choose<TValue, TResult>(this IEnumerable<TValue> @this, Func<TValue, Option<TResult>> projection)
        {
            if (@this is null)
            {
                throw new ArgumentNullException(nameof(@this));
            }

            if (projection is null)
            {
                throw new ArgumentNullException(nameof(projection));
            }

            return @this
                .Select(projection)
                .Where(o => o.HasValue)
                .Select(o => o.ValueOrFailure());
        }

        public static IEnumerable<TValue> Choose<TValue>(this IEnumerable<Option<TValue>> @this)
        {
            if (@this is null)
            {
                throw new ArgumentNullException(nameof(@this));
            }

            return @this
                .Choose(_ => _);
        }
    }
}