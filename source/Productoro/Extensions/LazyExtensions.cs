using System;

namespace Productoro.Extensions
{
    internal static class LazyExtensions
    {
        public static Lazy<TResult> Select<TValue, TResult>(this Lazy<TValue> @this, Func<TValue, TResult> projection)
        {
            if (@this is null)
            {
                throw new ArgumentNullException(nameof(@this));
            }

            if (projection is null)
            {
                throw new ArgumentNullException(nameof(projection));
            }

            return new Lazy<TResult>(() => projection(@this.Value));
        }
    }
}
