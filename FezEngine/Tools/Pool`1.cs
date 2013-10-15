// Type: FezEngine.Tools.Pool`1
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using System;
using System.Collections.Generic;

namespace FezEngine.Tools
{
  public class Pool<T> where T : class, new()
  {
    private readonly Stack<T> stack;
    private int size;

    public int Size
    {
      get
      {
        return this.size;
      }
      set
      {
        int num = value - this.size;
        if (num > 0)
        {
          for (int index = 0; index < num; ++index)
            this.stack.Push(Activator.CreateInstance<T>());
        }
        this.size = value;
      }
    }

    public int Available
    {
      get
      {
        return this.stack.Count;
      }
    }

    public Pool()
      : this(0)
    {
    }

    public Pool(int size)
    {
      this.stack = new Stack<T>(size);
      this.Size = size;
    }

    public T Take()
    {
      if (this.stack.Count <= 0)
        return Activator.CreateInstance<T>();
      else
        return this.stack.Pop();
    }

    public void Return(T item)
    {
      this.stack.Push(item);
    }
  }
}
