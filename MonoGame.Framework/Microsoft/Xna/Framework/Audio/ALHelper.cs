// Type: Microsoft.Xna.Framework.Audio.ALHelper
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

using OpenTK.Audio.OpenAL;
using System;
using System.Diagnostics;
using System.IO;

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

    [Conditional("DEBUG")]
    public static void Check()
    {
      ALError error;
      if ((error = AL.GetError()) == ALError.NoError)
        return;
      Console.WriteLine("AL Error : " + AL.GetErrorString(error));
    }

    public static void Log(string message, string module = "OpenAL")
    {
      try
      {
        Console.WriteLine("({0}) [{1}] {2}", (object) DateTime.Now.ToString("HH:mm:ss.fff"), (object) module, (object) message);
        using (FileStream fileStream = File.Open(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\FEZ\\Debug Log.txt", FileMode.Append))
        {
          using (StreamWriter streamWriter = new StreamWriter((Stream) fileStream))
            streamWriter.WriteLine("({0}) [{1}] {2}", (object) DateTime.Now.ToString("HH:mm:ss.fff"), (object) module, (object) message);
        }
      }
      catch (Exception ex)
      {
      }
    }
  }
}
