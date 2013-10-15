// Type: Microsoft.Xna.Framework.IUpdateable
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

using System;

namespace Microsoft.Xna.Framework
{
  public interface IUpdateable
  {
    bool Enabled { get; }

    int UpdateOrder { get; }

    event EventHandler<EventArgs> EnabledChanged;

    event EventHandler<EventArgs> UpdateOrderChanged;

    void Update(GameTime gameTime);
  }
}
