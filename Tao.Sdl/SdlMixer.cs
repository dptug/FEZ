// Type: Tao.Sdl.SdlMixer
// Assembly: Tao.Sdl, Version=1.2.13.0, Culture=neutral, PublicKeyToken=9c7a200e36c0094e
// MVID: 45EBE10E-1CB9-425D-83FF-C3B8997BBF28
// Assembly location: F:\Program Files (x86)\FEZ\Tao.Sdl.dll

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace Tao.Sdl
{
  [SuppressUnmanagedCodeSecurity]
  public static class SdlMixer
  {
    private const string SDL_MIXER_NATIVE_LIBRARY = "SDL_mixer.dll";
    private const CallingConvention CALLING_CONVENTION = CallingConvention.Cdecl;
    public const int MIX_MAJOR_VERSION = 1;
    public const int MIX_MINOR_VERSION = 2;
    public const int MIX_PATCHLEVEL = 7;
    public const int MIX_CHANNELS = 8;
    public const int MIX_DEFAULT_FREQUENCY = 22050;
    public const int MIX_DEFAULT_CHANNELS = 2;
    public const int MIX_MAX_VOLUME = 128;
    public const int MIX_CHANNEL_POST = -2;
    public const string MIX_EFFECTSMAXSPEED = "MIX_EFFECTSMAXSPEED";
    public const int MIX_NO_FADING = 0;
    public const int MIX_FADING_OUT = 1;
    public const int MIX_FADING_IN = 2;
    public const int MUS_NONE = 0;
    public const int MUS_CMD = 1;
    public const int MUS_WAV = 2;
    public const int MUS_MOD = 3;
    public const int MUS_MID = 4;
    public const int MUS_OGG = 5;
    public const int MUS_MP3 = 6;

    public static int MIX_DEFAULT_FORMAT
    {
      get
      {
        if (Sdl.SDL_BYTEORDER == 1234)
          return Sdl.AUDIO_S16SYS;
        else
          return -28656;
      }
    }

    public static Sdl.SDL_version MIX_VERSION()
    {
      return new Sdl.SDL_version()
      {
        major = (byte) 1,
        minor = (byte) 2,
        patch = (byte) 7
      };
    }

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_mixer.dll", EntryPoint = "Mix_Linked_Version", CallingConvention = CallingConvention.Cdecl)]
    private static IntPtr Mix_Linked_VersionInternal();

    public static Sdl.SDL_version Mix_Linked_Version()
    {
      return (Sdl.SDL_version) Marshal.PtrToStructure(SdlMixer.Mix_Linked_VersionInternal(), typeof (Sdl.SDL_version));
    }

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_mixer.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int Mix_OpenAudio(int frequency, short format, int channels, int chunksize);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_mixer.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int Mix_AllocateChannels(int numchans);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_mixer.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int Mix_QuerySpec(out int frequency, out short format, out int channels);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_mixer.dll", CallingConvention = CallingConvention.Cdecl)]
    public static IntPtr Mix_LoadWAV_RW(IntPtr src, int freesrc);

    public static IntPtr Mix_LoadWAV(string file)
    {
      return SdlMixer.Mix_LoadWAV_RW(Sdl.SDL_RWFromFile(file, "rb"), 1);
    }

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_mixer.dll", CallingConvention = CallingConvention.Cdecl)]
    public static IntPtr Mix_LoadMUS(string file);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_mixer.dll", CallingConvention = CallingConvention.Cdecl)]
    public static IntPtr Mix_LoadMUS_RW(IntPtr rw);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_mixer.dll", CallingConvention = CallingConvention.Cdecl)]
    public static IntPtr Mix_QuickLoad_WAV(IntPtr mem);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_mixer.dll", CallingConvention = CallingConvention.Cdecl)]
    public static IntPtr Mix_QuickLoad_RAW(IntPtr mem, int len);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_mixer.dll", CallingConvention = CallingConvention.Cdecl)]
    public static void Mix_FreeChunk(IntPtr chunk);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_mixer.dll", CallingConvention = CallingConvention.Cdecl)]
    public static void Mix_FreeMusic(IntPtr music);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_mixer.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int Mix_GetMusicType(IntPtr music);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_mixer.dll", CallingConvention = CallingConvention.Cdecl)]
    public static void Mix_SetPostMix(SdlMixer.MixFunctionDelegate mix_func, IntPtr arg);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_mixer.dll", CallingConvention = CallingConvention.Cdecl)]
    public static void Mix_HookMusic(SdlMixer.MixFunctionDelegate mix_func, IntPtr arg);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_mixer.dll", CallingConvention = CallingConvention.Cdecl)]
    public static void Mix_HookMusicFinished(SdlMixer.MusicFinishedDelegate music_finished);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_mixer.dll", CallingConvention = CallingConvention.Cdecl)]
    public static IntPtr Mix_GetMusicHookData();

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_mixer.dll", CallingConvention = CallingConvention.Cdecl)]
    public static void Mix_ChannelFinished(SdlMixer.ChannelFinishedDelegate channel_finished);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_mixer.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int Mix_RegisterEffect(int chan, SdlMixer.MixEffectFunctionDelegate f, SdlMixer.MixEffectDoneDelegate d, IntPtr arg);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_mixer.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int Mix_UnregisterEffect(int chan, SdlMixer.MixEffectFunctionDelegate f);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_mixer.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int Mix_UnregisterAllEffects(int channel);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_mixer.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int Mix_SetPanning(int channel, byte left, byte right);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_mixer.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int Mix_SetPosition(int channel, short angle, byte distance);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_mixer.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int Mix_SetDistance(int channel, byte distance);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_mixer.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int Mix_SetReverseStereo(int channel, int flip);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_mixer.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int Mix_ReserveChannels(int num);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_mixer.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int Mix_GroupChannel(int which, int tag);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_mixer.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int Mix_GroupChannels(int from, int to, int tag);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_mixer.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int Mix_GroupAvailable(int tag);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_mixer.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int Mix_GroupCount(int tag);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_mixer.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int Mix_GroupOldest(int tag);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_mixer.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int Mix_GroupNewer(int tag);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_mixer.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int Mix_PlayChannelTimed(int channel, IntPtr chunk, int loops, int ticks);

    public static int Mix_PlayChannel(int channel, IntPtr chunk, int loops)
    {
      return SdlMixer.Mix_PlayChannelTimed(channel, chunk, loops, -1);
    }

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_mixer.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int Mix_PlayMusic(IntPtr music, int loops);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_mixer.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int Mix_FadeInMusic(IntPtr music, int loops, int ms);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_mixer.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int Mix_FadeInMusicPos(IntPtr music, int loops, int ms, double position);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_mixer.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int Mix_FadeInChannelTimed(int channel, IntPtr chunk, int loops, int ms, int ticks);

    public static int Mix_FadeInChannel(int channel, IntPtr chunk, int loops, int ms)
    {
      return SdlMixer.Mix_FadeInChannelTimed(channel, chunk, loops, ms, -1);
    }

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_mixer.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int Mix_Volume(int channel, int volume);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_mixer.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int Mix_VolumeChunk(IntPtr chunk, int volume);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_mixer.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int Mix_VolumeMusic(int volume);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_mixer.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int Mix_HaltChannel(int channel);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_mixer.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int Mix_HaltGroup(int tag);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_mixer.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int Mix_HaltMusic();

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_mixer.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int Mix_ExpireChannel(int channel, int ticks);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_mixer.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int Mix_FadeOutChannel(int which, int ms);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_mixer.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int Mix_FadeOutGroup(int tag, int ms);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_mixer.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int Mix_FadeOutMusic(int ms);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_mixer.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int Mix_FadingMusic();

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_mixer.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int Mix_FadingChannel(int which);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_mixer.dll", CallingConvention = CallingConvention.Cdecl)]
    public static void Mix_Pause(int channel);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_mixer.dll", CallingConvention = CallingConvention.Cdecl)]
    public static void Mix_Resume(int channel);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_mixer.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int Mix_Paused(int channel);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_mixer.dll", CallingConvention = CallingConvention.Cdecl)]
    public static void Mix_PauseMusic();

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_mixer.dll", CallingConvention = CallingConvention.Cdecl)]
    public static void Mix_ResumeMusic();

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_mixer.dll", CallingConvention = CallingConvention.Cdecl)]
    public static void Mix_RewindMusic();

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_mixer.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int Mix_PausedMusic();

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_mixer.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int Mix_SetMusicPosition(double position);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_mixer.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int Mix_Playing(int channel);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_mixer.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int Mix_PlayingMusic();

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_mixer.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int Mix_SetMusicCMD(string command);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_mixer.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int Mix_SetSynchroValue(int value);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_mixer.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int Mix_GetSynchroValue();

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_mixer.dll", CallingConvention = CallingConvention.Cdecl)]
    public static IntPtr Mix_GetChunk(int channel);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_mixer.dll", CallingConvention = CallingConvention.Cdecl)]
    public static void Mix_CloseAudio();

    public static void Mix_SetError(string message)
    {
      Sdl.SDL_SetError(message);
    }

    public static string Mix_GetError()
    {
      return Sdl.SDL_GetError();
    }

    public struct Mix_Chunk
    {
      public int allocated;
      public IntPtr abuf;
      public int alen;
      public byte volume;
    }

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void MusicFinishedDelegate();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void MixFunctionDelegate(IntPtr udata, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2), Out] byte[] stream, int len);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void ChannelFinishedDelegate(int channel);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void MixEffectFunctionDelegate(int chan, IntPtr stream, int len, IntPtr udata);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void MixEffectDoneDelegate(int chan, IntPtr udata);
  }
}
