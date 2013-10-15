// Type: OptimusFix.SOP
// Assembly: OptimusFix, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D73C678E-9F94-43CA-B31C-E01183EAD576
// Assembly location: F:\Program Files (x86)\FEZ\OptimusFix.exe

using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace OptimusFix
{
  public static class SOP
  {
    private static SOP.CreateSessionDelegate CreateSession;
    private static SOP.CreateApplicationDelegate CreateApplication;
    private static SOP.CreateProfileDelegate CreateProfile;
    private static SOP.DeleteProfileDelegate DeleteProfile;
    private static SOP.DestroySessionDelegate DestroySession;
    private static SOP.EnumApplicationsDelegate EnumApplications;
    private static SOP.FindProfileByNameDelegate FindProfileByName;
    private static SOP.GetProfileInfoDelegate GetProfileInfo;
    private static SOP.InitializeDelegate Initialize;
    private static SOP.LoadSettingsDelegate LoadSettings;
    private static SOP.SaveSettingsDelegate SaveSettings;
    private static SOP.SetSettingDelegate SetSetting;

    [DllImport("kernel32.dll")]
    private static IntPtr LoadLibrary(string dllToLoad);

    [DllImport("nvapi.dll", EntryPoint = "nvapi_QueryInterface", CallingConvention = CallingConvention.Cdecl)]
    private static IntPtr QueryInterface(uint offset);

    private static bool CheckForError(int status)
    {
      return status != 0;
    }

    private static unsafe bool UnicodeStringCompare(ushort* unicodeString, ushort[] referenceString)
    {
      for (int index = 0; index < 2048; ++index)
      {
        if ((int) unicodeString[index] != (int) referenceString[index])
          return false;
      }
      return true;
    }

    private static ushort[] GetUnicodeString(string sourceString)
    {
      ushort[] numArray = new ushort[2048];
      for (int index = 0; index < 2048; ++index)
        numArray[index] = index >= sourceString.Length ? (ushort) 0 : Convert.ToUInt16(sourceString[index]);
      return numArray;
    }

    private static bool GetProcs()
    {
      if (IntPtr.Size != 4)
        return false;
      if (SOP.LoadLibrary("nvapi.dll") == IntPtr.Zero)
        return false;
      try
      {
        SOP.CreateApplication = Marshal.GetDelegateForFunctionPointer(SOP.QueryInterface(1128770014U), typeof (SOP.CreateApplicationDelegate)) as SOP.CreateApplicationDelegate;
        SOP.CreateProfile = Marshal.GetDelegateForFunctionPointer(SOP.QueryInterface(3424084072U), typeof (SOP.CreateProfileDelegate)) as SOP.CreateProfileDelegate;
        SOP.CreateSession = Marshal.GetDelegateForFunctionPointer(SOP.QueryInterface(110417198U), typeof (SOP.CreateSessionDelegate)) as SOP.CreateSessionDelegate;
        SOP.DeleteProfile = Marshal.GetDelegateForFunctionPointer(SOP.QueryInterface(386478598U), typeof (SOP.DeleteProfileDelegate)) as SOP.DeleteProfileDelegate;
        SOP.DestroySession = Marshal.GetDelegateForFunctionPointer(SOP.QueryInterface(3671707640U), typeof (SOP.DestroySessionDelegate)) as SOP.DestroySessionDelegate;
        SOP.EnumApplications = Marshal.GetDelegateForFunctionPointer(SOP.QueryInterface(2141329210U), typeof (SOP.EnumApplicationsDelegate)) as SOP.EnumApplicationsDelegate;
        SOP.FindProfileByName = Marshal.GetDelegateForFunctionPointer(SOP.QueryInterface(2118818315U), typeof (SOP.FindProfileByNameDelegate)) as SOP.FindProfileByNameDelegate;
        SOP.GetProfileInfo = Marshal.GetDelegateForFunctionPointer(SOP.QueryInterface(1640853462U), typeof (SOP.GetProfileInfoDelegate)) as SOP.GetProfileInfoDelegate;
        SOP.Initialize = Marshal.GetDelegateForFunctionPointer(SOP.QueryInterface(22079528U), typeof (SOP.InitializeDelegate)) as SOP.InitializeDelegate;
        SOP.LoadSettings = Marshal.GetDelegateForFunctionPointer(SOP.QueryInterface(928890219U), typeof (SOP.LoadSettingsDelegate)) as SOP.LoadSettingsDelegate;
        SOP.SaveSettings = Marshal.GetDelegateForFunctionPointer(SOP.QueryInterface(4240211476U), typeof (SOP.SaveSettingsDelegate)) as SOP.SaveSettingsDelegate;
        SOP.SetSetting = Marshal.GetDelegateForFunctionPointer(SOP.QueryInterface(1467863554U), typeof (SOP.SetSettingDelegate)) as SOP.SetSettingDelegate;
      }
      catch (Exception ex)
      {
        return false;
      }
      return true;
    }

    private static unsafe bool ContainsApplication(IntPtr session, IntPtr profile, SOP.Profile profileDescriptor, ushort[] unicodeApplicationName, out SOP.Application application)
    {
      application = new SOP.Application();
      if ((int) profileDescriptor.numOfApps == 0)
        return false;
      SOP.Application[] applicationArray = new SOP.Application[(IntPtr) profileDescriptor.numOfApps];
      uint appCount = profileDescriptor.numOfApps;
      fixed (SOP.Application* allApplications = applicationArray)
      {
        allApplications->version = 147464U;
        if (SOP.CheckForError(SOP.EnumApplications(session, profile, 0U, ref appCount, allApplications)))
          return false;
        for (uint index = 0U; index < appCount; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          if (SOP.UnicodeStringCompare(&((SOP.Application*) ((IntPtr) allApplications + (IntPtr) ((long) index * (long) sizeof (SOP.Application))))->appName.FixedElementField, unicodeApplicationName))
          {
            application = *(SOP.Application*) ((IntPtr) allApplications + (IntPtr) ((long) index * (long) sizeof (SOP.Application)));
            return true;
          }
        }
      }
      return false;
    }

    public static bool SOP_CheckProfile(string profileName)
    {
      IntPtr session;
      if (!SOP.GetProcs() || SOP.CheckForError(SOP.Initialize()) || (SOP.CheckForError(SOP.CreateSession(out session)) || SOP.CheckForError(SOP.LoadSettings(session))))
        return false;
      SOP.GetUnicodeString(profileName);
      IntPtr profile;
      bool flag = SOP.FindProfileByName(session, profileName, out profile) == 0;
      int num = SOP.DestroySession(session);
      return flag;
    }

    public static SOP.ResultCodes SOP_RemoveProfile(string profileName)
    {
      int num = 0;
      IntPtr session;
      if (!SOP.GetProcs() || SOP.CheckForError(SOP.Initialize()) || (SOP.CheckForError(SOP.CreateSession(out session)) || SOP.CheckForError(SOP.LoadSettings(session))))
        return SOP.ResultCodes.Error;
      SOP.GetUnicodeString(profileName);
      IntPtr profile;
      SOP.ResultCodes resultCodes;
      switch (SOP.FindProfileByName(session, profileName, out profile))
      {
        case 0:
          if (SOP.CheckForError(SOP.DeleteProfile(session, profile)) || SOP.CheckForError(SOP.SaveSettings(session)))
            return SOP.ResultCodes.Error;
          resultCodes = SOP.ResultCodes.Change;
          break;
        case -163:
          resultCodes = SOP.ResultCodes.NoChange;
          break;
        default:
          return SOP.ResultCodes.Error;
      }
      num = SOP.DestroySession(session);
      return resultCodes;
    }

    public static unsafe SOP.ResultCodes SOP_SetProfile(string profileName, string applicationName)
    {
      SOP.ResultCodes resultCodes = SOP.ResultCodes.NoChange;
      int num = 0;
      IntPtr session;
      if (!SOP.GetProcs() || SOP.CheckForError(SOP.Initialize()) || (SOP.CheckForError(SOP.CreateSession(out session)) || SOP.CheckForError(SOP.LoadSettings(session))))
        return SOP.ResultCodes.Error;
      ushort[] unicodeString1 = SOP.GetUnicodeString(profileName);
      ushort[] unicodeString2 = SOP.GetUnicodeString(applicationName);
      IntPtr profile;
      int status = SOP.FindProfileByName(session, profileName, out profile);
      if (status == -163)
      {
        SOP.Profile profileInfo = new SOP.Profile();
        profileInfo.version = 69652U;
        profileInfo.isPredefined = 0U;
        for (int index = 0; index < 2048; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          (&profileInfo.profileName.FixedElementField)[index] = unicodeString1[index];
        }
        fixed (uint* numPtr = new uint[32])
        {
          profileInfo.gpuSupport = numPtr;
          *profileInfo.gpuSupport = 1U;
        }
        if (SOP.CheckForError(SOP.CreateProfile(session, ref profileInfo, out profile)))
          return SOP.ResultCodes.Error;
        if (SOP.CheckForError(SOP.SetSetting(session, profile, ref new SOP.Setting()
        {
          version = 77856U,
          settingID = 284810369U,
          u32CurrentValue = 17U
        })))
          return SOP.ResultCodes.Error;
      }
      else if (SOP.CheckForError(status))
        return SOP.ResultCodes.Error;
      SOP.Profile profileInfo1 = new SOP.Profile();
      profileInfo1.version = 69652U;
      if (SOP.CheckForError(SOP.GetProfileInfo(session, profile, ref profileInfo1)))
        return SOP.ResultCodes.Error;
      SOP.Application application = new SOP.Application();
      if (!SOP.ContainsApplication(session, profile, profileInfo1, SOP.GetUnicodeString(applicationName.ToLower(CultureInfo.InvariantCulture)), out application))
      {
        application.version = 147464U;
        application.isPredefined = 0U;
        for (int index = 0; index < 2048; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          (&application.appName.FixedElementField)[index] = unicodeString2[index];
        }
        if (SOP.CheckForError(SOP.CreateApplication(session, profile, ref application)) || SOP.CheckForError(SOP.SaveSettings(session)))
          return SOP.ResultCodes.Error;
        resultCodes = SOP.ResultCodes.Change;
      }
      num = SOP.DestroySession(session);
      return resultCodes;
    }

    public enum ResultCodes
    {
      Error = -1,
      NoChange = 0,
      Change = 1,
    }

    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    private struct Application
    {
      public uint version;
      public uint isPredefined;
      public unsafe fixed ushort appName[2048];
      public unsafe fixed ushort userFriendlyName[2048];
      public unsafe fixed ushort launcher[2048];
      public unsafe fixed ushort fileInFolder[2048];
    }

    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    private struct Profile
    {
      public uint version;
      public unsafe fixed ushort profileName[2048];
      public unsafe uint* gpuSupport;
      public uint isPredefined;
      public uint numOfApps;
      public uint numOfSettings;
    }

    [StructLayout(LayoutKind.Explicit, CharSet = CharSet.Unicode)]
    private struct Setting
    {
      [FieldOffset(0)]
      public uint version;
      [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2048)]
      [FieldOffset(4)]
      public string settingName;
      [FieldOffset(4100)]
      public uint settingID;
      [FieldOffset(4104)]
      public uint settingType;
      [FieldOffset(4108)]
      public uint settingLocation;
      [FieldOffset(4112)]
      public uint isCurrentPredefined;
      [FieldOffset(4116)]
      public uint isPredefinedValid;
      [FieldOffset(4120)]
      public uint u32PredefinedValue;
      [FieldOffset(8220)]
      public uint u32CurrentValue;
    }

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate int CreateSessionDelegate(out IntPtr session);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate int CreateApplicationDelegate(IntPtr session, IntPtr profile, ref SOP.Application application);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate int CreateProfileDelegate(IntPtr session, ref SOP.Profile profileInfo, out IntPtr profile);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate int DeleteProfileDelegate(IntPtr session, IntPtr profile);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate int DestroySessionDelegate(IntPtr session);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate int EnumApplicationsDelegate(IntPtr session, IntPtr profile, uint startIndex, ref uint appCount, SOP.Application* allApplications);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate int FindProfileByNameDelegate(IntPtr session, [MarshalAs(UnmanagedType.BStr)] string profileName, out IntPtr profile);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate int GetProfileInfoDelegate(IntPtr session, IntPtr profile, ref SOP.Profile profileInfo);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate int InitializeDelegate();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate int LoadSettingsDelegate(IntPtr session);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate int SaveSettingsDelegate(IntPtr session);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate int SetSettingDelegate(IntPtr session, IntPtr profile, ref SOP.Setting setting);
  }
}
