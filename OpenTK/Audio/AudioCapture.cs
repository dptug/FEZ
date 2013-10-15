// Type: OpenTK.Audio.AudioCapture
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using OpenTK;
using OpenTK.Audio.OpenAL;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace OpenTK.Audio
{
  public sealed class AudioCapture : IDisposable
  {
    private IntPtr Handle;
    private bool _isrecording;
    private ALFormat sample_format;
    private int sample_frequency;
    private string device_name;
    private bool IsDisposed;

    public string CurrentDevice
    {
      get
      {
        return this.device_name;
      }
    }

    public static IList<string> AvailableDevices
    {
      get
      {
        return AudioDeviceEnumerator.AvailableRecordingDevices;
      }
    }

    public static string DefaultDevice
    {
      get
      {
        return AudioDeviceEnumerator.DefaultRecordingDevice;
      }
    }

    public AlcError CurrentError
    {
      get
      {
        return Alc.GetError(this.Handle);
      }
    }

    public int AvailableSamples
    {
      get
      {
        int data;
        Alc.GetInteger(this.Handle, AlcGetInteger.CaptureSamples, 1, out data);
        return data;
      }
    }

    public ALFormat SampleFormat
    {
      get
      {
        return this.sample_format;
      }
      private set
      {
        this.sample_format = value;
      }
    }

    public int SampleFrequency
    {
      get
      {
        return this.sample_frequency;
      }
      private set
      {
        this.sample_frequency = value;
      }
    }

    public bool IsRunning
    {
      get
      {
        return this._isrecording;
      }
    }

    static AudioCapture()
    {
      int num = AudioDeviceEnumerator.IsOpenALSupported ? 1 : 0;
    }

    public AudioCapture()
      : this(AudioCapture.DefaultDevice, 22050, ALFormat.Mono16, 4096)
    {
    }

    public AudioCapture(string deviceName, int frequency, ALFormat sampleFormat, int bufferSize)
    {
      if (!AudioDeviceEnumerator.IsOpenALSupported)
        throw new DllNotFoundException("soft_oal.dll");
      if (frequency <= 0)
        throw new ArgumentOutOfRangeException("frequency");
      if (bufferSize <= 0)
        throw new ArgumentOutOfRangeException("bufferSize");
      this.device_name = deviceName;
      this.Handle = Alc.CaptureOpenDevice(deviceName, frequency, sampleFormat, bufferSize);
      if (this.Handle == IntPtr.Zero)
      {
        this.device_name = "IntPtr.Zero";
        this.Handle = Alc.CaptureOpenDevice((string) null, frequency, sampleFormat, bufferSize);
      }
      if (this.Handle == IntPtr.Zero)
      {
        this.device_name = AudioDeviceEnumerator.DefaultRecordingDevice;
        this.Handle = Alc.CaptureOpenDevice(AudioDeviceEnumerator.DefaultRecordingDevice, frequency, sampleFormat, bufferSize);
      }
      if (this.Handle == IntPtr.Zero)
      {
        this.device_name = "None";
        throw new AudioDeviceException("All attempts to open capture devices returned IntPtr.Zero. See debug log for verbose list.");
      }
      else
      {
        this.CheckErrors();
        this.SampleFormat = sampleFormat;
        this.SampleFrequency = frequency;
      }
    }

    ~AudioCapture()
    {
      this.Dispose();
    }

    public void CheckErrors()
    {
      new AudioDeviceErrorChecker(this.Handle).Dispose();
    }

    public void Start()
    {
      Alc.CaptureStart(this.Handle);
      this._isrecording = true;
    }

    public void Stop()
    {
      Alc.CaptureStop(this.Handle);
      this._isrecording = false;
    }

    public void ReadSamples(IntPtr buffer, int sampleCount)
    {
      Alc.CaptureSamples(this.Handle, buffer, sampleCount);
    }

    public void ReadSamples<TBuffer>(TBuffer[] buffer, int sampleCount) where TBuffer : struct
    {
      if (buffer == null)
        throw new ArgumentNullException("buffer");
      int num = BlittableValueType<TBuffer>.Stride * buffer.Length;
      if (sampleCount * AudioCapture.GetSampleSize(this.SampleFormat) > num)
        throw new ArgumentOutOfRangeException("sampleCount");
      GCHandle gcHandle = GCHandle.Alloc((object) buffer, GCHandleType.Pinned);
      try
      {
        this.ReadSamples(gcHandle.AddrOfPinnedObject(), sampleCount);
      }
      finally
      {
        gcHandle.Free();
      }
    }

    private static int GetSampleSize(ALFormat format)
    {
      switch (format)
      {
        case ALFormat.Mono8:
          return 1;
        case ALFormat.Mono16:
          return 2;
        case ALFormat.Stereo8:
          return 2;
        case ALFormat.Stereo16:
          return 4;
        case ALFormat.MultiQuad8Ext:
          return 4;
        case ALFormat.MultiQuad16Ext:
          return 8;
        case ALFormat.MultiQuad32Ext:
          return 16;
        case ALFormat.MultiRear8Ext:
          return 1;
        case ALFormat.MultiRear16Ext:
          return 2;
        case ALFormat.MultiRear32Ext:
          return 4;
        case ALFormat.Multi51Chn8Ext:
          return 6;
        case ALFormat.Multi51Chn16Ext:
          return 12;
        case ALFormat.Multi51Chn32Ext:
          return 24;
        case ALFormat.Multi61Chn8Ext:
          return 7;
        case ALFormat.Multi71Chn16Ext:
          return 14;
        case ALFormat.Multi71Chn32Ext:
          return 28;
        case ALFormat.MonoFloat32Ext:
          return 4;
        case ALFormat.StereoFloat32Ext:
          return 8;
        case ALFormat.MonoDoubleExt:
          return 8;
        case ALFormat.StereoDoubleExt:
          return 16;
        default:
          return 1;
      }
    }

    private string ErrorMessage(string devicename, int frequency, ALFormat bufferformat, int buffersize)
    {
      AlcError currentError = this.CurrentError;
      string str;
      switch (currentError)
      {
        case AlcError.InvalidValue:
          str = ((object) currentError).ToString() + ": One of the parameters has an invalid value.";
          break;
        case AlcError.OutOfMemory:
          str = ((object) currentError).ToString() + ": The specified device is invalid, or can not capture audio.";
          break;
        default:
          str = ((object) currentError).ToString();
          break;
      }
      return "The handle returned by Alc.CaptureOpenDevice is null.\nAlc Error: " + (object) str + "\nDevice Name: " + devicename + "\nCapture frequency: " + (string) (object) frequency + "\nBuffer format: " + (string) (object) bufferformat + "\nBuffer Size: " + (string) (object) buffersize;
    }

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    private void Dispose(bool manual)
    {
      if (this.IsDisposed)
        return;
      if (this.Handle != IntPtr.Zero)
      {
        if (this._isrecording)
          this.Stop();
        Alc.CaptureCloseDevice(this.Handle);
      }
      this.IsDisposed = true;
    }
  }
}
