// Type: FezEngine.Services.DebuggingBag
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FezEngine.Services
{
  public class DebuggingBag : IDebuggingBag
  {
    private static readonly TimeSpan ExpirationTime = new TimeSpan(0, 0, 5);
    private readonly Dictionary<string, DebuggingBag.DebuggingLine> items;

    public object this[string name]
    {
      get
      {
        if (this.items.ContainsKey(name))
          return this.items[name].Value;
        else
          return (object) null;
      }
    }

    public IEnumerable<string> Keys
    {
      get
      {
        return (IEnumerable<string>) Enumerable.OrderBy<string, string>((IEnumerable<string>) this.items.Keys, (Func<string, string>) (x => x));
      }
    }

    static DebuggingBag()
    {
    }

    public DebuggingBag()
    {
      this.items = new Dictionary<string, DebuggingBag.DebuggingLine>();
    }

    public void Add(string name, object item)
    {
    }

    public void Empty()
    {
      List<string> list = new List<string>();
      foreach (string index in this.items.Keys)
      {
        if (this.items[index].Expired)
          list.Add(index);
      }
      foreach (string key in list)
        this.items.Remove(key);
    }

    public float GetAge(string name)
    {
      if (this.items.ContainsKey(name))
        return this.items[name].Age;
      else
        return 0.0f;
    }

    private class DebuggingLine
    {
      private readonly object value;
      private DateTime lastUpdateTime;

      public object Value
      {
        get
        {
          return this.value;
        }
      }

      public bool Expired
      {
        get
        {
          return DateTime.Now - this.lastUpdateTime >= DebuggingBag.ExpirationTime;
        }
      }

      public float Age
      {
        get
        {
          return MathHelper.Clamp((float) (DateTime.Now - this.lastUpdateTime).Ticks / (float) DebuggingBag.ExpirationTime.Ticks, 0.0f, 1f);
        }
      }

      public DebuggingLine(object value)
      {
        this.value = value;
        this.lastUpdateTime = DateTime.Now;
      }

      public void Refresh()
      {
        this.lastUpdateTime = DateTime.Now;
      }
    }
  }
}
