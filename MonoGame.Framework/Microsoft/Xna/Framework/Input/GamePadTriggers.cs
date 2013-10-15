// Type: Microsoft.Xna.Framework.Input.GamePadTriggers
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

using Microsoft.Xna.Framework;

namespace Microsoft.Xna.Framework.Input
{
  public struct GamePadTriggers
  {
    private float left;
    private float right;

    public float Left
    {
      get
      {
        return this.left;
      }
      internal set
      {
        this.left = MathHelper.Clamp(value, 0.0f, 1f);
      }
    }

    public float Right
    {
      get
      {
        return this.right;
      }
      internal set
      {
        this.right = MathHelper.Clamp(value, 0.0f, 1f);
      }
    }

    public GamePadTriggers(float leftTrigger, float rightTrigger)
    {
      this = new GamePadTriggers();
      this.Left = leftTrigger;
      this.Right = rightTrigger;
    }
  }
}
