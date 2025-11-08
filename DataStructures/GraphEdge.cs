/*
Each graph edge is the connective tissue that lets us treat service requests as a network of
related incidents rather than isolated tickets. By capturing both endpoints plus a floating-point
weight we can encode nuanced similarities—shared categories, overlapping locations, or
follow-up relationships—without hard-coding the semantics into the UI.

The ServiceRequestStatus form leans on these edges when it draws traversal lists and minimum
spanning trees. During refreshes the graph builder computes weights and hands them to this
class, and later the traversal panel reads them back to explain why two requests are traveling
together or how strongly they influence the prioritization overlay.
*/
using System;

namespace Sidequest_municiple_app {
    public class GraphEdge {
        public GraphNode Source { get; }
        public GraphNode Target { get; }
        public double Weight { get; private set; }

        public GraphEdge(GraphNode source, GraphNode target, double weight) {
            if (source == null) {
                throw new ArgumentNullException(nameof(source));
            }

            if (target == null) {
                throw new ArgumentNullException(nameof(target));
            }

            Source = source;
            Target = target;
            Weight = weight;
        }

        public void UpdateWeight(double weight) {
            Weight = weight;
        }
    }
}
