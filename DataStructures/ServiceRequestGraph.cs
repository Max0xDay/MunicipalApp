using System;
using System.Collections.Generic;
using System.Linq;

namespace Sidequest_municiple_app {
    public class ServiceRequestGraph {
        private readonly Dictionary<string, GraphNode> nodes;

        public ServiceRequestGraph() {
            nodes = new Dictionary<string, GraphNode>(StringComparer.OrdinalIgnoreCase);
        }

        public int Count => nodes.Count;

        public void Clear() {
            nodes.Clear();
        }

        public GraphNode AddOrUpdate(ServiceRequest request) {
            if (request == null) {
                throw new ArgumentNullException(nameof(request));
            }

            if (!nodes.TryGetValue(request.UniqueID, out GraphNode node)) {
                node = new GraphNode(request);
                nodes[request.UniqueID] = node;
            }
            else {
                node.UpdateValue(request);
            }

            return node;
        }

        public bool AddRelationship(string firstKey, string secondKey, double weight = 1) {
            if (firstKey == null) {
                throw new ArgumentNullException(nameof(firstKey));
            }

            if (secondKey == null) {
                throw new ArgumentNullException(nameof(secondKey));
            }

            if (string.Equals(firstKey, secondKey, StringComparison.OrdinalIgnoreCase)) {
                return false;
            }

            if (!nodes.TryGetValue(firstKey, out GraphNode firstNode) ||
                !nodes.TryGetValue(secondKey, out GraphNode secondNode)) {
                return false;
            }

            bool connected = firstNode.Connect(secondNode, weight);
            secondNode.Connect(firstNode, weight);

            UpdateRelatedRequestList(firstNode.Value, secondNode.Value);
            UpdateRelatedRequestList(secondNode.Value, firstNode.Value);

            return connected;
        }

        public IReadOnlyList<ServiceRequest> GetRelatedRequests(string key) {
            if (key == null) {
                throw new ArgumentNullException(nameof(key));
            }

            if (!nodes.TryGetValue(key, out GraphNode node)) {
                return Array.Empty<ServiceRequest>();
            }

            return node.Edges.Select(edge => edge.Target.Value).ToList();
        }

        public IReadOnlyList<ServiceRequest> BreadthFirst(string startKey, int maximumDepth = int.MaxValue) {
            if (startKey == null) {
                throw new ArgumentNullException(nameof(startKey));
            }

            if (!nodes.TryGetValue(startKey, out GraphNode startNode)) {
                return Array.Empty<ServiceRequest>();
            }

            List<ServiceRequest> traversal = new List<ServiceRequest>();
            HashSet<string> visited = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            Queue<KeyValuePair<GraphNode, int>> queue = new Queue<KeyValuePair<GraphNode, int>>();

            visited.Add(startNode.Key);
            queue.Enqueue(new KeyValuePair<GraphNode, int>(startNode, 0));

            while (queue.Count > 0) {
                KeyValuePair<GraphNode, int> item = queue.Dequeue();
                traversal.Add(item.Key.Value);

                if (item.Value >= maximumDepth) {
                    continue;
                }

                foreach (GraphEdge edge in item.Key.Edges) {
                    if (visited.Add(edge.Target.Key)) {
                        queue.Enqueue(new KeyValuePair<GraphNode, int>(edge.Target, item.Value + 1));
                    }
                }
            }

            return traversal;
        }

        public IReadOnlyList<ServiceRequest> DepthFirst(string startKey) {
            if (startKey == null) {
                throw new ArgumentNullException(nameof(startKey));
            }

            if (!nodes.TryGetValue(startKey, out GraphNode startNode)) {
                return Array.Empty<ServiceRequest>();
            }

            List<ServiceRequest> traversal = new List<ServiceRequest>();
            HashSet<string> visited = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            Stack<GraphNode> stack = new Stack<GraphNode>();

            stack.Push(startNode);

            while (stack.Count > 0) {
                GraphNode current = stack.Pop();
                if (!visited.Add(current.Key)) {
                    continue;
                }

                traversal.Add(current.Value);

                List<GraphEdge> neighbors = current.Edges
                    .OrderByDescending(edge => edge.Target.Key, StringComparer.OrdinalIgnoreCase)
                    .ToList();
                foreach (GraphEdge edge in neighbors) {
                    if (!visited.Contains(edge.Target.Key)) {
                        stack.Push(edge.Target);
                    }
                }
            }

            return traversal;
        }

