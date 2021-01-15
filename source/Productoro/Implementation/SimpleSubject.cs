using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace Productoro.Implementation
{
    internal sealed class SimpleSubject<TValue> : ISubject<TValue>
    {
        private event EventHandler<TValue>? Next;
        private event EventHandler<Exception>? Error;
        private event EventHandler? Complete;

        private readonly IObservable<TValue> observable;

        public SimpleSubject(params TValue[] initialValues)
        {
            observable = Observable
                .Create<TValue>(o => CreateObserveable(o))
                .StartWith(initialValues);

            IDisposable CreateObserveable(IObserver<TValue> observer)
            {
                if (observer is null)
                {
                    throw new InvalidOperationException("Observer cannot be null.");
                }

                Next += HandleOnNext;
                Error += HandleOnError;
                Complete += HandleCompletion;

                return Disposable.Create(() =>
                {
                    Next -= HandleOnNext;
                    Error -= HandleOnError;
                    Complete -= HandleCompletion;
                });

                void HandleOnNext(object? sender, TValue value) =>
                    observer.OnNext(value);

                void HandleOnError(object? sender, Exception exception) =>
                    observer.OnError(exception);

                void HandleCompletion(object? sender, EventArgs eventArgs) =>
                    observer.OnCompleted();
            }
        }

        public IDisposable Subscribe(IObserver<TValue> observer)
        {
            if (observer is null)
            {
                throw new ArgumentNullException(nameof(observer));
            }

            return observable.Subscribe(observer);
        }

        public void OnNext(TValue value) =>
            Next?.Invoke(this, value);

        public void OnError(Exception error) =>
            Error?.Invoke(this, error);

        public void OnCompleted() =>
            Complete?.Invoke(this, EventArgs.Empty);
    }
}