// Type: Common.SoundInfo
// Assembly: Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9992B00D-7E50-4755-8BAA-4E3BBC8F3470
// Assembly location: F:\Program Files (x86)\FEZ\Common.dll

using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Common
{
  public static class SoundInfo
  {
    [DllImport("winmm.dll")]
    private static uint mciSendString(string command, StringBuilder returnValue, int returnLength, IntPtr winHandle);

    public static int GetSoundLength(string fileName)
    {
      StringBuilder returnValue = new StringBuilder(32);
      int num1 = (int) SoundInfo.mciSendString(string.Format("open \"{0}\" type waveaudio alias wave", (object) fileName), (StringBuilder) null, 0, IntPtr.Zero);
      int num2 = (int) SoundInfo.mciSendString("status wave length", returnValue, returnValue.Capacity, IntPtr.Zero);
      int num3 = (int) SoundInfo.mciSendString("close wave", (StringBuilder) null, 0, IntPtr.Zero);
      int result;
      int.TryParse(((object) returnValue).ToString(), out result);
      return result;
    }
  }
}
