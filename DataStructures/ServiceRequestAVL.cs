/*
========  ServiceRequestAVL.cs  ========
Purpose: ServiceRequestAVL maintains a self-balancing AVL tree for chronologically ordered service requests.
Why its used: In the Sidequest municipal app, AVL trees guarantee balanced searches and insertions for time-sorted data.

In detail:
The ServiceRequestAVL class is our balanced guardian for chronological ordering. It layers an
AVL tree over municipal tickets, using a custom comparer that sorts by submission time and then
by identifier, which keeps the structure gracefully ordered even when the control room floods it
with backdated adjustments.

The ServiceRequestStatus form relies on this class for consistent ordering whenever it filters or
validates records. Every refresh seeds the tree from the database; rotations happen
transparently under the hood, and later the statistics panel leans on the AVL validations to
confirm the algorithmic landscape is still healthy.
=============================
*/
using System;
using System.Collections.Generic;

namespace Sidequest_municiple_app {
    public class ServiceRequestAVL {
        private sealed class SubmissionComparer : IComparer<ServiceRequest> {
            public int Compare(ServiceRequest x, ServiceRequest y) {
                if (ReferenceEquals(x, y)) {
                    return 0;
                }

                if (x == null) {
                    return -1;
                }

                if (y == null) {
                    return 1;
                }

                int dateComparison = DateTime.Compare(x.DateSubmitted, y.DateSubmitted);
                if (dateComparison != 0) {
                    return dateComparison;
                }

                return string.Compare(x.UniqueID, y.UniqueID, StringComparison.OrdinalIgnoreCase);
            }
        }

        private AVLNode root;
        private int count;
        private readonly IComparer<ServiceRequest> comparer;

        public ServiceRequestAVL() {
            comparer = new SubmissionComparer();
        }

        public int Count => count;

        public void Insert(ServiceRequest request) {
            if (request == null) {
                throw new ArgumentNullException(nameof(request));
            }

            root = InsertNode(root, request);
        }

        public ServiceRequest FindById(string key) {
            if (key == null) {
                throw new ArgumentNullException(nameof(key));
            }

            Stack<AVLNode> stack = new Stack<AVLNode>();
            AVLNode current = root;

            while (stack.Count > 0 || current != null) {
                if (current != null) {
                    stack.Push(current);
                    current = current.Left;
                }
                else {
                    current = stack.Pop();
                    if (string.Equals(current.Key, key, StringComparison.OrdinalIgnoreCase)) {
                        return current.Value;
                    }

                    current = current.Right;
                }
            }

            return null;
        }

        public IReadOnlyList<ServiceRequest> InOrder() {
            List<ServiceRequest> items = new List<ServiceRequest>();
            Stack<AVLNode> stack = new Stack<AVLNode>();
            AVLNode current = root;

            while (stack.Count > 0 || current != null) {
                if (current != null) {
                    stack.Push(current);
                    current = current.Left;
                }
                else {
                    current = stack.Pop();
                    items.Add(current.Value);
                    current = current.Right;
                }
            }

            return items;
        }

        public void Clear() {
            root = null;
            count = 0;
        }

        private AVLNode InsertNode(AVLNode node, ServiceRequest request) {
            if (node == null) {
                count++;
                return new AVLNode(request);
            }

            int comparison = comparer.Compare(request, node.Value);
            if (comparison < 0) {
                node.Left = InsertNode(node.Left, request);
            }
            else if (comparison > 0) {
                node.Right = InsertNode(node.Right, request);
            }
            else {
                node.UpdateValue(request);
                return node;
            }

            node.UpdateHeight();
            int balance = node.GetBalance();

            if (balance > 1) {
                if (comparer.Compare(request, node.Left.Value) < 0) {
                    return RotateRight(node);
                }

                node.Left = RotateLeft(node.Left);
                return RotateRight(node);
            }

            if (balance < -1) {
                if (comparer.Compare(request, node.Right.Value) > 0) {
                    return RotateLeft(node);
                }

                node.Right = RotateRight(node.Right);
                return RotateLeft(node);
            }

            return node;
        }

        private AVLNode RotateLeft(AVLNode node) {
            AVLNode pivot = node.Right;
            AVLNode subtree = pivot.Left;
            pivot.Left = node;
            node.Right = subtree;
            node.UpdateHeight();
            pivot.UpdateHeight();
            return pivot;
        }

        private AVLNode RotateRight(AVLNode node) {
            AVLNode pivot = node.Left;
            AVLNode subtree = pivot.Right;
            pivot.Right = node;
            node.Left = subtree;
            node.UpdateHeight();
            pivot.UpdateHeight();
            return pivot;
        }
    }
}
