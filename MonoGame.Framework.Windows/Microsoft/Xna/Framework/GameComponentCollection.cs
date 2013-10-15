// Type: Microsoft.Xna.Framework.GameComponentCollection
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

using System;
using System.Collections.ObjectModel;

namespace Microsoft.Xna.Framework
{
  public sealed class GameComponentCollection : Collection<IGameComponent>
  {
    public event EventHandler<GameComponentCollectionEventArgs> ComponentAdded;

    public event EventHandler<GameComponentCollectionEventArgs> ComponentRemoved;

    protected override void ClearItems()
    {
      for (int index = 0; index < this.Count; ++index)
        this.OnComponentRemoved(new GameComponentCollectionEventArgs(this[index]));
      base.ClearItems();
    }

    protected override void InsertItem(int index, IGameComponent item)
    {
      if (this.IndexOf(item) != -1)
        throw new ArgumentException("Cannot Add Same Component Multiple Times");
      base.InsertItem(index, item);
      if (item == null)
        return;
      this.OnComponentAdded(new GameComponentCollectionEventArgs(item));
    }

    private void OnComponentAdded(GameComponentCollectionEventArgs eventArgs)
    {
      if (this.ComponentAdded == null)
        return;
      this.ComponentAdded((object) this, eventArgs);
    }

    private void OnComponentRemoved(GameComponentCollectionEventArgs eventArgs)
    {
      if (this.ComponentRemoved == null)
        return;
      this.ComponentRemoved((object) this, eventArgs);
    }

    protected override void RemoveItem(int index)
    {
      IGameComponent gameComponent = this[index];
      base.RemoveItem(index);
      if (gameComponent == null)
        return;
      this.OnComponentRemoved(new GameComponentCollectionEventArgs(gameComponent));
    }

    protected override void SetItem(int index, IGameComponent item)
    {
      throw new NotSupportedException();
    }
  }
}
