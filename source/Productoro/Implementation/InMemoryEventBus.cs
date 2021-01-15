using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Threading.Tasks;
using Optional.Collections;
using Productoro.Extensions;

namespace Productoro.Implementation
{
    internal sealed class InMemoryEventBus : IEventBus
    {
        public static IEventBus Instance { get; } = new InMemoryEventBus();
        private readonly ConcurrentDictionary<Type, object> subscriptions;

        internal InMemoryEventBus() =>
            subscriptions = new ConcurrentDictionary<Type, object>();

        public ValueTask PostAsync<TEvent>(TEvent @event) =>
             subscriptions
                .GetValueOrNone(typeof(TEvent))
                .FlatMap(value => (value as EventHandlers<TEvent>).AsOption())
                .Match(
                    some: handlers => handlers.HandleAsync(@event),
                    none: () => new ValueTask());

        public IDisposable Subscribe<TEvent>(Func<TEvent, ValueTask> handler) =>
            subscriptions
                .GetOrAdd(typeof(TEvent), _ => new EventHandlers<TEvent>())
                .PipeTo(handlers => (handlers as EventHandlers<TEvent>).AsOption())
                .Match(
                    some: handlers => handlers.Register(handler),
                    none: () => Disposable.Empty);

        private sealed class EventHandlers<TEvent>
        {
            private readonly AsyncSemaphore _padLock;
            private readonly List<Func<TEvent, ValueTask>> _handlers;

            public EventHandlers()
            {
                _padLock = new AsyncSemaphore();
                _handlers = new List<Func<TEvent, ValueTask>>();
            }

            public IDisposable Register(Func<TEvent, ValueTask> handler)
            {
                using var _ = _padLock.Lock();
                _handlers.Add(handler);
                return Disposable.Create(() =>
                {
                    if (handler is not null)
                    {
                        Deregister(handler);
                    }
                });
            }

            private void Deregister(Func<TEvent, ValueTask> handler)
            {
                using var _ = _padLock.Lock();
                _handlers.Remove(handler);
            }

            public ValueTask HandleAsync(TEvent @event)
            {
                return InvokeHandlersAsync(@event, _handlers, _padLock);

                static async ValueTask InvokeHandlersAsync(TEvent @event, IReadOnlyCollection<Func<TEvent, ValueTask>> handlers, AsyncSemaphore padlock)
                {
                    using var _ = await padlock.LockAsync().ConfigureAwait(false);
                    foreach (var handler in handlers)
                    {
                        await handler(@event).ConfigureAwait(false);
                    }
                }
            }
        }
    }
}