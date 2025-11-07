using System;
using System.Collections.Generic;

namespace Sidequest_municiple_app
{
  public class ServiceRequestBST
  {
    private BSTNode root;
    private int count;

    public int Count => count;

    public void Insert(ServiceRequest request)
    {
      if (request == null)
      {
        throw new ArgumentNullException(nameof(request));
      }

      root = InsertNode(root, request);
    }

    public ServiceRequest Find(string key)
    {
      if (key == null)
      {
        throw new ArgumentNullException(nameof(key));
      }

      BSTNode current = root;
      while (current != null)
      {
        int comparison = string.Compare(key, current.Key, StringComparison.OrdinalIgnoreCase);
        if (comparison == 0)
        {
          return current.Value;
        }

        current = comparison < 0 ? current.Left : current.Right;
      }

      return null;
    }

    public bool Delete(string key)
    {
      if (key == null)
      {
        throw new ArgumentNullException(nameof(key));
      }

      int previousCount = count;
      root = DeleteNode(root, key);
      return count < previousCount;
    }

    public IReadOnlyList<ServiceRequest> InOrder()
    {
      List<ServiceRequest> items = new List<ServiceRequest>();
      Stack<BSTNode> stack = new Stack<BSTNode>();
      BSTNode current = root;

      while (stack.Count > 0 || current != null)
      {
        if (current != null)
        {
          stack.Push(current);
          current = current.Left;
        }
        else
        {
          current = stack.Pop();
          items.Add(current.Value);
          current = current.Right;
        }
      }

      return items;
    }

    public void Clear()
    {
      root = null;
      count = 0;
    }

    private BSTNode InsertNode(BSTNode node, ServiceRequest request)
    {
      if (node == null)
      {
        count++;
        return new BSTNode(request);
      }

      int comparison = string.Compare(request.UniqueID, node.Key, StringComparison.OrdinalIgnoreCase);
      if (comparison == 0)
      {
        node.UpdateValue(request);
        return node;
      }

      if (comparison < 0)
      {
        node.Left = InsertNode(node.Left, request);
        return node;
      }

      node.Right = InsertNode(node.Right, request);
      return node;
    }

    private BSTNode DeleteNode(BSTNode node, string key)
    {
      if (node == null)
      {
        return null;
      }

      int comparison = string.Compare(key, node.Key, StringComparison.OrdinalIgnoreCase);
      if (comparison < 0)
      {
        node.Left = DeleteNode(node.Left, key);
        return node;
      }

      if (comparison > 0)
      {
        node.Right = DeleteNode(node.Right, key);
        return node;
      }

      if (node.Left == null && node.Right == null)
      {
        count--;
        return null;
      }

      if (node.Left == null)
      {
        count--;
        return node.Right;
      }

      if (node.Right == null)
      {
        count--;
        return node.Left;
      }

      BSTNode successor = FindMin(node.Right);
      node.UpdateValue(successor.Value);
      node.Right = DeleteNode(node.Right, successor.Key);
      return node;
    }

    private BSTNode FindMin(BSTNode node)
    {
      BSTNode current = node;
      while (current.Left != null)
      {
        current = current.Left;
      }

      return current;
    }
  }
}
