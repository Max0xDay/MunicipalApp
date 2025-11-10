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
