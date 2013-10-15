// Type: OpenTK.Audio.AudioContext
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using OpenTK;
using OpenTK.Audio.OpenAL;
using System;
using System.Collections.Generic;

namespace OpenTK.Audio
{
  public sealed class AudioContext : IDisposable
  {
    private static object audio_context_lock = new object();
    private static Dictionary<ContextHandle, AudioContext> available_contexts = new Dictionary<ContextHandle, AudioContext>();
    private bool disposed;
    private bool is_processing;
    private bool is_synchronized;
    private IntPtr device_handle;
    private ContextHandle context_handle;
    private bool context_exists;
    private string device_name;

    internal bool IsCurrent
    {
      get
      {
        lock (AudioContext.audio_context_lock)
        {
          if (AudioContext.available_contexts.Count == 0)
            return false;
          else
            return AudioContext.CurrentContext == this;
        }
      }
      set
      {
        if (value)
          AudioContext.MakeCurrent(this);
        else
          AudioContext.MakeCurrent((AudioContext) null);
      }
    }

    private IntPtr Device
    {
      get
      {
        return this.device_handle;
      }
    }

    public AlcError CurrentError
    {
      get
      {
        if (this.disposed)
          throw new ObjectDisposedException(this.GetType().FullName);
        else
          return Alc.GetError(this.device_handle);
      }
    }

    public bool IsProcessing
    {
      get
      {
        if (this.disposed)
          throw new ObjectDisposedException(this.GetType().FullName);
        else
          return this.is_processing;
      }
      private set
      {
        this.is_processing = value;
      }
    }

    public bool IsSynchronized
    {
      get
      {
        if (this.disposed)
          throw new ObjectDisposedException(this.GetType().FullName);
        else
          return this.is_synchronized;
      }
      private set
      {
        this.is_synchronized = value;
      }
    }

    public string CurrentDevice
    {
      get
      {
        if (this.disposed)
          throw new ObjectDisposedException(this.GetType().FullName);
        else
          return this.device_name;
      }
    }

    public static AudioContext CurrentContext
    {
      get
      {
        lock (AudioContext.audio_context_lock)
        {
          if (AudioContext.available_contexts.Count == 0)
            return (AudioContext) null;
          AudioContext local_0;
          AudioContext.available_contexts.TryGetValue(Alc.GetCurrentContext(), out local_0);
          return local_0;
        }
      }
    }

    public static IList<string> AvailableDevices
    {
      get
      {
        return AudioDeviceEnumerator.AvailablePlaybackDevices;
      }
    }

    public static string DefaultDevice
    {
      get
      {
        return AudioDeviceEnumerator.DefaultPlaybackDevice;
      }
    }

    static AudioContext()
    {
      int num = AudioDeviceEnumerator.IsOpenALSupported ? 1 : 0;
    }

    public AudioContext()
      : this((string) null, 0, 0, false, true, AudioContext.MaxAuxiliarySends.UseDriverDefault)
    {
    }

    public AudioContext(string device)
      : this(device, 0, 0, false, true, AudioContext.MaxAuxiliarySends.UseDriverDefault)
    {
    }

    public AudioContext(string device, int freq)
      : this(device, freq, 0, false, true, AudioContext.MaxAuxiliarySends.UseDriverDefault)
    {
    }

    public AudioContext(string device, int freq, int refresh)
      : this(device, freq, refresh, false, true, AudioContext.MaxAuxiliarySends.UseDriverDefault)
    {
    }

    public AudioContext(string device, int freq, int refresh, bool sync)
      : this(AudioDeviceEnumerator.AvailablePlaybackDevices[0], freq, refresh, sync, true)
    {
    }

    public AudioContext(string device, int freq, int refresh, bool sync, bool enableEfx)
    {
      this.CreateContext(device, freq, refresh, sync, enableEfx, AudioContext.MaxAuxiliarySends.UseDriverDefault);
    }

    public AudioContext(string device, int freq, int refresh, bool sync, bool enableEfx, AudioContext.MaxAuxiliarySends efxMaxAuxSends)
    {
      this.CreateContext(device, freq, refresh, sync, enableEfx, efxMaxAuxSends);
    }

    ~AudioContext()
    {
      this.Dispose(false);
    }

