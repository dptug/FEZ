// Type: OpenTK.Audio.OpenAL.XRamExtension
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using System;
using System.Runtime.InteropServices;

namespace OpenTK.Audio.OpenAL
{
  [CLSCompliant(true)]
  public sealed class XRamExtension
  {
    private bool _valid;
    private XRamExtension.Delegate_SetBufferMode Imported_SetBufferMode;
    private XRamExtension.Delegate_GetBufferMode Imported_GetBufferMode;
    private int AL_EAX_RAM_SIZE;
    private int AL_EAX_RAM_FREE;
    private int AL_STORAGE_AUTOMATIC;
    private int AL_STORAGE_HARDWARE;
    private int AL_STORAGE_ACCESSIBLE;

    public bool IsInitialized
    {
      get
      {
        return this._valid;
      }
    }

    public int GetRamSize
    {
      get
      {
        return AL.Get((ALGetInteger) this.AL_EAX_RAM_SIZE);
      }
    }

    public int GetRamFree
    {
      get
      {
        return AL.Get((ALGetInteger) this.AL_EAX_RAM_FREE);
      }
    }

    public XRamExtension()
    {
      this._valid = false;
      if (!AL.IsExtensionPresent("EAX-RAM"))
        return;
      this.AL_EAX_RAM_SIZE = AL.GetEnumValue("AL_EAX_RAM_SIZE");
      this.AL_EAX_RAM_FREE = AL.GetEnumValue("AL_EAX_RAM_FREE");
      this.AL_STORAGE_AUTOMATIC = AL.GetEnumValue("AL_STORAGE_AUTOMATIC");
      this.AL_STORAGE_HARDWARE = AL.GetEnumValue("AL_STORAGE_HARDWARE");
      this.AL_STORAGE_ACCESSIBLE = AL.GetEnumValue("AL_STORAGE_ACCESSIBLE");
      if (this.AL_EAX_RAM_SIZE == 0 || this.AL_EAX_RAM_FREE == 0 || (this.AL_STORAGE_AUTOMATIC == 0 || this.AL_STORAGE_HARDWARE == 0))
        return;
      if (this.AL_STORAGE_ACCESSIBLE == 0)
        return;
      try
      {
        this.Imported_GetBufferMode = (XRamExtension.Delegate_GetBufferMode) Marshal.GetDelegateForFunctionPointer(AL.GetProcAddress("EAXGetBufferMode"), typeof (XRamExtension.Delegate_GetBufferMode));
        this.Imported_SetBufferMode = (XRamExtension.Delegate_SetBufferMode) Marshal.GetDelegateForFunctionPointer(AL.GetProcAddress("EAXSetBufferMode"), typeof (XRamExtension.Delegate_SetBufferMode));
      }
      catch (Exception ex)
      {
        return;
      }
      this._valid = true;
    }

    [CLSCompliant(false)]
    public bool SetBufferMode(int n, ref uint buffer, XRamExtension.XRamStorage mode)
    {
      switch (mode)
      {
        case XRamExtension.XRamStorage.Hardware:
          return this.Imported_SetBufferMode(n, ref buffer, this.AL_STORAGE_HARDWARE);
        case XRamExtension.XRamStorage.Accessible:
          return this.Imported_SetBufferMode(n, ref buffer, this.AL_STORAGE_ACCESSIBLE);
        default:
          return this.Imported_SetBufferMode(n, ref buffer, this.AL_STORAGE_AUTOMATIC);
      }
    }

    [CLSCompliant(true)]
    public bool SetBufferMode(int n, ref int buffer, XRamExtension.XRamStorage mode)
    {
      uint buffer1 = (uint) buffer;
      return this.SetBufferMode(n, ref buffer1, mode);
    }

    [CLSCompliant(false)]
    public XRamExtension.XRamStorage GetBufferMode(ref uint buffer)
    {
      int num = this.Imported_GetBufferMode(buffer, IntPtr.Zero);
      if (num == this.AL_STORAGE_ACCESSIBLE)
        return XRamExtension.XRamStorage.Accessible;
      return num == this.AL_STORAGE_HARDWARE ? XRamExtension.XRamStorage.Hardware : XRamExtension.XRamStorage.Automatic;
    }

    [CLSCompliant(true)]
    public XRamExtension.XRamStorage GetBufferMode(ref int buffer)
    {
      uint buffer1 = (uint) buffer;
      return this.GetBufferMode(ref buffer1);
    }

    private delegate bool Delegate_SetBufferMode(int n, ref uint buffers, int value);

    private delegate int Delegate_GetBufferMode(uint buffer, IntPtr value);

    public enum XRamStorage : byte
    {
      Automatic,
      Hardware,
      Accessible,
    }
  }
}
