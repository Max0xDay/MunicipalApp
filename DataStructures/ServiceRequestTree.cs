using System;
using System.Collections.Generic;

namespace Sidequest_municiple_app {
    public class ServiceRequestTree {
        private TreeNode root;
        private int count;

        public ServiceRequestTree() {
            root = new TreeNode("Municipal Services", null);
        }

        public int Count => count;

        public void Insert(ServiceRequest request) {
            if (request == null) {
                throw new ArgumentNullException(nameof(request));
            }

            string category = request.Category ?? "Uncategorized";
            string priority = request.GetPriorityString();

            TreeNode categoryNode = root.FindChild(category);
            if (categoryNode == null) {
                categoryNode = root.AddChild(category, null);
            }

            TreeNode priorityNode = categoryNode.FindChild(priority);
            if (priorityNode == null) {
                priorityNode = categoryNode.AddChild(priority, null);
            }

            priorityNode.AddChild(request.UniqueID, request);
            count++;
        }

        public IReadOnlyList<ServiceRequest> GetAll() {
            List<ServiceRequest> items = new List<ServiceRequest>();
            CollectAll(root, items);
            return items;
        }

        public IReadOnlyList<TreeNode> GetHierarchy() {
            List<TreeNode> nodes = new List<TreeNode>();
            CollectHierarchy(root, nodes);
            return nodes;
        }

        public void Clear() {
            root = new TreeNode("Municipal Services", null);
            count = 0;
        }

        private void CollectAll(TreeNode node, List<ServiceRequest> items) {
            if (node == null) {
                return;
            }

            if (node.Value != null) {
                items.Add(node.Value);
            }

            foreach (TreeNode child in node.Children) {
                CollectAll(child, items);
            }
        }

        private void CollectHierarchy(TreeNode node, List<TreeNode> nodes) {
            if (node == null) {
                return;
            }

            nodes.Add(node);

            foreach (TreeNode child in node.Children) {
                CollectHierarchy(child, nodes);
            }
        }
    }
}
