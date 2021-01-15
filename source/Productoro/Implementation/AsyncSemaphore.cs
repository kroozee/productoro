using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Productoro.Implementation
{
    internal sealed class AsyncSemaphore
    {
        private readonly Queue<TaskCompletionSource<Nothing>> waiters;
        private readonly object padlock;
        private int currentCount;

        public AsyncSemaphore(int initialCount = 1)
        {
            this.waiters = new Queue<TaskCompletionSource<Nothing>>();
            this.padlock = new object();
            this.currentCount = initialCount;
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
            lock (padlock)
            {
                if (currentCount > 0)
                {
                    currentCount -= 1;
                    return new ValueTask();
                }
                else
                {
                    var waiter = new TaskCompletionSource<Nothing>();
                    waiters.Enqueue(waiter);
                    return new ValueTask(waiter.Task);
                }
            }
        }

        private void Release()
        {
            TaskCompletionSource<Nothing>? toRelease = null;
            lock (padlock)
            {
                if (waiters.Count > 0)
                {
                    toRelease = waiters.Dequeue();
                }
                else
                {
                    currentCount += 1;
                }
            }
            toRelease?.SetResult(Nothing.Default);
        }

        public readonly struct Releaser : IDisposable
        {
            private readonly AsyncSemaphore instance;

            public Releaser(AsyncSemaphore instance) =>
                this.instance = instance ?? throw new ArgumentNullException(nameof(instance));

            public void Dispose() =>
                instance.Release();
        }
    }
}