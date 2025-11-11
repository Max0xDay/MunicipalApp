/*
========  HeapNode.cs  ========
Purpose: HeapNodes wrap service requests with priority metadata for efficient min-heap operations.
Why its used: In the Sidequest municipal app, heap nodes enable priority-based triage of urgent tickets.

In detail:
The heap node is our way of wrapping a municipal request with the metadata needed for triage
under pressure. It stores the raw ticket, a normalized priority score, and a timestamp capturing
when the request hit the queue so tie-breaking always favors earlier arrivals.

Whenever the `ServiceRequestHeap` rebuilds or bubbles an item, these nodes carry the payload.
The ServiceRequestStatus form ultimately renders their contents in the priority list view,
meaning this class indirectly governs which issues bubble to the top of the city's attention
when everything goes wrong at once.
=============================
*/
using System;

namespace Sidequest_municiple_app {
    public class HeapNode {
        public ServiceRequest Value { get; private set; }
        public int Priority { get; private set; }
        public DateTime EnqueuedAt { get; private set; }

        public HeapNode(ServiceRequest value, int priority) {
            if (value == null) {
                throw new ArgumentNullException(nameof(value));
            }

            Value = value;
            Priority = priority;
            EnqueuedAt = DateTime.UtcNow;
        }

        public void UpdatePriority(int priority) {
            Priority = priority;
        }

        public void UpdateValue(ServiceRequest value) {
            if (value == null) {
                throw new ArgumentNullException(nameof(value));
            }

            Value = value;
        }
    }
}
