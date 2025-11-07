using System;

namespace Sidequest_municiple_app
{
  public class BSTNode
  {
    public ServiceRequest Value { get; private set; }
    public string Key { get; private set; }
    public BSTNode Left { get; set; }
    public BSTNode Right { get; set; }

    public BSTNode(ServiceRequest value)
    {
      if (value == null)
      {
        throw new ArgumentNullException(nameof(value));
      }

      Value = value;
      Key = value.UniqueID;
    }

    public int CompareTo(string key)
    {
      if (key == null)
      {
        throw new ArgumentNullException(nameof(key));
      }

      return string.Compare(Key, key, StringComparison.OrdinalIgnoreCase);
    }

    public void UpdateValue(ServiceRequest value)
    {
      if (value == null)
      {
        throw new ArgumentNullException(nameof(value));
      }

      Value = value;
      Key = value.UniqueID;
    }
  }
}
