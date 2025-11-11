/*
========  RedBlackNode.cs  ========
Purpose: RedBlackNodes enforce the coloring invariants that keep our red-black tree balanced.
Why its used: In the Sidequest municipal app, these nodes power category-based indexing with guaranteed logarithmic depth.

In detail:
Each node marries a service request to a color flag—red or black—and maintains bidirectional
parent-child linkage so rotation logic can rewrite subtrees without losing context. By keying on
the request's category field, we cluster issues by type while the coloring rules prevent
pathological skew even when categories arrive in sorted bursts.

The `ServiceRequestRedBlackTree` instantiates these nodes on every insert, then manipulates
colors and pointers through uncle checks and rotations. The ServiceRequestStatus form leverages
the resulting structure for stable category queries and color-annotated visualizations that help
operators understand balancing mechanics in real time.
=============================
*/
using System;

namespace Sidequest_municiple_app {
    public enum NodeColor {
        Red,
        Black
    }

    public class RedBlackNode {
        public string Key { get; private set; }
        public ServiceRequest Value { get; private set; }
        public NodeColor Color { get; set; }
        public RedBlackNode Left { get; set; }
        public RedBlackNode Right { get; set; }
        public RedBlackNode Parent { get; set; }

        public RedBlackNode(ServiceRequest request) {
            if (request == null) {
                throw new ArgumentNullException(nameof(request));
            }

            Key = request.Category ?? string.Empty;
            Value = request;
            Color = NodeColor.Red;
        }

        public void UpdateValue(ServiceRequest request) {
            if (request == null) {
                throw new ArgumentNullException(nameof(request));
            }

            Value = request;
        }

        public bool IsRed() {
            return Color == NodeColor.Red;
        }

        public bool IsBlack() {
            return Color == NodeColor.Black;
        }
    }
}
