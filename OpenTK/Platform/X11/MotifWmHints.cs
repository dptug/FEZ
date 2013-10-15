// Type: OpenTK.Platform.X11.MotifWmHints
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using System;

namespace OpenTK.Platform.X11
{
  internal struct MotifWmHints
  {
    internal IntPtr flags;
    internal IntPtr functions;
    internal IntPtr decorations;
    internal IntPtr input_mode;
    internal IntPtr status;

    public override string ToString()
    {
      return string.Format("MotifWmHints <flags={0}, functions={1}, decorations={2}, input_mode={3}, status={4}", (object) (MotifFlags) this.flags.ToInt32(), (object) (MotifFunctions) this.functions.ToInt32(), (object) (MotifDecorations) this.decorations.ToInt32(), (object) (MotifInputMode) this.input_mode.ToInt32(), (object) this.status.ToInt32());
    }
  }
}
