// Type: Microsoft.Xna.Framework.Audio.AudioListener
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

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
