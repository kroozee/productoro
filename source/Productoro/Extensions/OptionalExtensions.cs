using Optional;

namespace Productoro.Extensions
{
    internal static class OptionalExtensions
    {
        public static Option<T> AsOption<T>(this T? @this) =>
            @this is null
                ? Option.None<T>()
                : Option.Some(@this);
    }
}
