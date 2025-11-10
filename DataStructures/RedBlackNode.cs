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
