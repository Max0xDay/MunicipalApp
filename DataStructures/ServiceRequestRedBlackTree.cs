/*
========  ServiceRequestRedBlackTree.cs  ========
Purpose: ServiceRequestRedBlackTree maintains a self-balancing tree indexed by category with strict color invariants.
Why its used: In the Sidequest municipal app, this tree guarantees logarithmic lookup times for category-based queries.

In detail:
This class orchestrates red-black insertion mechanics—uncle checks, rotations, recoloring—to
preserve the five red-black properties across every insert. By keying on request categories it
clusters similar issues together, and the balancing discipline ensures no single branch
degenerates into a linked list even when categories arrive in alphabetical order. The exposed
node-with-color snapshots let UI components render the tree's internal state for debugging or
educational displays.

The ServiceRequestStatus form uses this structure when category-driven navigation matters more
than chronological ordering. During refreshes it loads requests from SQLite, inserts them by
category, and later walks the tree in-order to display grouped results. The color annotations
feed visualization widgets that help operators understand why certain rotations occurred and how
the tree stays balanced under asymmetric workloads.
=============================
*/
using System;
using System.Collections.Generic;

namespace Sidequest_municiple_app {
    public class ServiceRequestRedBlackTree {
        private RedBlackNode root;
        private int count;

        public int Count => count;

        public void Insert(ServiceRequest request) {
            if (request == null) {
                throw new ArgumentNullException(nameof(request));
            }

            RedBlackNode newNode = new RedBlackNode(request);
            
            if (root == null) {
                root = newNode;
                root.Color = NodeColor.Black;
                count++;
                return;
            }

            RedBlackNode current = root;
            RedBlackNode parent = null;

            while (current != null) {
                parent = current;
                int comparison = string.Compare(newNode.Key, current.Key, StringComparison.OrdinalIgnoreCase);
                
                if (comparison < 0) {
                    current = current.Left;
                }
                else if (comparison > 0) {
                    current = current.Right;
                }
                else {
                    current.UpdateValue(request);
                    return;
                }
            }

            newNode.Parent = parent;
            int parentComparison = string.Compare(newNode.Key, parent.Key, StringComparison.OrdinalIgnoreCase);
            
            if (parentComparison < 0) {
                parent.Left = newNode;
            }
            else {
                parent.Right = newNode;
            }

            count++;
            FixInsertViolation(newNode);
        }

        public ServiceRequest Find(string category) {
            if (category == null) {
                throw new ArgumentNullException(nameof(category));
            }

            RedBlackNode current = root;
            
            while (current != null) {
                int comparison = string.Compare(category, current.Key, StringComparison.OrdinalIgnoreCase);
                
                if (comparison == 0) {
                    return current.Value;
                }
                
                current = comparison < 0 ? current.Left : current.Right;
            }

            return null;
        }

        public IReadOnlyList<ServiceRequest> InOrder() {
            List<ServiceRequest> items = new List<ServiceRequest>();
            InOrderTraversal(root, items);
            return items;
        }

        public IReadOnlyList<RedBlackNode> GetNodesWithColors() {
            List<RedBlackNode> nodes = new List<RedBlackNode>();
            CollectNodes(root, nodes);
            return nodes;
        }

        public void Clear() {
            root = null;
            count = 0;
        }

        private void InOrderTraversal(RedBlackNode node, List<ServiceRequest> items) {
            if (node == null) {
                return;
            }

            InOrderTraversal(node.Left, items);
            items.Add(node.Value);
            InOrderTraversal(node.Right, items);
        }

        private void CollectNodes(RedBlackNode node, List<RedBlackNode> nodes) {
            if (node == null) {
                return;
            }

            nodes.Add(node);
            CollectNodes(node.Left, nodes);
            CollectNodes(node.Right, nodes);
        }

        private void FixInsertViolation(RedBlackNode node) {
            while (node != root && node.Parent.IsRed()) {
                RedBlackNode parent = node.Parent;
                RedBlackNode grandParent = parent.Parent;

                if (grandParent == null) {
                    break;
                }

                if (parent == grandParent.Left) {
                    RedBlackNode uncle = grandParent.Right;

                    if (uncle != null && uncle.IsRed()) {
                        parent.Color = NodeColor.Black;
                        uncle.Color = NodeColor.Black;
                        grandParent.Color = NodeColor.Red;
                        node = grandParent;
                    }
                    else {
                        if (node == parent.Right) {
                            node = parent;
                            RotateLeft(node);
                            parent = node.Parent;
                        }

                        parent.Color = NodeColor.Black;
                        grandParent.Color = NodeColor.Red;
                        RotateRight(grandParent);
                    }
                }
                else {
                    RedBlackNode uncle = grandParent.Left;

                    if (uncle != null && uncle.IsRed()) {
                        parent.Color = NodeColor.Black;
                        uncle.Color = NodeColor.Black;
                        grandParent.Color = NodeColor.Red;
                        node = grandParent;
                    }
                    else {
                        if (node == parent.Left) {
                            node = parent;
                            RotateRight(node);
                            parent = node.Parent;
                        }

                        parent.Color = NodeColor.Black;
                        grandParent.Color = NodeColor.Red;
                        RotateLeft(grandParent);
                    }
                }
            }

            root.Color = NodeColor.Black;
        }

        private void RotateLeft(RedBlackNode node) {
            RedBlackNode rightChild = node.Right;
            node.Right = rightChild.Left;

            if (rightChild.Left != null) {
                rightChild.Left.Parent = node;
            }

            rightChild.Parent = node.Parent;

            if (node.Parent == null) {
                root = rightChild;
            }
            else if (node == node.Parent.Left) {
                node.Parent.Left = rightChild;
            }
            else {
                node.Parent.Right = rightChild;
            }

            rightChild.Left = node;
            node.Parent = rightChild;
        }

        private void RotateRight(RedBlackNode node) {
            RedBlackNode leftChild = node.Left;
            node.Left = leftChild.Right;

            if (leftChild.Right != null) {
                leftChild.Right.Parent = node;
            }

            leftChild.Parent = node.Parent;

            if (node.Parent == null) {
                root = leftChild;
            }
            else if (node == node.Parent.Right) {
                node.Parent.Right = leftChild;
            }
            else {
                node.Parent.Left = leftChild;
            }

            leftChild.Right = node;
            node.Parent = leftChild;
        }
    }
}