        public void BuildRelationships(IEnumerable<ServiceRequest> requests) {
            if (requests == null) {
                throw new ArgumentNullException(nameof(requests));
            }

            List<ServiceRequest> requestList = requests.ToList();
            foreach (ServiceRequest request in requestList) {
                AddOrUpdate(request);
                request.RelatedRequestIDs.Clear();
            }

            var grouped = requestList
                .Where(r => !string.IsNullOrWhiteSpace(r.Category) && !string.IsNullOrWhiteSpace(r.Location))
                .GroupBy(
                    r => new RelationshipKey(r.Category, r.Location),
                    new RelationshipKeyComparer());

            foreach (IGrouping<RelationshipKey, ServiceRequest> group in grouped) {
                List<ServiceRequest> bucket = group.ToList();
                for (int i = 0; i < bucket.Count; i++) {
                    for (int j = i + 1; j < bucket.Count; j++) {
                        AddRelationship(bucket[i].UniqueID, bucket[j].UniqueID, 1);
                    }
                }
            }

            foreach (ServiceRequest request in requestList) {
                if (request.RelatedRequestIDs.Count == 0 && request.RelatedRequestIDs is List<string> relatedList) {
                    relatedList.TrimExcess();
                }
            }
        }

        public IReadOnlyList<GraphEdge> MinimumSpanningTree(string startKey) {
            if (startKey == null) {
                throw new ArgumentNullException(nameof(startKey));
            }

            if (!nodes.TryGetValue(startKey, out GraphNode startNode)) {
                return Array.Empty<GraphEdge>();
            }

            List<GraphEdge> treeEdges = new List<GraphEdge>();
            HashSet<string> visited = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            List<GraphEdge> candidates = new List<GraphEdge>();

            visited.Add(startNode.Key);
            candidates.AddRange(startNode.Edges);

            while (candidates.Count > 0) {
                GraphEdge edge = ExtractMinimumEdge(candidates);
                if (edge == null) {
                    break;
                }

                if (visited.Contains(edge.Target.Key)) {
                    continue;
                }

                treeEdges.Add(edge);
                visited.Add(edge.Target.Key);

                foreach (GraphEdge neighbor in edge.Target.Edges) {
                    if (!visited.Contains(neighbor.Target.Key)) {
                        candidates.Add(neighbor);
                    }
                }
            }

            return treeEdges;
        }

        private void UpdateRelatedRequestList(ServiceRequest source, ServiceRequest target) {
            if (!source.RelatedRequestIDs.Any(id =>
                    string.Equals(id, target.UniqueID, StringComparison.OrdinalIgnoreCase))) {
                source.RelatedRequestIDs.Add(target.UniqueID);
            }
        }

        private readonly struct RelationshipKey {
            public RelationshipKey(string category, string location) {
                Category = category;
                Location = location;
            }

            public string Category { get; }
            public string Location { get; }
        }

        private sealed class RelationshipKeyComparer : IEqualityComparer<RelationshipKey> {
            public bool Equals(RelationshipKey x, RelationshipKey y) {
                return string.Equals(x.Category, y.Category, StringComparison.OrdinalIgnoreCase) &&
                    string.Equals(x.Location, y.Location, StringComparison.OrdinalIgnoreCase);
            }

            public int GetHashCode(RelationshipKey obj) {
                int categoryHash = obj.Category?.ToLowerInvariant().GetHashCode() ?? 0;
                int locationHash = obj.Location?.ToLowerInvariant().GetHashCode() ?? 0;
                return categoryHash ^ (locationHash * 397);
            }
        }

        private GraphEdge ExtractMinimumEdge(List<GraphEdge> edges) {
            if (edges.Count == 0) {
                return null;
            }

            int index = 0;
            double weight = edges[0].Weight;

            for (int i = 1; i < edges.Count; i++) {
                double candidateWeight = edges[i].Weight;
                int comparison = candidateWeight.CompareTo(weight);
                if (comparison < 0) {
                    index = i;
                    weight = candidateWeight;
                    continue;
                }

                if (comparison == 0) {
                    string currentKey = edges[index].Target.Key;
                    string candidateKey = edges[i].Target.Key;
                    if (string.Compare(candidateKey, currentKey, StringComparison.OrdinalIgnoreCase) < 0) {
                        index = i;
                        weight = candidateWeight;
                    }
                }
            }

            GraphEdge selected = edges[index];
            edges.RemoveAt(index);
            return selected;
        }
    }
}
