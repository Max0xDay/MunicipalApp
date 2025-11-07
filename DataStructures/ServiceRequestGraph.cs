using System;
using System.Collections.Generic;
using System.Linq;

namespace Sidequest_municiple_app
{
  public class ServiceRequestGraph
  {
    private readonly Dictionary<string, GraphNode> nodes;

    public ServiceRequestGraph()
    {
      nodes = new Dictionary<string, GraphNode>(StringComparer.OrdinalIgnoreCase);
    }

    public int Count => nodes.Count;

    public void Clear()
    {
      nodes.Clear();
    }

    public GraphNode AddOrUpdate(ServiceRequest request)
    {
      if (request == null)
      {
        throw new ArgumentNullException(nameof(request));
      }

      if (!nodes.TryGetValue(request.UniqueID, out GraphNode node))
      {
        node = new GraphNode(request);
        nodes[request.UniqueID] = node;
      }
      else
      {
        node.UpdateValue(request);
      }

      return node;
    }

    public bool AddRelationship(string firstKey, string secondKey, double weight = 1)
    {
      if (firstKey == null)
      {
        throw new ArgumentNullException(nameof(firstKey));
      }

      if (secondKey == null)
      {
        throw new ArgumentNullException(nameof(secondKey));
      }

      if (string.Equals(firstKey, secondKey, StringComparison.OrdinalIgnoreCase))
      {
        return false;
      }

      if (!nodes.TryGetValue(firstKey, out GraphNode firstNode) ||
          !nodes.TryGetValue(secondKey, out GraphNode secondNode))
      {
        return false;
      }

      bool connected = firstNode.Connect(secondNode, weight);
      secondNode.Connect(firstNode, weight);

      UpdateRelatedRequestList(firstNode.Value, secondNode.Value);
      UpdateRelatedRequestList(secondNode.Value, firstNode.Value);

      return connected;
    }

    public IReadOnlyList<ServiceRequest> GetRelatedRequests(string key)
    {
      if (key == null)
      {
        throw new ArgumentNullException(nameof(key));
      }

      if (!nodes.TryGetValue(key, out GraphNode node))
      {
        return Array.Empty<ServiceRequest>();
      }

      return node.Edges.Select(edge => edge.Target.Value).ToList();
    }

    public void BuildRelationships(IEnumerable<ServiceRequest> requests)
    {
      if (requests == null)
      {
        throw new ArgumentNullException(nameof(requests));
      }

      List<ServiceRequest> requestList = requests.ToList();
      foreach (ServiceRequest request in requestList)
      {
        AddOrUpdate(request);
        request.RelatedRequestIDs.Clear();
      }

      // #COMPLETION_DRIVE: Assuming requests sharing category and location should be linked for related tracking
      // #SUGGEST_VERIFY: Confirm the relationship criteria with module brief or lecturer before freezing graph edges
      var grouped = requestList
        .Where(r => !string.IsNullOrWhiteSpace(r.Category) && !string.IsNullOrWhiteSpace(r.Location))
        .GroupBy(
          r => new RelationshipKey(r.Category, r.Location),
          new RelationshipKeyComparer());

      foreach (IGrouping<RelationshipKey, ServiceRequest> group in grouped)
      {
        List<ServiceRequest> bucket = group.ToList();
        for (int i = 0; i < bucket.Count; i++)
        {
          for (int j = i + 1; j < bucket.Count; j++)
          {
            AddRelationship(bucket[i].UniqueID, bucket[j].UniqueID, 1);
          }
        }
      }

      foreach (ServiceRequest request in requestList)
      {
        if (request.RelatedRequestIDs.Count == 0 && request.RelatedRequestIDs is List<string> relatedList)
        {
          relatedList.TrimExcess();
        }
      }
    }

    private void UpdateRelatedRequestList(ServiceRequest source, ServiceRequest target)
    {
      if (!source.RelatedRequestIDs.Any(id =>
            string.Equals(id, target.UniqueID, StringComparison.OrdinalIgnoreCase)))
      {
        source.RelatedRequestIDs.Add(target.UniqueID);
      }
    }

    private readonly struct RelationshipKey
    {
      public RelationshipKey(string category, string location)
      {
        Category = category;
        Location = location;
      }

      public string Category { get; }
      public string Location { get; }
    }

    private sealed class RelationshipKeyComparer : IEqualityComparer<RelationshipKey>
    {
      public bool Equals(RelationshipKey x, RelationshipKey y)
      {
        return string.Equals(x.Category, y.Category, StringComparison.OrdinalIgnoreCase) &&
               string.Equals(x.Location, y.Location, StringComparison.OrdinalIgnoreCase);
      }

      public int GetHashCode(RelationshipKey obj)
      {
        int categoryHash = obj.Category?.ToLowerInvariant().GetHashCode() ?? 0;
        int locationHash = obj.Location?.ToLowerInvariant().GetHashCode() ?? 0;
        return categoryHash ^ (locationHash * 397);
      }
    }
  }
}
