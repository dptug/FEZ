// Type: Microsoft.Xna.Framework.Input.Axis
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

using System;

namespace Microsoft.Xna.Framework.Input
{
  public class Axis
  {
    public Input Negative { get; set; }

    public Input Positive { get; set; }

    public InputType Type { get; set; }

    public Axis()
    {
      this.Negative = new Input();
      this.Positive = new Input();
    }

    public float ReadAxis(IntPtr device)
    {
      return this.Positive.ReadFloat(device) - this.Negative.ReadFloat(device);
    }

    internal void AssignAxis(int id, bool negative)
    {
      this.Negative.ID = id;
      this.Negative.Negative = !negative;
      this.Negative.Type = InputType.Axis;
      this.Positive.ID = id;
      this.Positive.Negative = negative;
      this.Positive.Type = InputType.Axis;
    }
  }
}
