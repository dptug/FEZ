// Type: Microsoft.Xna.Framework.Input.Stick
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

using Microsoft.Xna.Framework;
using System;

namespace Microsoft.Xna.Framework.Input
{
  public class Stick
  {
    public Input Press { get; set; }

    public Axis X { get; set; }

    public Axis Y { get; set; }

    public Stick()
    {
      this.X = new Axis();
      this.Y = new Axis();
      this.Press = new Input();
    }

    internal Vector2 ReadAxisPair(IntPtr device)
    {
      return new Vector2(this.X.ReadAxis(device), -this.Y.ReadAxis(device));
    }
  }
}
