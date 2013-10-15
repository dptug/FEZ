// Type: SharpDX.Collections.ObservableCollection`1
// Assembly: SharpDX, Version=2.4.2.0, Culture=neutral, PublicKeyToken=627a3d6d1956f55a
// MVID: 578390A1-1524-4146-8C27-2E9750400D7A
// Assembly location: F:\Program Files (x86)\FEZ\SharpDX.dll

using System;
using System.Collections.ObjectModel;

namespace SharpDX.Collections
{
  public class ObservableCollection<T> : Collection<T>
  {
    public event EventHandler<ObservableCollectionEventArgs<T>> ItemAdded;

    public event EventHandler<ObservableCollectionEventArgs<T>> ItemRemoved;

    protected override void ClearItems()
    {
      for (int index = 0; index < this.Count; ++index)
        this.OnComponentRemoved(new ObservableCollectionEventArgs<T>(this[index]));
      base.ClearItems();
    }

    protected override void InsertItem(int index, T item)
    {
      if (this.Contains(item))
        throw new ArgumentException("This item is already added");
      base.InsertItem(index, item);
      if ((object) item == null)
        return;
      this.OnComponentAdded(new ObservableCollectionEventArgs<T>(item));
    }

    protected override void RemoveItem(int index)
    {
      T obj = this[index];
      base.RemoveItem(index);
      if ((object) obj == null)
        return;
      this.OnComponentRemoved(new ObservableCollectionEventArgs<T>(obj));
    }

    protected override void SetItem(int index, T item)
    {
      throw new NotSupportedException("Cannot set item into this instance");
    }

    private void OnComponentAdded(ObservableCollectionEventArgs<T> e)
    {
      EventHandler<ObservableCollectionEventArgs<T>> eventHandler = this.ItemAdded;
      if (eventHandler == null)
        return;
      eventHandler((object) this, e);
    }

    private void OnComponentRemoved(ObservableCollectionEventArgs<T> e)
    {
      EventHandler<ObservableCollectionEventArgs<T>> eventHandler = this.ItemRemoved;
      if (eventHandler == null)
        return;
      eventHandler((object) this, e);
    }
  }
}
