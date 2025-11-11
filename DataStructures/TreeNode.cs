/*
========  TreeNode.cs  ========
Purpose: TreeNodes scaffold an arbitrary-arity hierarchy for municipal service requests organized by category and priority.
Why its used: In the Sidequest municipal app, they drive a multi-level taxonomy that mirrors how city departments classify work.

In detail:
This node links a string key and an optional service request to a variable list of children,
with upward parent references that simplify depth queries and path traversals. By allowing
unbounded branching we can reflect the natural grouping patterns in real-world municipal
data—categories spawn priority tiers, priority tiers spawn individual tickets—without imposing
artificial binary constraints.

The `ServiceRequestTree` uses these nodes to construct a three-tier hierarchy rooted at
"Municipal Services," then branching into categories, then into priority levels, and finally into
leaf nodes holding actual requests. The ServiceRequestStatus form walks this structure to
render collapsible tree views, making it easier for operators to drill into problem domains
without scrolling flat lists.
=============================
*/
using System;
using System.Collections.Generic;

namespace Sidequest_municiple_app {
    public class TreeNode {
        public string Key { get; private set; }
        public ServiceRequest Value { get; private set; }
        public TreeNode Parent { get; set; }
        public List<TreeNode> Children { get; private set; }

        public TreeNode(string key, ServiceRequest request) {
            if (string.IsNullOrWhiteSpace(key)) {
                throw new ArgumentException("Key cannot be empty", nameof(key));
            }

            Key = key;
            Value = request;
            Children = new List<TreeNode>();
        }

        public TreeNode AddChild(string key, ServiceRequest request) {
            TreeNode child = new TreeNode(key, request);
            child.Parent = this;
            Children.Add(child);
            return child;
        }

        public TreeNode FindChild(string key) {
            if (key == null) {
                return null;
            }

            foreach (TreeNode child in Children) {
                if (string.Equals(child.Key, key, StringComparison.OrdinalIgnoreCase)) {
                    return child;
                }
            }

            return null;
        }

        public int GetDepth() {
            int depth = 0;
            TreeNode current = Parent;

            while (current != null) {
                depth++;
                current = current.Parent;
            }

            return depth;
        }

        public bool IsLeaf() {
            return Children.Count == 0;
        }
    }
}
