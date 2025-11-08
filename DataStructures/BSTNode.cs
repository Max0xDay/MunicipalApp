/*
This binary search tree node is the calm center of our alphabetical indexing strategy, keeping
each service request tied to its unique identifier while exposing direct links to its neighbors.
Because the municipal dataset leans heavily on string-based IDs, the node compares using an
ordinal-insensitive check to stay resilient against user input quirks.

Within the broader application the node underpins the `ServiceRequestBST` class that feeds
sorted listings and historical navigation. Every insert or lookup in that structure travels
through these key comparisons, giving the ServiceRequestStatus form a deterministic way to
surface records without hitting the database on each keystroke.
*/
using System;

namespace Sidequest_municiple_app {
    public class BSTNode {
        public ServiceRequest Value { get; private set; }
        public string Key { get; private set; }
        public BSTNode Left { get; set; }
        public BSTNode Right { get; set; }

        public BSTNode(ServiceRequest value) {
            if (value == null) {
                throw new ArgumentNullException(nameof(value));
            }

            Value = value;
            Key = value.UniqueID;
        }

        public int CompareTo(string key) {
            if (key == null) {
                throw new ArgumentNullException(nameof(key));
            }

            return string.Compare(Key, key, StringComparison.OrdinalIgnoreCase);
        }

        public void UpdateValue(ServiceRequest value) {
            if (value == null) {
                throw new ArgumentNullException(nameof(value));
            }

            Value = value;
            Key = value.UniqueID;
        }
    }
}
