// Type: Microsoft.Xna.Framework.Input.DPad
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

namespace Microsoft.Xna.Framework.Input
{
  public class DPad
  {
    public Input Up { get; set; }

    public Input Down { get; set; }

    public Input Left { get; set; }

    public Input Right { get; set; }

    public DPad()
    {
      this.Up = new Input();
      this.Down = new Input();
      this.Left = new Input();
      this.Right = new Input();
    }

    internal void AssignPovHat(int id)
    {
      this.Up.ID = id;
      this.Up.Negative = false;
      this.Up.Type = InputType.PovUp;
      this.Down.ID = id;
      this.Down.Negative = false;
      this.Down.Type = InputType.PovDown;
      this.Left.ID = id;
      this.Left.Negative = false;
      this.Left.Type = InputType.PovLeft;
      this.Right.ID = id;
      this.Right.Negative = false;
      this.Right.Type = InputType.PovRight;
    }
  }
}
