// Type: OpenTK.Platform.MacOS.Carbon.SpeechChannel
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using System;
using System.Runtime.InteropServices;

namespace OpenTK.Platform.MacOS.Carbon
{
  internal class SpeechChannel
  {
    protected const string appServicesPath = "/System/Library/Frameworks/ApplicationServices.framework/Versions/Current/ApplicationServices";
    private IntPtr _id;

    public unsafe SpeechChannel()
    {
      int num = (int) SpeechChannel.NewSpeechChannel((IntPtr) ((void*) null), ref this._id);
    }

    [DllImport("/System/Library/Frameworks/ApplicationServices.framework/Versions/Current/ApplicationServices")]
    private static short NewSpeechChannel(IntPtr voice, ref IntPtr result);

    [DllImport("/System/Library/Frameworks/ApplicationServices.framework/Versions/Current/ApplicationServices")]
    private static short SpeakText(IntPtr channel, string text, long length);

    public bool Speak(string text)
    {
      return (int) SpeechChannel.SpeakText(this._id, text, (long) text.Length) == 0;
    }
  }
}
