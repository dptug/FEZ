// Type: OpenTK.Audio.AudioDeviceErrorChecker
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using OpenTK.Audio.OpenAL;
using System;

namespace OpenTK.Audio
{
  internal struct AudioDeviceErrorChecker : IDisposable
  {
    private static readonly string ErrorString = "Device {0} reported {1}.";
    private readonly IntPtr Device;

    static AudioDeviceErrorChecker()
    {
    }

    public AudioDeviceErrorChecker(IntPtr device)
    {
      if (device == IntPtr.Zero)
        throw new AudioDeviceException();
      this.Device = device;
    }

    public void Dispose()
    {
      AlcError error = Alc.GetError(this.Device);
      switch (error)
      {
        case AlcError.InvalidDevice:
          throw new AudioDeviceException(string.Format(AudioDeviceErrorChecker.ErrorString, (object) this.Device, (object) error));
        case AlcError.InvalidContext:
          throw new AudioContextException(string.Format(AudioDeviceErrorChecker.ErrorString, (object) this.Device, (object) error));
        case AlcError.InvalidValue:
          throw new AudioValueException(string.Format(AudioDeviceErrorChecker.ErrorString, (object) this.Device, (object) error));
        case AlcError.OutOfMemory:
          throw new OutOfMemoryException(string.Format(AudioDeviceErrorChecker.ErrorString, (object) this.Device, (object) error));
      }
    }
  }
}
