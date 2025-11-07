using System;
using System.Collections.Generic;

namespace Sidequest_municiple_app {
    public class GraphNode {
        private readonly Dictionary<string, GraphEdge> adjacency;

        public ServiceRequest Value { get; private set; }
        public string Key { get; private set; }

        public GraphNode(ServiceRequest value) {
            if (value == null) {
                throw new ArgumentNullException(nameof(value));
            }

            Value = value;
            Key = value.UniqueID;
            adjacency = new Dictionary<string, GraphEdge>(StringComparer.OrdinalIgnoreCase);
        }

        public IReadOnlyCollection<GraphEdge> Edges => adjacency.Values;

        public bool Connect(GraphNode target, double weight) {
            if (target == null) {
                throw new ArgumentNullException(nameof(target));
            }

            GraphEdge edge = new GraphEdge(this, target, weight);
            adjacency[target.Key] = edge;
            return true;
        }

        public bool Disconnect(string key) {
            if (key == null) {
                throw new ArgumentNullException(nameof(key));
            }

            return adjacency.Remove(key);
        }

        public bool HasEdge(string key) {
            if (key == null) {
                throw new ArgumentNullException(nameof(key));
            }

            return adjacency.ContainsKey(key);
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
