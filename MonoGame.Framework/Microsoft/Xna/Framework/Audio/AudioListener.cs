// Type: Microsoft.Xna.Framework.Audio.AudioListener
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

using Microsoft.Xna.Framework;

namespace Microsoft.Xna.Framework.Audio
{
  public class AudioListener
  {
    public Vector3 Forward { get; set; }

    public Vector3 Position { get; set; }

    public Vector3 Up { get; set; }

    public Vector3 Velocity { get; set; }

    public AudioListener()
    {
      this.Forward = Vector3.Forward;
      this.Position = Vector3.Zero;
      this.Up = Vector3.Up;
      this.Velocity = Vector3.Zero;
    }
  }
}
