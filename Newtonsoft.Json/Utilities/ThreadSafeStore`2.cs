// Type: Newtonsoft.Json.Utilities.ThreadSafeStore`2
// Assembly: Newtonsoft.Json, Version=4.5.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: DB013E7B-84E7-4AE6-A132-56DC73318C48
// Assembly location: F:\Program Files (x86)\FEZ\Newtonsoft.Json.dll

using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;

namespace Newtonsoft.Json.Utilities
{
  internal class ThreadSafeStore<TKey, TValue>
  {
    private readonly object _lock = new object();
    private Dictionary<TKey, TValue> _store;
    private readonly Func<TKey, TValue> _creator;

    public ThreadSafeStore(Func<TKey, TValue> creator)
    {
      if (creator == null)
        throw new ArgumentNullException("creator");
      this._creator = creator;
    }

    public TValue Get(TKey key)
    {
      TValue obj;
      if (this._store == null || !this._store.TryGetValue(key, out obj))
        return this.AddValue(key);
      else
        return obj;
    }

    private TValue AddValue(TKey key)
    {
      TValue obj = this._creator(key);
      lock (this._lock)
      {
        if (this._store == null)
        {
          this._store = new Dictionary<TKey, TValue>();
          this._store[key] = obj;
        }
        else
        {
          TValue local_1;
          if (this._store.TryGetValue(key, out local_1))
            return local_1;
          Dictionary<TKey, TValue> local_2 = new Dictionary<TKey, TValue>((IDictionary<TKey, TValue>) this._store);
          local_2[key] = obj;
          this._store = local_2;
        }
        return obj;
      }
    }
  }
}
