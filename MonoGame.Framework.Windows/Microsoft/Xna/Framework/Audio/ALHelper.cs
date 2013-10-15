// Type: Microsoft.Xna.Framework.Audio.ALHelper
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

using OpenTK.Audio.OpenAL;
using System;

namespace Microsoft.Xna.Framework.Audio
{
  internal static class ALHelper
  {
    public static readonly XRamExtension XRam = new XRamExtension();
    public static readonly EffectsExtension Efx = new EffectsExtension();

    static ALHelper()
    {
    }

    public static bool TryCheck()
    {
      return AL.GetError() != ALError.NoError;
    }

    public static void Check()
    {
      ALError error;
      if ((error = AL.GetError()) == ALError.NoError)
        return;
      Console.WriteLine("AL Error : " + AL.GetErrorString(error));
    }
  }
}
