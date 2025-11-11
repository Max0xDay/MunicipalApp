/*
========  AVLNode.cs  ========
Purpose: AVLNodes in code are used for maintaining balanced binary search trees.
Why its used: In the Sidequest municipal app, AVL trees help manage and sort service requests

In detail:
This AVL node capsule gives us a self-balancing wrapper around service requests so the tree
stays height-aware no matter how chaotic the municipal ticket stream becomes. By pairing each
request with cached height data for its subtrees, we can reason about balance factors in
constant time and keep rotations cheap even when the workload spikes.

Inside the app this node is instantiated every time a request flows into the AVL structure that
powers sorted views and integrity validation. The key mirrors the unique request identifier,
while height snapshots feed the rotations that happen in `ServiceRequestAVL`, ensuring fast
status lookups for the dashboard grids.
=============================
*/
using System;

namespace Sidequest_municiple_app {
    public class AVLNode {
        public ServiceRequest Value { get; private set; }
        public string Key { get; private set; }
        public AVLNode Left { get; set; }
        public AVLNode Right { get; set; }
        public int Height { get; private set; }

        public AVLNode(ServiceRequest value) {
            if (value == null) {
                throw new ArgumentNullException(nameof(value));
            }

            Value = value;
            Key = value.UniqueID;
            Height = 1;
        }

        public void UpdateHeight() {
            int leftHeight = Left?.Height ?? 0;
            int rightHeight = Right?.Height ?? 0;
            Height = Math.Max(leftHeight, rightHeight) + 1;
        }

        public int GetBalance() {
            int leftHeight = Left?.Height ?? 0;
            int rightHeight = Right?.Height ?? 0;
            return leftHeight - rightHeight;
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
