/*
========  PriorityCategoryQueue.cs  ========
Purpose: PriorityCategoryQueue implements a multi-level priority queue with FIFO ordering within each tier.
Why its used: In the Sidequest municipal app, this queue ensures fair processing of same-priority requests.

In detail:
This generic priority queue takes the municipal workload and funnels it through a tiered
structure built on sorted dictionaries of FIFO queues. Instead of a single min-heap we maintain
discrete lanes per priority level, so equal-ranked items keep their submission order—a subtle
requirement that keeps frontline support from being blindsided.

Inside the application it backs the `ServiceRequestHeap` when we want an easy way to group
requests by severity while still allowing category overrides. The ServiceRequestStatus form reads
snapshots from this queue whenever it assembles the priority sidebar, ensuring the UI
communicates urgency and fairness without hitting the database for every refresh.
=============================
*/
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sidequest_municiple_app {
    public class PriorityCategoryQueue<T> where T : class {
        private SortedDictionary<int, Queue<T>> priorityQueues;
        private Dictionary<int, int> priorityCounts;

        public int Count { get; private set; }

        public PriorityCategoryQueue() {
            priorityQueues = new SortedDictionary<int, Queue<T>>();
            priorityCounts = new Dictionary<int, int>();
            Count = 0;
        }

        public void Enqueue(T item, int priority) {
            if (!priorityQueues.ContainsKey(priority)) {
                priorityQueues[priority] = new Queue<T>();
                priorityCounts[priority] = 0;
            }

            priorityQueues[priority].Enqueue(item);
            priorityCounts[priority]++;
            Count++;
        }

        public T Dequeue() {
            if (Count == 0) {
                throw new InvalidOperationException("Queue is empty");
            }

            int highestPriority = priorityQueues.Keys.First();
            T item = priorityQueues[highestPriority].Dequeue();
            priorityCounts[highestPriority]--;
            Count--;

            if (priorityQueues[highestPriority].Count == 0) {
                priorityQueues.Remove(highestPriority);
                priorityCounts.Remove(highestPriority);
            }

            return item;
        }

        public T Peek() {
            if (Count == 0) {
                throw new InvalidOperationException("Queue is empty");
            }

            int highestPriority = priorityQueues.Keys.First();
            return priorityQueues[highestPriority].Peek();
        }

        public bool Contains(int priority) {
            return priorityQueues.ContainsKey(priority);
        }

        public int GetPriorityCount(int priority) {
            return priorityCounts.ContainsKey(priority) ? priorityCounts[priority] : 0;
        }

        public IEnumerable<int> GetActivePriorities() {
            return priorityQueues.Keys;
        }

        public void Clear() {
            priorityQueues.Clear();
            priorityCounts.Clear();
            Count = 0;
        }
    }
}
