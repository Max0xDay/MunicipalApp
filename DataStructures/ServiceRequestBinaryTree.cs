using System;
using System.Collections.Generic;

namespace Sidequest_municiple_app {
    public class ServiceRequestBinaryTree {
        private BinaryTreeNode root;
        private int count;

        public int Count => count;

        public void Insert(ServiceRequest request) {
            if (request == null) {
                throw new ArgumentNullException(nameof(request));
            }

            if (root == null) {
                root = new BinaryTreeNode(request, 0);
                count++;
                return;
            }

            Queue<BinaryTreeNode> queue = new Queue<BinaryTreeNode>();
            queue.Enqueue(root);

            while (queue.Count > 0) {
                BinaryTreeNode current = queue.Dequeue();

                if (current.Left == null) {
                    current.Left = new BinaryTreeNode(request, current.Level + 1);
                    count++;
                    return;
                }
                else {
                    queue.Enqueue(current.Left);
                }

                if (current.Right == null) {
                    current.Right = new BinaryTreeNode(request, current.Level + 1);
                    count++;
                    return;
                }
                else {
                    queue.Enqueue(current.Right);
                }
            }
        }

        public IReadOnlyList<ServiceRequest> LevelOrder() {
            List<ServiceRequest> items = new List<ServiceRequest>();

            if (root == null) {
                return items;
            }

            Queue<BinaryTreeNode> queue = new Queue<BinaryTreeNode>();
            queue.Enqueue(root);

            while (queue.Count > 0) {
                BinaryTreeNode current = queue.Dequeue();
                items.Add(current.Value);

                if (current.Left != null) {
                    queue.Enqueue(current.Left);
                }

                if (current.Right != null) {
                    queue.Enqueue(current.Right);
                }
            }

            return items;
        }

        public IReadOnlyList<ServiceRequest> PreOrder() {
            List<ServiceRequest> items = new List<ServiceRequest>();
            PreOrderTraversal(root, items);
            return items;
        }

        public IReadOnlyList<ServiceRequest> InOrder() {
            List<ServiceRequest> items = new List<ServiceRequest>();
            InOrderTraversal(root, items);
            return items;
        }

        public IReadOnlyList<ServiceRequest> PostOrder() {
            List<ServiceRequest> items = new List<ServiceRequest>();
            PostOrderTraversal(root, items);
            return items;
        }

        public void Clear() {
            root = null;
            count = 0;
        }

        private void PreOrderTraversal(BinaryTreeNode node, List<ServiceRequest> items) {
            if (node == null) {
                return;
            }

            items.Add(node.Value);
            PreOrderTraversal(node.Left, items);
            PreOrderTraversal(node.Right, items);
        }

        private void InOrderTraversal(BinaryTreeNode node, List<ServiceRequest> items) {
            if (node == null) {
                return;
            }

            InOrderTraversal(node.Left, items);
            items.Add(node.Value);
            InOrderTraversal(node.Right, items);
        }

        private void PostOrderTraversal(BinaryTreeNode node, List<ServiceRequest> items) {
            if (node == null) {
                return;
            }

            PostOrderTraversal(node.Left, items);
            PostOrderTraversal(node.Right, items);
            items.Add(node.Value);
        }
    }
}
