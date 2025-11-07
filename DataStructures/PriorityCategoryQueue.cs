using System;
using System.Collections.Generic;
using System.Linq;

namespace Sidequest_municiple_app
{
    public class PriorityCategoryQueue<T> where T : class
    {
        private SortedDictionary<int, Queue<T>> priorityQueues;
        private Dictionary<int, int> priorityCounts;

        public int Count { get; private set; }

        public PriorityCategoryQueue()
        {
            priorityQueues = new SortedDictionary<int, Queue<T>>();
            priorityCounts = new Dictionary<int, int>();
            Count = 0;
        }

        public void Enqueue(T item, int priority)
        {
            if (!priorityQueues.ContainsKey(priority))
            {
                priorityQueues[priority] = new Queue<T>();
                priorityCounts[priority] = 0;
            }

            priorityQueues[priority].Enqueue(item);
            priorityCounts[priority]++;
            Count++;
        }

        public T Dequeue()
        {
            if (Count == 0)
            {
                throw new InvalidOperationException("Queue is empty");
            }

            int highestPriority = priorityQueues.Keys.First();
            T item = priorityQueues[highestPriority].Dequeue();
            priorityCounts[highestPriority]--;
            Count--;

            if (priorityQueues[highestPriority].Count == 0)
            {
                priorityQueues.Remove(highestPriority);
                priorityCounts.Remove(highestPriority);
            }

            return item;
        }

        public T Peek()
        {
            if (Count == 0)
            {
                throw new InvalidOperationException("Queue is empty");
            }

            int highestPriority = priorityQueues.Keys.First();
            return priorityQueues[highestPriority].Peek();
        }

        public bool Contains(int priority)
        {
            return priorityQueues.ContainsKey(priority);
        }

        public int GetPriorityCount(int priority)
        {
            return priorityCounts.ContainsKey(priority) ? priorityCounts[priority] : 0;
        }

        public IEnumerable<int> GetActivePriorities()
        {
            return priorityQueues.Keys;
        }

        public void Clear()
        {
            priorityQueues.Clear();
            priorityCounts.Clear();
            Count = 0;
        }
    }
}
