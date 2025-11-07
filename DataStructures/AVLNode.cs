using System;

namespace Sidequest_municiple_app {
    public class AVLNode {
        public ServiceRequest Value { get; private set; }
        public string Key { get; private set; }
        public AVLNode Left { get; set; }
        public AVLNode Right { get; set; }
        public int Height { get; private set; }

        public AVLNode(ServiceRequest value) {
            if (value == null) {
                throw new ArgumentNullException(nameof(value));
            }

            Value = value;
            Key = value.UniqueID;
            Height = 1;
        }

        public void UpdateHeight() {
            int leftHeight = Left?.Height ?? 0;
            int rightHeight = Right?.Height ?? 0;
            Height = Math.Max(leftHeight, rightHeight) + 1;
        }

        public int GetBalance() {
            int leftHeight = Left?.Height ?? 0;
            int rightHeight = Right?.Height ?? 0;
            return leftHeight - rightHeight;
        }

        public void UpdateValue(ServiceRequest value) {
            if (value == null) {
                throw new ArgumentNullException(nameof(value));
            }

            Value = value;
            Key = value.UniqueID;
        }
    }
}
