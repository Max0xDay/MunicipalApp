using System;
using System.Collections.Generic;

namespace Sidequest_municiple_app
{
  public class ServiceRequestHeap
  {
    private readonly List<HeapNode> storage;

    public ServiceRequestHeap()
    {
      storage = new List<HeapNode>();
    }

    public int Count => storage.Count;

    public void Insert(ServiceRequest request)
    {
      if (request == null)
      {
        throw new ArgumentNullException(nameof(request));
      }

      HeapNode node = new HeapNode(request, (int)request.Priority);
      storage.Add(node);
      HeapifyUp(storage.Count - 1);
    }

    public ServiceRequest Peek()
    {
      if (storage.Count == 0)
      {
        throw new InvalidOperationException("Heap is empty");
      }

      return storage[0].Value;
    }

    public ServiceRequest Extract()
    {
      if (storage.Count == 0)
      {
        throw new InvalidOperationException("Heap is empty");
      }

      ServiceRequest value = storage[0].Value;
      HeapNode last = storage[storage.Count - 1];
      storage.RemoveAt(storage.Count - 1);

      if (storage.Count > 0)
      {
        storage[0] = last;
        HeapifyDown(0);
      }

      return value;
    }

    public void Clear()
    {
      storage.Clear();
    }

    public IReadOnlyList<ServiceRequest> GetOrderedSnapshot()
    {
      List<HeapNode> copy = new List<HeapNode>(storage);
      copy.Sort((a, b) =>
      {
        int priorityComparison = b.Priority.CompareTo(a.Priority);
        if (priorityComparison != 0)
        {
          return priorityComparison;
        }

        return a.EnqueuedAt.CompareTo(b.EnqueuedAt);
      });

      List<ServiceRequest> ordered = new List<ServiceRequest>(copy.Count);
      foreach (HeapNode node in copy)
      {
        ordered.Add(node.Value);
      }

      return ordered;
    }

    private void HeapifyUp(int index)
    {
      while (index > 0)
      {
        int parentIndex = (index - 1) / 2;
        if (IsHigherPriority(storage[index], storage[parentIndex]))
        {
          Swap(index, parentIndex);
          index = parentIndex;
        }
        else
        {
          break;
        }
      }
    }

    private void HeapifyDown(int index)
    {
      int lastIndex = storage.Count - 1;
      while (true)
      {
        int leftIndex = index * 2 + 1;
        int rightIndex = index * 2 + 2;
        int highestIndex = index;

        if (leftIndex <= lastIndex && IsHigherPriority(storage[leftIndex], storage[highestIndex]))
        {
          highestIndex = leftIndex;
        }

        if (rightIndex <= lastIndex && IsHigherPriority(storage[rightIndex], storage[highestIndex]))
        {
          highestIndex = rightIndex;
        }

        if (highestIndex == index)
        {
          break;
        }

        Swap(index, highestIndex);
        index = highestIndex;
      }
    }

    private bool IsHigherPriority(HeapNode first, HeapNode second)
    {
      int priorityComparison = first.Priority.CompareTo(second.Priority);
      if (priorityComparison != 0)
      {
        return priorityComparison > 0;
      }

      return first.EnqueuedAt < second.EnqueuedAt;
    }

    private void Swap(int firstIndex, int secondIndex)
    {
      HeapNode temp = storage[firstIndex];
      storage[firstIndex] = storage[secondIndex];
      storage[secondIndex] = temp;
    }
  }
}