    private void CreateContext(string device, int freq, int refresh, bool sync, bool enableEfx, AudioContext.MaxAuxiliarySends efxAuxiliarySends)
    {
      if (!AudioDeviceEnumerator.IsOpenALSupported)
        throw new DllNotFoundException("soft_oal.dll");
      if (AudioDeviceEnumerator.Version == AudioDeviceEnumerator.AlcVersion.Alc1_1 && AudioDeviceEnumerator.AvailablePlaybackDevices.Count == 0)
        throw new NotSupportedException("No audio hardware is available.");
      if (this.context_exists)
        throw new NotSupportedException("Multiple AudioContexts are not supported.");
      if (freq < 0)
        throw new ArgumentOutOfRangeException("freq", (object) freq, "Should be greater than zero.");
      if (refresh < 0)
        throw new ArgumentOutOfRangeException("refresh", (object) refresh, "Should be greater than zero.");
      if (!string.IsNullOrEmpty(device))
      {
        this.device_name = device;
        this.device_handle = Alc.OpenDevice(device);
      }
      if (this.device_handle == IntPtr.Zero)
      {
        this.device_name = "IntPtr.Zero (null string)";
        this.device_handle = Alc.OpenDevice((string) null);
      }
      if (this.device_handle == IntPtr.Zero)
      {
        this.device_name = AudioContext.DefaultDevice;
        this.device_handle = Alc.OpenDevice(AudioContext.DefaultDevice);
      }
      if (this.device_handle == IntPtr.Zero)
      {
        this.device_name = "None";
        throw new AudioDeviceException(string.Format("Audio device '{0}' does not exist or is tied up by another application.", string.IsNullOrEmpty(device) ? (object) "default" : (object) device));
      }
      else
      {
        this.CheckErrors();
        List<int> list = new List<int>();
        if (freq != 0)
        {
          list.Add(4103);
          list.Add(freq);
        }
        if (refresh != 0)
        {
          list.Add(4104);
          list.Add(refresh);
        }
        list.Add(4105);
        list.Add(sync ? 1 : 0);
        if (enableEfx && Alc.IsExtensionPresent(this.device_handle, "ALC_EXT_EFX"))
        {
          int data;
          switch (efxAuxiliarySends)
          {
            case AudioContext.MaxAuxiliarySends.One:
            case AudioContext.MaxAuxiliarySends.Two:
            case AudioContext.MaxAuxiliarySends.Three:
            case AudioContext.MaxAuxiliarySends.Four:
              data = (int) efxAuxiliarySends;
              break;
            default:
              Alc.GetInteger(this.device_handle, AlcGetInteger.EfxMaxAuxiliarySends, 1, out data);
              break;
          }
          list.Add(131075);
          list.Add(data);
        }
        list.Add(0);
        this.context_handle = Alc.CreateContext(this.device_handle, list.ToArray());
        if (this.context_handle == ContextHandle.Zero)
        {
          Alc.CloseDevice(this.device_handle);
          throw new AudioContextException("The audio context could not be created with the specified parameters.");
        }
        else
        {
          this.CheckErrors();
          if (AudioDeviceEnumerator.AvailablePlaybackDevices.Count > 0)
            this.MakeCurrent();
          this.CheckErrors();
          this.device_name = Alc.GetString(this.device_handle, AlcGetString.DeviceSpecifier);
          lock (AudioContext.audio_context_lock)
          {
            AudioContext.available_contexts.Add(this.context_handle, this);
            this.context_exists = true;
          }
        }
      }
    }

    private static void MakeCurrent(AudioContext context)
    {
      lock (AudioContext.audio_context_lock)
      {
        if (!Alc.MakeContextCurrent(context != null ? context.context_handle : ContextHandle.Zero))
          throw new AudioContextException(string.Format("ALC {0} error detected at {1}.", (object) ((object) Alc.GetError(context != null ? (IntPtr) context.context_handle : IntPtr.Zero)).ToString(), context != null ? (object) context.ToString() : (object) "null"));
      }
    }

    public void CheckErrors()
    {
      if (this.disposed)
        throw new ObjectDisposedException(this.GetType().FullName);
      new AudioDeviceErrorChecker(this.device_handle).Dispose();
    }

    public void MakeCurrent()
    {
      if (this.disposed)
        throw new ObjectDisposedException(this.GetType().FullName);
      AudioContext.MakeCurrent(this);
    }

    public void Process()
    {
      if (this.disposed)
        throw new ObjectDisposedException(this.GetType().FullName);
      Alc.ProcessContext(this.context_handle);
      this.IsProcessing = true;
    }

    public void Suspend()
    {
      if (this.disposed)
        throw new ObjectDisposedException(this.GetType().FullName);
      Alc.SuspendContext(this.context_handle);
      this.IsProcessing = false;
    }

    public bool SupportsExtension(string extension)
    {
      if (this.disposed)
        throw new ObjectDisposedException(this.GetType().FullName);
      else
        return Alc.IsExtensionPresent(this.Device, extension);
    }

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    private void Dispose(bool manual)
    {
      if (this.disposed)
        return;
      if (this.IsCurrent)
        this.IsCurrent = false;
      if (this.context_handle != ContextHandle.Zero)
      {
        AudioContext.available_contexts.Remove(this.context_handle);
        Alc.DestroyContext(this.context_handle);
      }
      if (this.device_handle != IntPtr.Zero)
        Alc.CloseDevice(this.device_handle);
      int num = manual ? 1 : 0;
      this.disposed = true;
    }

    public override int GetHashCode()
    {
      return base.GetHashCode();
    }

    public override bool Equals(object obj)
    {
      return base.Equals(obj);
    }

    public override string ToString()
    {
      return string.Format("{0} (handle: {1}, device: {2})", (object) this.device_name, (object) this.context_handle, (object) this.device_handle);
    }

    public enum MaxAuxiliarySends
    {
      UseDriverDefault,
      One,
      Two,
      Three,
      Four,
    }
  }
}
