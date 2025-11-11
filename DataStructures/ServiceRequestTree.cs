/*
========  ServiceRequestTree.cs  ========
Purpose: ServiceRequestTree organizes municipal tickets into a three-tier taxonomy rooted by category and priority.
Why its used: In the Sidequest municipal app, this n-ary tree mirrors the natural classification hierarchy of city services.

In detail:
This class constructs a multi-level tree where the root represents the entire municipal service
domain, first-level children are categories, second-level children are priority tiers, and leaf
nodes hold individual service requests. By allowing each parent to spawn arbitrary numbers of
children, the structure adapts to however many categories or priority levels exist in the live
dataset without hardcoding assumptions about branching factors.

The ServiceRequestStatus form leverages this tree for hierarchical views that let operators
expand categories to see priorities, then expand priorities to see individual tickets. The
insertion logic handles missing intermediate nodes on the fly, so even if a new category appears
mid-session, the tree accommodates it seamlessly. The resulting hierarchy feeds collapsible
grid controls and tree-view widgets that simplify navigation across large municipal workloads.
=============================
*/
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
