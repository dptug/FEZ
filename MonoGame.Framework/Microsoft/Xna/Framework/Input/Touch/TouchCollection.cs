// Type: Microsoft.Xna.Framework.Input.Touch.TouchCollection
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Xna.Framework.Input.Touch
{
  public struct TouchCollection : IList<TouchLocation>, ICollection<TouchLocation>, IEnumerable<TouchLocation>, IEnumerable
  {
    private TouchLocation[] _collection;
    private bool _isConnected;

    public bool IsConnected
    {
      get
      {
        return this._isConnected;
      }
    }

    public bool IsReadOnly
    {
      get
      {
        return true;
      }
    }

    public TouchLocation this[int index]
    {
      get
      {
        return this._collection[index];
      }
      set
      {
        throw new NotSupportedException();
      }
    }

    public int Count
    {
      get
      {
        return this._collection.Length;
      }
    }

    public TouchCollection(TouchLocation[] touches)
    {
      this._isConnected = true;
      this._collection = touches;
    }

    public bool FindById(int id, out TouchLocation touchLocation)
    {
      for (int index = 0; index < this._collection.Length; ++index)
      {
        TouchLocation touchLocation1 = this._collection[index];
        if (touchLocation1.Id == id)
        {
          touchLocation = touchLocation1;
          return true;
        }
      }
      touchLocation = new TouchLocation();
      return false;
    }

    public int IndexOf(TouchLocation item)
    {
      for (int index = 0; index < this._collection.Length; ++index)
      {
        if (item == this._collection[index])
          return index;
      }
      return -1;
    }

    public void Insert(int index, TouchLocation item)
    {
      throw new NotSupportedException();
    }

    public void RemoveAt(int index)
    {
      throw new NotSupportedException();
    }

    public void Add(TouchLocation item)
    {
      throw new NotSupportedException();
    }

    public void Clear()
    {
      throw new NotSupportedException();
    }

    public bool Contains(TouchLocation item)
    {
      for (int index = 0; index < this._collection.Length; ++index)
      {
        if (item == this._collection[index])
          return true;
      }
      return false;
    }

    public void CopyTo(TouchLocation[] array, int arrayIndex)
    {
      this._collection.CopyTo((Array) array, arrayIndex);
    }

    public bool Remove(TouchLocation item)
    {
      throw new NotSupportedException();
    }

    public IEnumerator<TouchLocation> GetEnumerator()
    {
      return Enumerable.AsEnumerable<TouchLocation>((IEnumerable<TouchLocation>) this._collection).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return this._collection.GetEnumerator();
    }
  }
}
