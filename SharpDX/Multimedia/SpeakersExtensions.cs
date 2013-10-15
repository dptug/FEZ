// Type: SharpDX.Multimedia.SpeakersExtensions
// Assembly: SharpDX, Version=2.4.2.0, Culture=neutral, PublicKeyToken=627a3d6d1956f55a
// MVID: 578390A1-1524-4146-8C27-2E9750400D7A
// Assembly location: F:\Program Files (x86)\FEZ\SharpDX.dll

namespace SharpDX.Multimedia
{
  public static class SpeakersExtensions
  {
    public static int ToChannelCount(Speakers speakers)
    {
      int num1 = (int) speakers;
      int num2 = 0;
      while (num1 != 0)
      {
        if ((num1 & 1) != 0)
          ++num2;
        num1 >>= 1;
      }
      return num2;
    }
  }
}
