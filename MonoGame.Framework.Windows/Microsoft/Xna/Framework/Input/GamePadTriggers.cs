// Type: Microsoft.Xna.Framework.Input.GamePadTriggers
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

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
