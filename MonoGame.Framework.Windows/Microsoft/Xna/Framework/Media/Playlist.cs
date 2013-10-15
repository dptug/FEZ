// Type: Microsoft.Xna.Framework.Media.Playlist
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

using System;

namespace Microsoft.Xna.Framework.Media
{
  public sealed class Playlist : IDisposable
  {
    public TimeSpan Duration { get; internal set; }

    public string Name { get; internal set; }

    public void Dispose()
    {
    }
  }
}
