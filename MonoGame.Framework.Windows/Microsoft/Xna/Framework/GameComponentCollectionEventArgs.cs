// Type: Microsoft.Xna.Framework.GameComponentCollectionEventArgs
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

using System;

namespace Microsoft.Xna.Framework
{
  public class GameComponentCollectionEventArgs : EventArgs
  {
    private IGameComponent _gameComponent;

    public IGameComponent GameComponent
    {
      get
      {
        return this._gameComponent;
      }
    }

    public GameComponentCollectionEventArgs(IGameComponent gameComponent)
    {
      this._gameComponent = gameComponent;
    }
  }
}
