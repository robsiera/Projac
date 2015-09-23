﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Projac.Connector
{
    internal static class TaskExtensions
    {
        public static Task ExecuteAsync(this IEnumerable<Task> enumerable, CancellationToken cancellationToken)
        {
            if (enumerable == null)
                throw new ArgumentNullException(nameof(enumerable));
            var source = new TaskCompletionSource<object>();
            var enumerator = enumerable.GetEnumerator();
            EnumerateAsyncCore(source, enumerator, cancellationToken);
            return source.Task.
                ContinueWith(
                    next => enumerator.Dispose(),
                    cancellationToken,
                    TaskContinuationOptions.ExecuteSynchronously,
                    TaskScheduler.Current);
        }

        private static void EnumerateAsyncContinuation(Task previous, TaskCompletionSource<object> source, IEnumerator<Task> enumerator, CancellationToken cancellationToken)
        {
            if (!previous.IsCanceled || !previous.IsFaulted)
            {
                EnumerateAsyncCore(source, enumerator, cancellationToken);
            }
        }

        private static void EnumerateAsyncCore(TaskCompletionSource<object> source, IEnumerator<Task> enumerator, CancellationToken cancellationToken)
        {
            try
            {
                if (enumerator.MoveNext())
                {
                    enumerator.Current.
                        ContinueWith(
                            next => EnumerateAsyncContinuation(next, source, enumerator, cancellationToken),
                            cancellationToken,
                            TaskContinuationOptions.ExecuteSynchronously,
                            TaskScheduler.Current);
                }
                else
                {
                    source.SetResult(null);
                }
            }
            catch (Exception exception)
            {
                source.SetException(exception);
            }
        }
    }
}