/*
========  BinaryTreeNode.cs  ========
Purpose: BinaryTreeNodes give us a straightforward container for service requests in a complete binary tree.
Why its used: In the Sidequest municipal app, the binary tree provides level-aware insertion and basic traversal patterns.

In detail:
This node wraps each service request with explicit left and right pointers plus a level counter
so the tree can maintain its layered structure. Unlike self-balancing variants, this design
accepts requests wherever the next available slot appears, prioritizing insertion speed over
depth guarantees. The level marker helps debugging and visualization without forcing any
particular ordering discipline.

The `ServiceRequestBinaryTree` relies on these nodes to host requests in breadth-first order,
feeding the form layers that showcase alternative traversal strategies—preorder, inorder,
postorder—so the analytics views can compare structural approaches without rewriting the
underlying request storage.
=============================
*/
using System;

namespace Sidequest_municiple_app {
    public class BinaryTreeNode {
        public ServiceRequest Value { get; private set; }
        public BinaryTreeNode Left { get; set; }
        public BinaryTreeNode Right { get; set; }
        public int Level { get; set; }

        public BinaryTreeNode(ServiceRequest request, int level = 0) {
            if (request == null) {
                throw new ArgumentNullException(nameof(request));
            }

            Value = request;
            Level = level;
        }

        public void UpdateValue(ServiceRequest request) {
            if (request == null) {
                throw new ArgumentNullException(nameof(request));
            }

            Value = request;
        }

        public bool IsLeaf() {
            return Left == null && Right == null;
        }

        public bool HasTwoChildren() {
            return Left != null && Right != null;
        }
    }
}
