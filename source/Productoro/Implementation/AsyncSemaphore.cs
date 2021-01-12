using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Productoro.Implementation
{
    internal sealed class AsyncSemaphore
    {
        private readonly Queue<TaskCompletionSource<Nothing>> _waiters;
        private readonly object _gate;
        private int _currentCount;

        public AsyncSemaphore(int initialCount = 1)
        {
            _waiters = new Queue<TaskCompletionSource<Nothing>>();
            _gate = new object();
            _currentCount = initialCount;
        }

        public ValueTask<Releaser> LockAsync()
        {
            var waitTask = WaitAsync();
            return waitTask.IsCompletedSuccessfully
                ? new ValueTask<Releaser>(new Releaser(this))
                : SlowWaitAsync(waitTask, this);

            static async ValueTask<Releaser> SlowWaitAsync(ValueTask waitTask, AsyncSemaphore instance)
            {
                await waitTask.ConfigureAwait(false);
                return new Releaser(instance);
            }
        }

        public Releaser Lock()
        {
            var waitTask = WaitAsync();
            if (waitTask.IsCompletedSuccessfully)
            {
                return new Releaser(this);
            }
            else
            {
                using var flag = new ManualResetEventSlim(false);
                Exception? error = null;
                Task.Run(async () =>
                {
                    try
                    {
                        await waitTask.ConfigureAwait(false);
                    }
                    catch (Exception exception)
                    {
                        error = exception;
                    }
                    finally
                    {
                        flag.Set();
                    }
                });

                flag.Wait();

                if (error is not null)
                {
                    throw new InvalidOperationException("An error occurred within the lock.", error);
                }

                return new Releaser(this);
            }
        }

        private ValueTask WaitAsync()
        {
            lock (_gate)
            {
                if (_currentCount > 0)
                {
                    _currentCount -= 1;
                    return new ValueTask();
                }
                else
                {
                    var waiter = new TaskCompletionSource<Nothing>();
                    _waiters.Enqueue(waiter);
                    return new ValueTask(waiter.Task);
                }
            }
        }

        private void Release()
        {
            TaskCompletionSource<Nothing>? toRelease = null;
            lock (_gate)
            {
                if (_waiters.Count > 0)
                {
                    toRelease = _waiters.Dequeue();
                }
                else
                {
                    _currentCount += 1;
                }
            }
            toRelease?.SetResult(Nothing.Default);
        }

        public readonly struct Releaser : IDisposable
        {
            private readonly AsyncSemaphore _instance;

            public Releaser(AsyncSemaphore instance) =>
                _instance = instance ?? throw new ArgumentNullException(nameof(instance));

            public void Dispose() =>
                _instance.Release();
        }
    }
}