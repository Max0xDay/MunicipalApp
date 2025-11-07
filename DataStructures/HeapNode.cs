using System;

namespace Sidequest_municiple_app
{
  public class HeapNode
  {
    public ServiceRequest Value { get; private set; }
    public int Priority { get; private set; }
    public DateTime EnqueuedAt { get; private set; }

    public HeapNode(ServiceRequest value, int priority)
    {
      if (value == null)
      {
        throw new ArgumentNullException(nameof(value));
      }

      Value = value;
      Priority = priority;
      EnqueuedAt = DateTime.UtcNow;
    }

    public void UpdatePriority(int priority)
    {
      Priority = priority;
    }

    public void UpdateValue(ServiceRequest value)
    {
      if (value == null)
      {
        throw new ArgumentNullException(nameof(value));
      }

      Value = value;
    }
  }
}
