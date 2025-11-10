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
