// Type: OpenTK.Audio.AudioDeviceEnumerator
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using OpenTK;
using OpenTK.Audio.OpenAL;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace OpenTK.Audio
{
  public static class AudioDeviceEnumerator
  {
    private static readonly List<string> available_playback_devices = new List<string>();
    private static readonly List<string> available_recording_devices = new List<string>();
    private static bool openal_supported = true;
    private static string default_playback_device;
    private static string default_recording_device;
    private static string lastError;
    private static AudioDeviceEnumerator.AlcVersion version;

    internal static IList<string> AvailablePlaybackDevices
    {
      get
      {
        return (IList<string>) AudioDeviceEnumerator.available_playback_devices.AsReadOnly();
      }
    }

    internal static IList<string> AvailableRecordingDevices
    {
      get
      {
        return (IList<string>) AudioDeviceEnumerator.available_recording_devices.AsReadOnly();
      }
    }

    internal static string DefaultPlaybackDevice
    {
      get
      {
        return AudioDeviceEnumerator.default_playback_device;
      }
    }

    internal static string DefaultRecordingDevice
    {
      get
      {
        return AudioDeviceEnumerator.default_recording_device;
      }
    }

    internal static bool IsOpenALSupported
    {
      get
      {
        return AudioDeviceEnumerator.openal_supported;
      }
    }

    public static string LastError
    {
      get
      {
        return AudioDeviceEnumerator.lastError;
      }
    }

    internal static AudioDeviceEnumerator.AlcVersion Version
    {
      get
      {
        return AudioDeviceEnumerator.version;
      }
    }

    static AudioDeviceEnumerator()
    {
      IntPtr device = IntPtr.Zero;
      ContextHandle context = ContextHandle.Zero;
      try
      {
        device = Alc.OpenDevice((string) null);
        int num1 = (int) Alc.GetError(device);
        context = Alc.CreateContext(device, (int[]) null);
        int num2 = (int) Alc.GetError(device);
        bool flag = Alc.MakeContextCurrent(context);
        AlcError error1 = Alc.GetError(device);
        if (!flag)
        {
          throw new AudioContextException("Failed to create dummy Context. Device (" + device.ToString() + ") Context (" + context.Handle.ToString() + ") MakeContextCurrent " + (flag ? "succeeded" : "failed") + ", Alc Error (" + ((object) error1).ToString() + ") " + Alc.GetString(IntPtr.Zero, (AlcGetString) error1));
        }
        else
        {
          if (Alc.IsExtensionPresent(IntPtr.Zero, "ALC_ENUMERATION_EXT"))
          {
            AudioDeviceEnumerator.version = AudioDeviceEnumerator.AlcVersion.Alc1_1;
            if (Alc.IsExtensionPresent(IntPtr.Zero, "ALC_ENUMERATE_ALL_EXT"))
            {
              AudioDeviceEnumerator.available_playback_devices.AddRange((IEnumerable<string>) Alc.GetString(IntPtr.Zero, AlcGetStringList.AllDevicesSpecifier));
              AudioDeviceEnumerator.default_playback_device = Alc.GetString(IntPtr.Zero, AlcGetString.DefaultAllDevicesSpecifier);
            }
            else
            {
              AudioDeviceEnumerator.available_playback_devices.AddRange((IEnumerable<string>) Alc.GetString(IntPtr.Zero, AlcGetStringList.DeviceSpecifier));
              AudioDeviceEnumerator.default_playback_device = Alc.GetString(IntPtr.Zero, AlcGetString.DefaultDeviceSpecifier);
            }
          }
          else
            AudioDeviceEnumerator.version = AudioDeviceEnumerator.AlcVersion.Alc1_0;
          AlcError error2 = Alc.GetError(device);
          if (error2 != AlcError.NoError)
            throw new AudioContextException("Alc Error occured when querying available playback devices. " + ((object) error2).ToString());
          if (AudioDeviceEnumerator.version == AudioDeviceEnumerator.AlcVersion.Alc1_1 && Alc.IsExtensionPresent(IntPtr.Zero, "ALC_EXT_CAPTURE"))
          {
            AudioDeviceEnumerator.available_recording_devices.AddRange((IEnumerable<string>) Alc.GetString(IntPtr.Zero, AlcGetStringList.CaptureDeviceSpecifier));
            AudioDeviceEnumerator.default_recording_device = Alc.GetString(IntPtr.Zero, AlcGetString.CaptureDefaultDeviceSpecifier);
          }
          AlcError error3 = Alc.GetError(device);
          if (error3 != AlcError.NoError)
            throw new AudioContextException("Alc Error occured when querying available recording devices. " + ((object) error3).ToString());
        }
      }
      catch (DllNotFoundException ex)
      {
        Trace.WriteLine(ex.ToString());
        AudioDeviceEnumerator.openal_supported = false;
      }
      catch (AudioContextException ex)
      {
        Trace.WriteLine(ex.ToString());
        AudioDeviceEnumerator.lastError = ex.ToString();
        AudioDeviceEnumerator.openal_supported = false;
      }
      finally
      {
        Alc.MakeContextCurrent(ContextHandle.Zero);
        if (context != ContextHandle.Zero && context.Handle != IntPtr.Zero)
          Alc.DestroyContext(context);
        if (device != IntPtr.Zero)
          Alc.CloseDevice(device);
      }
    }

    internal enum AlcVersion
    {
      Alc1_0,
      Alc1_1,
    }
  }
}
