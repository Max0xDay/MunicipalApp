using System;

namespace Sidequest_municiple_app
{
  public class GraphEdge
  {
    public GraphNode Source { get; }
    public GraphNode Target { get; }
    public double Weight { get; private set; }

    public GraphEdge(GraphNode source, GraphNode target, double weight)
    {
      if (source == null)
      {
        throw new ArgumentNullException(nameof(source));
      }

      if (target == null)
      {
        throw new ArgumentNullException(nameof(target));
      }

      Source = source;
      Target = target;
      Weight = weight;
    }

    public void UpdateWeight(double weight)
    {
      Weight = weight;
    }
  }
}
