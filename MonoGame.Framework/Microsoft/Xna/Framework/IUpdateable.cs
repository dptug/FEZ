﻿// Type: Microsoft.Xna.Framework.IUpdateable
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

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
