// Type: Microsoft.Xna.Framework.GamerServices.GamerCollection`1
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Microsoft.Xna.Framework.GamerServices
{
  public class GamerCollection<T> : ReadOnlyCollection<T>, IEnumerable<T>, IEnumerable where T : Gamer
  {
    internal GamerCollection(List<T> list)
      : base((IList<T>) list)
    {
    }

    internal GamerCollection()
      : base((IList<T>) new List<T>())
    {
    }

    internal void AddGamer(T item)
    {
      if (this.Items.Count > 0)
      {
        for (int index = 0; index < Enumerable.Count<T>((IEnumerable<T>) this.Items); ++index)
        {
          if (item.Gamertag.CompareTo(this.Items[index].Gamertag) > 0)
          {
            this.Items.Insert(index, item);
            return;
          }
        }
      }
      this.Items.Add(item);
    }

    internal void RemoveGamer(T item)
    {
      this.Items.Remove(item);
    }

    internal void RemoveGamerAt(int item)
    {
      this.Items.RemoveAt(item);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return (IEnumerator) this.GetEnumerator();
    }
  }
}
