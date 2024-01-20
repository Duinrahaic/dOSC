using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace dOSCEngine.Utilities
{


    /// <summary>
    /// Manages a concurrent queue of committed items associated with sources.
    /// </summary>
    /// <typeparam name="T">The type of data to be committed.</typeparam>
    public class CommitManager<T>
    {
        private readonly ConcurrentQueue<(T data, string source)> queue = new ConcurrentQueue<(T, string)>();
        private readonly ConcurrentDictionary<(T data, string source), bool> committedSources = new ConcurrentDictionary<(T, string), bool>(TupleComparer.Default);

        /// <summary>
        /// Commits data to the queue, associating it with a source.
        /// </summary>
        /// <param name="data">The data to be committed.</param>
        /// <param name="source">The source associated with the committed data.</param>
        public void Commit(T data, string source)
        {
            var commit = (data, source);

            // Check if the same commit already exists in the queue
            if (committedSources.TryAdd(commit, true))
            {
                // If not, enqueue the data along with the source
                queue.Enqueue(commit);
            }
            // No logging for duplicate commits
        }

        /// <summary>
        /// Retrieves the items in the queue without dequeuing them.
        /// </summary>
        /// <returns>An enumerable of committed items in the queue.</returns>
        public IEnumerable<(T data, string source)> PeekQueue()
        {
            // Return the items in the queue without dequeuing them
            return queue.ToArray();
        }

        /// <summary>
        /// Retrieves the next item in the queue without dequeuing it.
        /// </summary>
        /// <returns>The next committed item in the queue, or default if the queue is empty.</returns>
        public (T data, string source) PeekNext()
        {
            return queue.TryPeek(out var nextItem) ? nextItem : default;
        }

        /// <summary>
        /// Removes a source from the committed sources and dequeues the corresponding items from the queue.
        /// </summary>
        /// <param name="source">The source to be removed.</param>
        /// <returns>True if the source was successfully removed, false otherwise.</returns>
        public bool RemoveCommit(string source)
        {
            lock (queue)
            {
                // Create a dummy tuple with default data and the specified source for removal
                var dummyTuple = committedSources.FirstOrDefault(x=>x.Key.source == source);
                // Remove the source from committedSources
                if (committedSources.TryRemove(dummyTuple.Key, out _))
                {

                    // Dequeue the corresponding item(s) from the queue
                    var itemsToRemove = queue.Where(item => item.source == source).ToList();
                    foreach (var itemToRemove in itemsToRemove)
                    {
                        queue.TryDequeue(out _);
                    }

                    return true;
                }

            }


            return false;
        }

        /// <summary>
        /// Checks if there are any committed items in the queue.
        /// </summary>
        /// <returns>True if there are committed items, false otherwise.</returns>
        public bool HasCommits()
        {
            // Check if there are any items in the queue
            return queue.Any();
        }

        private class TupleComparer : IEqualityComparer<(T data, string source)>
        {
            public static readonly TupleComparer Default = new TupleComparer();

            public bool Equals((T data, string source) x, (T data, string source) y)
            {
                return EqualityComparer<T>.Default.Equals(x.data, y.data) && x.source == y.source;
            }

            public int GetHashCode((T data, string source) obj)
            {
                return HashCode.Combine(EqualityComparer<T>.Default.GetHashCode(obj.data), obj.source);
            }
        }
    }
}
