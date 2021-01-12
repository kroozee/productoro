using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace Productoro
{
    public interface IEventBus
    {
        ValueTask PostAsync<TEvent>(TEvent @event);
        IDisposable Subscribe<TEvent>(Func<TEvent, ValueTask> handleAsync);
    }

    public static class EventBusExtensions
    {
        public static IDisposable Subscribe<TEvent>(this IEventBus @this, Action<TEvent> handler)
        {
            if (@this is null)
            {
                throw new ArgumentNullException(nameof(@this));
            }

            if (handler is null)
            {
                throw new ArgumentNullException(nameof(handler));
            }

            return @this.Subscribe<TEvent>(e =>
            {
                handler(e);
                return new ValueTask();
            });
        }

        public static IObservable<TEvent> Observe<TEvent>(this IEventBus @this)
        {
            if (@this is null)
            {
                throw new ArgumentNullException(nameof(@this));
            }

            return Observable.Create<TEvent>(observer =>
            {
                try
                {
                    return @this.Subscribe<TEvent>(observer.OnNext);
                }
                catch (Exception error)
                {
                    observer.OnError(error);
                }
                return Disposable.Empty;
            });
        }
    }
}