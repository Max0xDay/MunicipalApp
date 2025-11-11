/*
========  GraphNode.cs  ========
Purpose: GraphNodes represent vertices in a graph, each wrapping a service request with its adjacency information.
Why its used: In the Sidequest municipal app, graph nodes enable relationship tracking between municipal incidents.

In detail:
This graph node lets us treat each service request as a vertex with a brain: it hoists the
ticket metadata, exposes a key for adjacency hashing, and curates outbound edges in a
dictionary tuned for fast lookups. The design deliberately favors write-through updates so the
node stays synchronized even when requests shift status or priority.

In practice the ServiceRequestGraph builds one of these for every issue pulled from SQLite,
then uses the adjacency map to capture neighborhood relationships. When the dashboard asks
for related requests or a traversal preview, the node hands back its edges without
recalculating, keeping the user interface responsive while still reflecting the latest operational
topology.
=============================
*/
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
