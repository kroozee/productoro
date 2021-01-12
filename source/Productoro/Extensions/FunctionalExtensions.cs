using System;

namespace Productoro.Extensions
{
    internal static class FunctionalExtensions
    {
        public static TResult PipeTo<TValue, TResult>(this TValue @this, Func<TValue, TResult> function)
        {
            if (@this is null)
            {
                throw new ArgumentNullException(nameof(@this));
            }

            if (function is null)
            {
                throw new ArgumentNullException(nameof(function));
            }

            return function(@this);
        }
    }
}