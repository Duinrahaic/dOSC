using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dOSCEngine.Utilities
{
    /// <summary>
    /// Represents a queue processor that asynchronously processes items in a queue.
    /// </summary>
    /// <typeparam name="T">The type of items to be processed.</typeparam>
    public class QueueProcessor<T> : IDisposable
    {
        // Concurrent queue to store items and their associated custom processing functions
        private readonly ConcurrentQueue<(T Item, Func<T, Task> ProcessFunc, Action<T> Callback)> itemQueue =
            new ConcurrentQueue<(T Item, Func<T, Task> ProcessFunc, Action<T> Callback)>();

        // Semaphore to ensure thread safety when processing items
        private readonly SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);

        // CancellationTokenSource to handle cancellation of processing
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        // Task responsible for processing the item queue
        private Task processingTask;

        // Default asynchronous processing function
        private readonly Func<T, Task> processItemAsync;

        // Callback function invoked before processing each item
        private readonly Action<T> processItemCallback;

        // Flag indicating whether processing should be sequential
        private bool isSequential;

        /// <summary>
        /// Initializes a new instance of the <see cref="QueueProcessor{T}"/> class.
        /// </summary>
        /// <param name="customProcessItemAsync">The custom asynchronous processing function.</param>
        /// <param name="customProcessItemCallback">The custom callback function.</param>
        /// <param name="isSequential">Flag indicating whether processing should be sequential.</param>
        public QueueProcessor(
            Func<T, Task> customProcessItemAsync = null,
            Action<T> customProcessItemCallback = null,
            bool isSequential = false)
        {
            // Use the provided custom processing logic, or use a default implementation
            this.processItemAsync = customProcessItemAsync ?? DefaultProcessItemAsync;

            // Use the provided custom callback logic, or use a default implementation
            this.processItemCallback = customProcessItemCallback ?? DefaultProcessItemCallback;

            // Set the processing mode (sequential or parallel)
            this.isSequential = isSequential;
        }

        /// <summary>
        /// Enqueues an item with an optional custom processing and callback functions.
        /// </summary>
        /// <param name="item">The item to enqueue.</param>
        /// <param name="customProcessItemAsync">The custom asynchronous processing function for the item.</param>
        /// <param name="customCallback">The custom callback function for the item.</param>
        public void EnqueueItem(T item, Func<T, Task> customProcessItemAsync = null, Action<T> customCallback = null)
        {
            itemQueue.Enqueue((item, customProcessItemAsync, customCallback));
        }

        /// <summary>
        /// Peeks at the items currently in the queue without removing them.
        /// </summary>
        /// <returns>An enumerable of items currently in the queue.</returns>
        public IEnumerable<T> PeekQueue()
        {
            return itemQueue.Select(item => item.Item).ToArray();
        }

        /// <summary>
        /// Gets the number of items in the queue.
        /// </summary>
        /// <returns>The number of items in the queue.</returns>
        public int GetQueueCount()
        {
            return itemQueue.Count;
        }

        /// <summary>
        /// Clears the queue
        /// </summary>
        public void ClearQueue()
        {
            itemQueue.Clear();
        }
        
        
        
        /// <summary>
        /// Checks if there are any items in the queue.
        /// </summary>
        /// <returns>True if there are items in the queue, otherwise false.</returns>
        public bool HasItemsInQueue()
        {
            return itemQueue.Any();
        }

        /// <summary>
        /// Starts processing the item queue.
        /// </summary>
        public void StartProcessing()
        {
            processingTask = isSequential
                ? Task.Run(() => ProcessQueueSequential())
                : Task.Run(() => ProcessQueueAsync());
        }

        /// <summary>
        /// Stops processing the item queue asynchronously.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task StopProcessingAsync()
        {
            cancellationTokenSource.Cancel();
            await processingTask; // Await the completion of the processing task
        }

        /// <summary>
        /// Sets whether processing should be sequential or parallel.
        /// </summary>
        /// <param name="sequential">Flag indicating whether processing should be sequential.</param>
        public void SetSequentialProcessing(bool sequential)
        {
            isSequential = sequential;
        }

        /// <summary>
        /// Asynchronously processes the item queue.
        /// </summary>
        private async Task ProcessQueueAsync()
        {
            try
            {
                while (!cancellationTokenSource.Token.IsCancellationRequested)
                {
                    // Dequeue an item and its associated custom processing and callback functions
                    if (itemQueue.TryDequeue(out (T Item, Func<T, Task> ProcessFunc, Action<T> Callback) item))
                    {
                        processItemCallback?.Invoke(item.Item); // Invoke the callback before processing
                        await (item.ProcessFunc?.Invoke(item.Item) ?? processItemAsync(item.Item));
                    }
                    else
                    {
                        // Optionally, add a delay or perform other actions when the queue is empty
                        await Task.Delay(100);
                    }
                }
            }
            catch (OperationCanceledException)
            {
                // Handle cancellation if needed
            }
        }

        /// <summary>
        /// Sequentially processes the item queue.
        /// </summary>
        private async Task ProcessQueueSequential()
        {
            try
            {
                while (!cancellationTokenSource.Token.IsCancellationRequested)
                {
                    // Dequeue an item and its associated custom processing and callback functions
                    if (itemQueue.TryDequeue(out (T Item, Func<T, Task> ProcessFunc, Action<T> Callback) item))
                    {
                        processItemCallback?.Invoke(item.Item); // Invoke the callback before processing
                        (item.ProcessFunc?.Invoke(item.Item) ?? processItemAsync(item.Item)).GetAwaiter().GetResult();
                    }
                    else
                    {
                        // Optionally, add a delay or perform other actions when the queue is empty
                        await Task.Delay(100);
                    }
                }
            }
            catch (OperationCanceledException)
            {
                // Handle cancellation if needed
            }
        }

        /// <summary>
        /// Default asynchronous processing function.
        /// </summary>
        /// <param name="item">The item to be processed.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        private async Task DefaultProcessItemAsync(T item)
        {
            await semaphore.WaitAsync();

            try
            {
                // Default processing logic

                // Simulate work by delaying asynchronously
                await Task.Delay(100);
            }
            finally
            {
                semaphore.Release();
            }
        }

        /// <summary>
        /// Default callback function.
        /// </summary>
        /// <param name="item">The item for which the callback is invoked.</param>
        private void DefaultProcessItemCallback(T item)
        {
            // Default callback logic
        }

        /// <summary>
        /// Disposes of the resources used by the <see cref="QueueProcessor{T}"/>.
        /// </summary>
        public void Dispose()
        {
            cancellationTokenSource.Cancel(); // Stop processing
            cancellationTokenSource.Dispose();
            semaphore.Dispose();
        }
    }
}