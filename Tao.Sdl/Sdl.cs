// Type: Tao.Sdl.Sdl
// Assembly: Tao.Sdl, Version=1.2.13.0, Culture=neutral, PublicKeyToken=9c7a200e36c0094e
// MVID: 45EBE10E-1CB9-425D-83FF-C3B8997BBF28
// Assembly location: F:\Program Files (x86)\FEZ\Tao.Sdl.dll

using System;
using System.Collections;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace Tao.Sdl
{
  [SuppressUnmanagedCodeSecurity]
  public static class Sdl
  {
    public static readonly short AUDIO_U16 = (short) 16;
    public static readonly short AUDIO_S16 = (short) -32752;
    private static byte[] keyboardState = new byte[323];
    private const string SDL_NATIVE_LIBRARY = "SDL.dll";
    private const CallingConvention CALLING_CONVENTION = CallingConvention.Cdecl;
    private const int BYTE_SIZE = 8;
    public const int SDL_INIT_TIMER = 1;
    public const int SDL_INIT_AUDIO = 16;
    public const int SDL_INIT_VIDEO = 32;
    public const int SDL_INIT_CDROM = 256;
    public const int SDL_INIT_JOYSTICK = 512;
    public const int SDL_INIT_NOPARACHUTE = 1048576;
    public const int SDL_INIT_EVENTTHREAD = 16777216;
    public const int SDL_INIT_EVERYTHING = 65535;
    public const byte SDL_APPMOUSEFOCUS = (byte) 1;
    public const byte SDL_APPINPUTFOCUS = (byte) 2;
    public const byte SDL_APPACTIVE = (byte) 4;
    public const short AUDIO_U8 = (short) 8;
    public const short AUDIO_S8 = (short) -32760;
    public const short AUDIO_U16LSB = (short) 16;
    public const short AUDIO_S16LSB = (short) -32752;
    public const short AUDIO_U16MSB = (short) 4112;
    public const short AUDIO_S16MSB = (short) -28656;
    public const int SDL_MIX_MAXVOLUME = 128;
    public const int SDL_MAX_TRACKS = 99;
    public const int SDL_AUDIO_TRACK = 0;
    public const int SDL_DATA_TRACK = 4;
    public const int CD_FPS = 75;
    public const int SDL_LIL_ENDIAN = 1234;
    public const int SDL_BIG_ENDIAN = 4321;
    public const byte SDL_PRESSED = (byte) 1;
    public const byte SDL_RELEASED = (byte) 0;
    public const int SDL_ALLEVENTS = -1;
    public const int SDL_QUERY = -1;
    public const int SDL_IGNORE = 0;
    public const int SDL_DISABLE = 0;
    public const int SDL_ENABLE = 1;
    public const byte SDL_HAT_CENTERED = (byte) 0;
    public const byte SDL_HAT_UP = (byte) 1;
    public const byte SDL_HAT_RIGHT = (byte) 2;
    public const byte SDL_HAT_DOWN = (byte) 4;
    public const byte SDL_HAT_LEFT = (byte) 8;
    public const byte SDL_HAT_RIGHTUP = (byte) 3;
    public const byte SDL_HAT_RIGHTDOWN = (byte) 6;
    public const byte SDL_HAT_LEFTUP = (byte) 9;
    public const byte SDL_HAT_LEFTDOWN = (byte) 12;
    public const int SDL_ALL_HOTKEYS = -1;
    public const int SDL_DEFAULT_REPEAT_DELAY = 500;
    public const int SDL_DEFAULT_REPEAT_INTERVAL = 30;
    public const short KMOD_CTRL = (short) 192;
    public const short KMOD_SHIFT = (short) 3;
    public const short KMOD_ALT = (short) 768;
    public const short KMOD_META = (short) 3072;
    public const byte SDL_BUTTON_LEFT = (byte) 1;
    public const byte SDL_BUTTON_MIDDLE = (byte) 2;
    public const byte SDL_BUTTON_RIGHT = (byte) 3;
    public const byte SDL_BUTTON_WHEELUP = (byte) 4;
    public const byte SDL_BUTTON_WHEELDOWN = (byte) 5;
    public const byte SDL_BUTTON_X1 = (byte) 6;
    public const byte SDL_BUTTON_X2 = (byte) 7;
    public const byte SDL_BUTTON_LMASK = (byte) 1;
    public const byte SDL_BUTTON_MMASK = (byte) 2;
    public const byte SDL_BUTTON_RMASK = (byte) 4;
    public const byte SDL_BUTTON_X1MASK = (byte) 32;
    public const byte SDL_BUTTON_X2MASK = (byte) 64;
    public const int SDL_MUTEX_TIMEDOUT = 1;
    public const int SDL_MUTEX_MAXWAIT = -1;
    public const int RW_SEEK_SET = 0;
    public const int RW_SEEK_CUR = 1;
    public const int RW_SEEK_END = 2;
    public const int SDL_TIMESLICE = 10;
    public const int TIMER_RESOLUTION = 10;
    public const int SDL_MAJOR_VERSION = 1;
    public const int SDL_MINOR_VERSION = 2;
    public const int SDL_PATCHLEVEL = 11;
    public const int SDL_ALPHA_OPAQUE = 255;
    public const int SDL_ALPHA_TRANSPARENT = 0;
    public const int SDL_SWSURFACE = 0;
    public const int SDL_HWSURFACE = 1;
    public const int SDL_ASYNCBLIT = 4;
    public const int SDL_ANYFORMAT = 268435456;
    public const int SDL_HWPALETTE = 536870912;
    public const int SDL_DOUBLEBUF = 1073741824;
    public const int SDL_FULLSCREEN = -2147483648;
    public const int SDL_OPENGL = 2;
    public const int SDL_OPENGLBLIT = 10;
    public const int SDL_RESIZABLE = 16;
    public const int SDL_NOFRAME = 32;
    public const int SDL_HWACCEL = 256;
    public const int SDL_SRCCOLORKEY = 4096;
    public const int SDL_RLEACCELOK = 8192;
    public const int SDL_RLEACCEL = 16384;
    public const int SDL_SRCALPHA = 65536;
    public const int SDL_PREALLOC = 16777216;
    public const int SDL_YV12_OVERLAY = 842094169;
    public const int SDL_IYUV_OVERLAY = 1448433993;
    public const int SDL_YUY2_OVERLAY = 844715353;
    public const int SDL_UYVY_OVERLAY = 1498831189;
    public const int SDL_YVYU_OVERLAY = 1431918169;
    public const byte SDL_LOGPAL = (byte) 1;
    public const byte SDL_PHYSPAL = (byte) 2;
    public const int SDL_AUDIO_STOPPED = 0;
    public const int SDL_AUDIO_PLAYING = 1;
    public const int SDL_AUDIO_PAUSED = 2;
    public const int CD_TRAYEMPTY = 0;
    public const int CD_STOPPED = 1;
    public const int CD_PLAYING = 2;
    public const int CD_PAUSED = 3;
    public const int CD_ERROR = -1;
    public const int SDL_NOEVENT = 0;
    [CLSCompliant(false)]
    public const int SDL_ACTIVEEVENT = 1;
    public const int SDL_KEYDOWN = 2;
    public const int SDL_KEYUP = 3;
    public const int SDL_MOUSEMOTION = 4;
    public const int SDL_MOUSEBUTTONDOWN = 5;
    public const int SDL_MOUSEBUTTONUP = 6;
    public const int SDL_JOYAXISMOTION = 7;
    public const int SDL_JOYBALLMOTION = 8;
    public const int SDL_JOYHATMOTION = 9;
    public const int SDL_JOYBUTTONDOWN = 10;
    public const int SDL_JOYBUTTONUP = 11;
    [CLSCompliant(false)]
    public const int SDL_QUIT = 12;
    [CLSCompliant(false)]
    public const int SDL_SYSWMEVENT = 13;
    public const int SDL_EVENT_RESERVEDA = 14;
    public const int SDL_EVENT_RESERVEDB = 15;
    public const int SDL_VIDEORESIZE = 16;
    public const int SDL_VIDEOEXPOSE = 17;
    public const int SDL_EVENT_RESERVED2 = 18;
    public const int SDL_EVENT_RESERVED3 = 19;
    public const int SDL_EVENT_RESERVED4 = 20;
    public const int SDL_EVENT_RESERVED5 = 21;
    public const int SDL_EVENT_RESERVED6 = 22;
    public const int SDL_EVENT_RESERVED7 = 23;
    [CLSCompliant(false)]
    public const int SDL_USEREVENT = 24;
    public const int SDL_NUMEVENTS = 32;
    public const int SDL_ACTIVEEVENTMASK = 2;
    public const int SDL_KEYDOWNMASK = 4;
    public const int SDL_KEYUPMASK = 8;
    public const int SDL_KEYEVENTMASK = 12;
    public const int SDL_MOUSEMOTIONMASK = 16;
    public const int SDL_MOUSEBUTTONDOWNMASK = 32;
    public const int SDL_MOUSEBUTTONUPMASK = 64;
    public const int SDL_MOUSEEVENTMASK = 112;
    public const int SDL_JOYAXISMOTIONMASK = 128;
    public const int SDL_JOYBALLMOTIONMASK = 256;
    public const int SDL_JOYHATMOTIONMASK = 512;
    public const int SDL_JOYBUTTONDOWNMASK = 1024;
    public const int SDL_JOYBUTTONUPMASK = 2048;
    public const int SDL_JOYEVENTMASK = 3968;
    public const int SDL_VIDEORESIZEMASK = 65536;
    public const int SDL_VIDEOEXPOSEMASK = 131072;
    public const int SDL_QUITMASK = 4096;
    public const int SDL_SYSWMEVENTMASK = 8192;
    public const int SDL_ADDEVENT = 0;
    public const int SDL_PEEKEVENT = 1;
    public const int SDL_GETEVENT = 2;
    public const int SDLK_UNKNOWN = 0;
    public const int SDLK_FIRST = 0;
    public const int SDLK_BACKSPACE = 8;
    public const int SDLK_TAB = 9;
    public const int SDLK_CLEAR = 12;
    public const int SDLK_RETURN = 13;
    public const int SDLK_PAUSE = 19;
    public const int SDLK_ESCAPE = 27;
    public const int SDLK_SPACE = 32;
    public const int SDLK_EXCLAIM = 33;
    public const int SDLK_QUOTEDBL = 34;
    public const int SDLK_HASH = 35;
    public const int SDLK_DOLLAR = 36;
    public const int SDLK_AMPERSAND = 38;
    public const int SDLK_QUOTE = 39;
    public const int SDLK_LEFTPAREN = 40;
    public const int SDLK_RIGHTPAREN = 41;
    public const int SDLK_ASTERISK = 42;
    public const int SDLK_PLUS = 43;
    public const int SDLK_COMMA = 44;
    public const int SDLK_MINUS = 45;
    public const int SDLK_PERIOD = 46;
    public const int SDLK_SLASH = 47;
    public const int SDLK_0 = 48;
    public const int SDLK_1 = 49;
    public const int SDLK_2 = 50;
    public const int SDLK_3 = 51;
    public const int SDLK_4 = 52;
    public const int SDLK_5 = 53;
    public const int SDLK_6 = 54;
    public const int SDLK_7 = 55;
    public const int SDLK_8 = 56;
    public const int SDLK_9 = 57;
    public const int SDLK_COLON = 58;
    public const int SDLK_SEMICOLON = 59;
    public const int SDLK_LESS = 60;
    public const int SDLK_EQUALS = 61;
    public const int SDLK_GREATER = 62;
    public const int SDLK_QUESTION = 63;
    public const int SDLK_AT = 64;
    public const int SDLK_LEFTBRACKET = 91;
    public const int SDLK_BACKSLASH = 92;
    public const int SDLK_RIGHTBRACKET = 93;
    public const int SDLK_CARET = 94;
    public const int SDLK_UNDERSCORE = 95;
    public const int SDLK_BACKQUOTE = 96;
    public const int SDLK_a = 97;
    public const int SDLK_b = 98;
    public const int SDLK_c = 99;
    public const int SDLK_d = 100;
    public const int SDLK_e = 101;
    public const int SDLK_f = 102;
    public const int SDLK_g = 103;
    public const int SDLK_h = 104;
    public const int SDLK_i = 105;
    public const int SDLK_j = 106;
    public const int SDLK_k = 107;
    public const int SDLK_l = 108;
    public const int SDLK_m = 109;
    public const int SDLK_n = 110;
    public const int SDLK_o = 111;
    public const int SDLK_p = 112;
    public const int SDLK_q = 113;
    public const int SDLK_r = 114;
    public const int SDLK_s = 115;
    public const int SDLK_t = 116;
    public const int SDLK_u = 117;
    public const int SDLK_v = 118;
    public const int SDLK_w = 119;
    public const int SDLK_x = 120;
    public const int SDLK_y = 121;
    public const int SDLK_z = 122;
    public const int SDLK_DELETE = 127;
    public const int SDLK_WORLD_0 = 160;
    public const int SDLK_WORLD_1 = 161;
    public const int SDLK_WORLD_2 = 162;
    public const int SDLK_WORLD_3 = 163;
    public const int SDLK_WORLD_4 = 164;
    public const int SDLK_WORLD_5 = 165;
    public const int SDLK_WORLD_6 = 166;
    public const int SDLK_WORLD_7 = 167;
    public const int SDLK_WORLD_8 = 168;
    public const int SDLK_WORLD_9 = 169;
    public const int SDLK_WORLD_10 = 170;
    public const int SDLK_WORLD_11 = 171;
    public const int SDLK_WORLD_12 = 172;
    public const int SDLK_WORLD_13 = 173;
    public const int SDLK_WORLD_14 = 174;
    public const int SDLK_WORLD_15 = 175;
    public const int SDLK_WORLD_16 = 176;
    public const int SDLK_WORLD_17 = 177;
    public const int SDLK_WORLD_18 = 178;
    public const int SDLK_WORLD_19 = 179;
    public const int SDLK_WORLD_20 = 180;
    public const int SDLK_WORLD_21 = 181;
    public const int SDLK_WORLD_22 = 182;
    public const int SDLK_WORLD_23 = 183;
    public const int SDLK_WORLD_24 = 184;
    public const int SDLK_WORLD_25 = 185;
    public const int SDLK_WORLD_26 = 186;
    public const int SDLK_WORLD_27 = 187;
    public const int SDLK_WORLD_28 = 188;
    public const int SDLK_WORLD_29 = 189;
    public const int SDLK_WORLD_30 = 190;
    public const int SDLK_WORLD_31 = 191;
    public const int SDLK_WORLD_32 = 192;
    public const int SDLK_WORLD_33 = 193;
    public const int SDLK_WORLD_34 = 194;
    public const int SDLK_WORLD_35 = 195;
    public const int SDLK_WORLD_36 = 196;
    public const int SDLK_WORLD_37 = 197;
    public const int SDLK_WORLD_38 = 198;
    public const int SDLK_WORLD_39 = 199;
    public const int SDLK_WORLD_40 = 200;
    public const int SDLK_WORLD_41 = 201;
    public const int SDLK_WORLD_42 = 202;
    public const int SDLK_WORLD_43 = 203;
    public const int SDLK_WORLD_44 = 204;
    public const int SDLK_WORLD_45 = 205;
    public const int SDLK_WORLD_46 = 206;
    public const int SDLK_WORLD_47 = 207;
    public const int SDLK_WORLD_48 = 208;
    public const int SDLK_WORLD_49 = 209;
    public const int SDLK_WORLD_50 = 210;
    public const int SDLK_WORLD_51 = 211;
    public const int SDLK_WORLD_52 = 212;
    public const int SDLK_WORLD_53 = 213;
    public const int SDLK_WORLD_54 = 214;
    public const int SDLK_WORLD_55 = 215;
    public const int SDLK_WORLD_56 = 216;
    public const int SDLK_WORLD_57 = 217;
    public const int SDLK_WORLD_58 = 218;
    public const int SDLK_WORLD_59 = 219;
    public const int SDLK_WORLD_60 = 220;
    public const int SDLK_WORLD_61 = 221;
    public const int SDLK_WORLD_62 = 222;
    public const int SDLK_WORLD_63 = 223;
    public const int SDLK_WORLD_64 = 224;
    public const int SDLK_WORLD_65 = 225;
    public const int SDLK_WORLD_66 = 226;
    public const int SDLK_WORLD_67 = 227;
    public const int SDLK_WORLD_68 = 228;
    public const int SDLK_WORLD_69 = 229;
    public const int SDLK_WORLD_70 = 230;
    public const int SDLK_WORLD_71 = 231;
    public const int SDLK_WORLD_72 = 232;
    public const int SDLK_WORLD_73 = 233;
    public const int SDLK_WORLD_74 = 234;
    public const int SDLK_WORLD_75 = 235;
    public const int SDLK_WORLD_76 = 236;
    public const int SDLK_WORLD_77 = 237;
    public const int SDLK_WORLD_78 = 238;
    public const int SDLK_WORLD_79 = 239;
    public const int SDLK_WORLD_80 = 240;
    public const int SDLK_WORLD_81 = 241;
    public const int SDLK_WORLD_82 = 242;
    public const int SDLK_WORLD_83 = 243;
    public const int SDLK_WORLD_84 = 244;
    public const int SDLK_WORLD_85 = 245;
    public const int SDLK_WORLD_86 = 246;
    public const int SDLK_WORLD_87 = 247;
    public const int SDLK_WORLD_88 = 248;
    public const int SDLK_WORLD_89 = 249;
    public const int SDLK_WORLD_90 = 250;
    public const int SDLK_WORLD_91 = 251;
    public const int SDLK_WORLD_92 = 252;
    public const int SDLK_WORLD_93 = 253;
    public const int SDLK_WORLD_94 = 254;
    public const int SDLK_WORLD_95 = 255;
    public const int SDLK_KP0 = 256;
    public const int SDLK_KP1 = 257;
    public const int SDLK_KP2 = 258;
    public const int SDLK_KP3 = 259;
    public const int SDLK_KP4 = 260;
    public const int SDLK_KP5 = 261;
    public const int SDLK_KP6 = 262;
    public const int SDLK_KP7 = 263;
    public const int SDLK_KP8 = 264;
    public const int SDLK_KP9 = 265;
    public const int SDLK_KP_PERIOD = 266;
    public const int SDLK_KP_DIVIDE = 267;
    public const int SDLK_KP_MULTIPLY = 268;
    public const int SDLK_KP_MINUS = 269;
    public const int SDLK_KP_PLUS = 270;
    public const int SDLK_KP_ENTER = 271;
    public const int SDLK_KP_EQUALS = 272;
    public const int SDLK_UP = 273;
    public const int SDLK_DOWN = 274;
    public const int SDLK_RIGHT = 275;
    public const int SDLK_LEFT = 276;
    public const int SDLK_INSERT = 277;
    public const int SDLK_HOME = 278;
    public const int SDLK_END = 279;
    public const int SDLK_PAGEUP = 280;
    public const int SDLK_PAGEDOWN = 281;
    public const int SDLK_F1 = 282;
    public const int SDLK_F2 = 283;
    public const int SDLK_F3 = 284;
    public const int SDLK_F4 = 285;
    public const int SDLK_F5 = 286;
    public const int SDLK_F6 = 287;
    public const int SDLK_F7 = 288;
    public const int SDLK_F8 = 289;
    public const int SDLK_F9 = 290;
    public const int SDLK_F10 = 291;
    public const int SDLK_F11 = 292;
    public const int SDLK_F12 = 293;
    public const int SDLK_F13 = 294;
    public const int SDLK_F14 = 295;
    public const int SDLK_F15 = 296;
    public const int SDLK_NUMLOCK = 300;
    public const int SDLK_CAPSLOCK = 301;
    public const int SDLK_SCROLLOCK = 302;
    public const int SDLK_RSHIFT = 303;
    public const int SDLK_LSHIFT = 304;
    public const int SDLK_RCTRL = 305;
    public const int SDLK_LCTRL = 306;
    public const int SDLK_RALT = 307;
    public const int SDLK_LALT = 308;
    public const int SDLK_RMETA = 309;
    public const int SDLK_LMETA = 310;
    public const int SDLK_LSUPER = 311;
    public const int SDLK_RSUPER = 312;
    public const int SDLK_MODE = 313;
    public const int SDLK_COMPOSE = 314;
    public const int SDLK_HELP = 315;
    public const int SDLK_PRINT = 316;
    public const int SDLK_SYSREQ = 317;
    public const int SDLK_BREAK = 318;
    public const int SDLK_MENU = 319;
    public const int SDLK_POWER = 320;
    public const int SDLK_EURO = 321;
    public const int SDLK_UNDO = 322;
    public const int SDLK_LAST = 323;
    public const int KMOD_NONE = 0;
    public const int KMOD_LSHIFT = 1;
    public const int KMOD_RSHIFT = 2;
    public const int KMOD_LCTRL = 64;
    public const int KMOD_RCTRL = 128;
    public const int KMOD_LALT = 256;
    public const int KMOD_RALT = 512;
    public const int KMOD_LMETA = 1024;
    public const int KMOD_RMETA = 2048;
    public const int KMOD_NUM = 4096;
    public const int KMOD_CAPS = 8192;
    public const int KMOD_MODE = 16384;
    public const int KMOD_RESERVED = 32768;
    public const int SDL_FALSE = 0;
    public const int SDL_TRUE = 1;
    public const int SDL_SYSWM_X11 = 0;
    public const int SDL_GL_RED_SIZE = 0;
    public const int SDL_GL_GREEN_SIZE = 1;
    public const int SDL_GL_BLUE_SIZE = 2;
    public const int SDL_GL_ALPHA_SIZE = 3;
    public const int SDL_GL_BUFFER_SIZE = 4;
    public const int SDL_GL_DOUBLEBUFFER = 5;
    public const int SDL_GL_DEPTH_SIZE = 6;
    public const int SDL_GL_STENCIL_SIZE = 7;
    public const int SDL_GL_ACCUM_RED_SIZE = 8;
    public const int SDL_GL_ACCUM_GREEN_SIZE = 9;
    public const int SDL_GL_ACCUM_BLUE_SIZE = 10;
    public const int SDL_GL_ACCUM_ALPHA_SIZE = 11;
    public const int SDL_GL_STEREO = 12;
    public const int SDL_GL_MULTISAMPLEBUFFERS = 13;
    public const int SDL_GL_MULTISAMPLESAMPLES = 14;
    public const int SDL_GL_ACCELERATED_VISUAL = 15;
    public const int SDL_GL_SWAP_CONTROL = 16;
    public const int SDL_GRAB_QUERY = -1;
    public const int SDL_GRAB_OFF = 0;
    public const int SDL_GRAB_ON = 1;
    public const int SDL_GRAB_FULLSCREEN = 2;

    public static int AUDIO_U16SYS
    {
      get
      {
        return Sdl.SDL_BYTEORDER == 1234 ? 16 : 4112;
      }
    }

    public static int AUDIO_S16SYS
    {
      get
      {
        return Sdl.SDL_BYTEORDER == 1234 ? -32752 : -28656;
      }
    }

    public static int SDL_BYTEORDER
    {
      get
      {
        return BitConverter.IsLittleEndian ? 1234 : 4321;
      }
    }

    public static int SDL_COMPILEDVERSION
    {
      get
      {
        Sdl.SDL_version sdlVersion = Sdl.SDL_Linked_Version();
        return Sdl.SDL_VERSIONNUM(sdlVersion.major, sdlVersion.minor, sdlVersion.patch);
      }
    }

    static Sdl()
    {
    }

    [DllImport("/System/Library/Frameworks/Cocoa.framework/Cocoa")]
    private static void NSApplicationLoad();

    [DllImport("libobjc.dylib")]
    public static int objc_getClass(string name);

    [DllImport("libobjc.dylib")]
    public static int sel_registerName(string name);

    [DllImport("libobjc.dylib")]
    public static int objc_msgSend(int self, int cmd);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", EntryPoint = "SDL_Init", CallingConvention = CallingConvention.Cdecl)]
    private static int __SDL_Init(int flags);

    public static int SDL_Init(int flags)
    {
      try
      {
        if (File.Exists("/System/Library/Frameworks/Cocoa.framework/Cocoa"))
        {
          Sdl.objc_msgSend(Sdl.objc_getClass("NSAutoreleasePool"), Sdl.sel_registerName("new"));
          Sdl.NSApplicationLoad();
        }
      }
      catch
      {
      }
      return Sdl.__SDL_Init(flags);
    }

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", EntryPoint = "SDL_Init", CallingConvention = CallingConvention.Cdecl)]
    private static int __SDL_Init(uint flags);

    [CLSCompliant(false)]
    public static int SDL_Init(uint flags)
    {
      try
      {
        if (File.Exists("/System/Library/Frameworks/Cocoa.framework/Cocoa"))
        {
          Sdl.objc_msgSend(Sdl.objc_getClass("NSAutoreleasePool"), Sdl.sel_registerName("new"));
          Sdl.NSApplicationLoad();
        }
      }
      catch
      {
      }
      return Sdl.__SDL_Init(flags);
    }

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", EntryPoint = "SDL_InitSubSystem", CallingConvention = CallingConvention.Cdecl)]
    private static int __SDL_InitSubSystem(int flags);

    public static int SDL_InitSubSystem(int flags)
    {
      try
      {
        if (File.Exists("/System/Library/Frameworks/Cocoa.framework/Cocoa"))
        {
          Sdl.objc_msgSend(Sdl.objc_getClass("NSAutoreleasePool"), Sdl.sel_registerName("new"));
          Sdl.NSApplicationLoad();
        }
      }
      catch
      {
      }
      return Sdl.__SDL_InitSubSystem(flags);
    }

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", EntryPoint = "SDL_InitSubSystem", CallingConvention = CallingConvention.Cdecl)]
    private static int __SDL_InitSubSystem(uint flags);

    [CLSCompliant(false)]
    public static int SDL_InitSubSystem(uint flags)
    {
      try
      {
        if (File.Exists("/System/Library/Frameworks/Cocoa.framework/Cocoa"))
        {
          Sdl.objc_msgSend(Sdl.objc_getClass("NSAutoreleasePool"), Sdl.sel_registerName("new"));
          Sdl.NSApplicationLoad();
        }
      }
      catch
      {
      }
      return Sdl.__SDL_InitSubSystem(flags);
    }

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static void SDL_QuitSubSystem(int flags);

    [SuppressUnmanagedCodeSecurity]
    [CLSCompliant(false)]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static void SDL_QuitSubSystem(uint flags);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_WasInit(int flags);

    [SuppressUnmanagedCodeSecurity]
    [CLSCompliant(false)]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static uint SDL_WasInit(uint flags);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static void SDL_Quit();

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static byte SDL_GetAppState();

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_AudioInit(string driver_name);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static void SDL_AudioQuit();

    public static string SDL_AudioDriverName(string namebuf, int maxlen)
    {
      StringBuilder namebuf1 = new StringBuilder(namebuf);
      Sdl.__SDL_AudioDriverName(namebuf1, maxlen);
      return ((object) namebuf1).ToString();
    }

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", EntryPoint = "SDL_AudioDriverName", CallingConvention = CallingConvention.Cdecl)]
    private static void __SDL_AudioDriverName(StringBuilder namebuf, int maxlen);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_OpenAudio(IntPtr desired, IntPtr obtained);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_GetAudioStatus();

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static void SDL_PauseAudio(int pause_on);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static IntPtr SDL_LoadWAV_RW(IntPtr src, int freesrc, out IntPtr spec, out IntPtr audio_buf, out int audio_len);

    [SuppressUnmanagedCodeSecurity]
    [CLSCompliant(false)]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static IntPtr SDL_LoadWAV_RW(IntPtr src, int freesrc, out IntPtr spec, out IntPtr audio_buf, out uint audio_len);

    public static IntPtr SDL_LoadWAV(string file, out IntPtr spec, out IntPtr audio_buf, out int audio_len)
    {
      IntPtr num = Sdl.SDL_LoadWAV_RW(Sdl.SDL_RWFromFile(file, "rb"), 1, out spec, out audio_buf, out audio_len);
      Console.WriteLine("audio_len: " + audio_len.ToString());
      return num;
    }

    [CLSCompliant(false)]
    public static IntPtr SDL_LoadWAV(string file, out IntPtr spec, out IntPtr audio_buf, out uint audio_len)
    {
      IntPtr num = Sdl.SDL_LoadWAV_RW(Sdl.SDL_RWFromFile(file, "rb"), 1, out spec, out audio_buf, out audio_len);
      Console.WriteLine("audio_len: " + audio_len.ToString());
      return num;
    }

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static void SDL_FreeWAV(ref IntPtr audio_buf);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_BuildAudioCVT(IntPtr cvt, short src_format, byte src_channels, int src_rate, short dst_format, byte dst_channels, int dst_rate);

    [SuppressUnmanagedCodeSecurity]
    [CLSCompliant(false)]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_BuildAudioCVT(IntPtr cvt, ushort src_format, byte src_channels, int src_rate, ushort dst_format, byte dst_channels, int dst_rate);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_ConvertAudio(IntPtr cvt);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static void SDL_MixAudio(IntPtr dst, IntPtr src, int len, int volume);

    [SuppressUnmanagedCodeSecurity]
    [CLSCompliant(false)]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static void SDL_MixAudio(IntPtr dst, IntPtr src, uint len, int volume);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static void SDL_LockAudio();

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static void SDL_UnlockAudio();

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static void SDL_CloseAudio();

    public static int CD_INDRIVE(int status)
    {
      return status > 0 ? 1 : 0;
    }

    public static void FRAMES_TO_MSF(int frames, out int M, out int S, out int F)
    {
      F = 0;
      S = 0;
      M = 0;
      if (frames == 0)
        return;
      F = frames % 75;
      frames /= 75;
      S = frames % 60;
      frames /= 60;
      M = frames;
    }

    public static int MSF_TO_FRAMES(int M, int S, int F)
    {
      return M * 60 * 75 + S * 75 + F;
    }

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_CDNumDrives();

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static string SDL_CDName(int drive);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static IntPtr SDL_CDOpen(int drive);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_CDStatus(IntPtr cdrom);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_CDPlay(IntPtr cdrom, int start, int length);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_CDPlayTracks(IntPtr cdrom, int start_track, int start_frame, int ntracks, int nframes);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_CDPause(IntPtr cdrom);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_CDResume(IntPtr cdrom);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_CDStop(IntPtr cdrom);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_CDEject(IntPtr cdrom);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static void SDL_CDClose(IntPtr cdrom);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_HasRDTSC();

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_HasMMX();

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_HasMMXExt();

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_Has3DNow();

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_HasSSE();

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_HasSSE2();

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_HasAltiVec();

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static void SDL_SetError(string message);

    public static string SDL_GetError()
    {
      return Marshal.PtrToStringAnsi(Sdl.__SDL_GetError());
    }

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", EntryPoint = "SDL_GetError", CallingConvention = CallingConvention.Cdecl)]
    private static IntPtr __SDL_GetError();

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static void SDL_ClearError();

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static void SDL_PumpEvents();

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_PeepEvents([In, Out] Sdl.SDL_Event[] events, int numevents, int action, int mask);

    [SuppressUnmanagedCodeSecurity]
    [CLSCompliant(false)]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_PeepEvents([In, Out] Sdl.SDL_Event[] events, int numevents, int action, uint mask);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_PollEvent(out Sdl.SDL_Event sdlEvent);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_WaitEvent(out Sdl.SDL_Event evt);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_PushEvent(out Sdl.SDL_Event evt);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static void SDL_SetEventFilter(Sdl.SDL_EventFilter filter);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static Sdl.SDL_EventFilter SDL_GetEventFilter();

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_EventState(byte type, int state);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_NumJoysticks();

    public static string SDL_JoystickName(int device_index)
    {
      return Marshal.PtrToStringAuto(Sdl.__SDL_JoystickName(device_index));
    }

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", EntryPoint = "SDL_JoystickName", CallingConvention = CallingConvention.Cdecl)]
    private static IntPtr __SDL_JoystickName(int device_index);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static IntPtr SDL_JoystickOpen(int device_index);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_JoystickOpened(int device_index);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_JoystickIndex(IntPtr joystick);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_JoystickNumAxes(IntPtr joystick);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_JoystickNumBalls(IntPtr joystick);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_JoystickNumHats(IntPtr joystick);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_JoystickNumButtons(IntPtr joystick);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static void SDL_JoystickUpdate();

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_JoystickEventState(int state);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static short SDL_JoystickGetAxis(IntPtr joystick, int axis);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static byte SDL_JoystickGetHat(IntPtr joystick, int hat);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_JoystickGetBall(IntPtr joystick, int ball, out int dx, out int dy);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static byte SDL_JoystickGetButton(IntPtr joystick, int button);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static void SDL_JoystickClose(IntPtr joystick);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_EnableUNICODE(int enable);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_EnableKeyRepeat(int rate, int delay);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_GetKeyRepeat(out int rate, out int delay);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", EntryPoint = "SDL_GetKeyState", CallingConvention = CallingConvention.Cdecl)]
    private static IntPtr SDL_GetKeyStateInternal(out int numkeys);

    public static byte[] SDL_GetKeyState(out int numkeys)
    {
      Marshal.Copy(Sdl.SDL_GetKeyStateInternal(out numkeys), Sdl.keyboardState, 0, numkeys);
      return Sdl.keyboardState;
    }

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_GetModState();

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static void SDL_SetModState(int modstate);

    public static string SDL_GetKeyName(int key)
    {
      return Marshal.PtrToStringAnsi(Sdl.__SDL_GetKeyName(key));
    }

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", EntryPoint = "SDL_GetKeyName", CallingConvention = CallingConvention.Cdecl)]
    private static IntPtr __SDL_GetKeyName(int key);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static IntPtr SDL_LoadObject(string sofile);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static IntPtr SDL_LoadFunction(IntPtr handle, string name);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static void SDL_UnloadObject(IntPtr handle);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static byte SDL_GetMouseState(out int x, out int y);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static byte SDL_GetRelativeMouseState(out int x, out int y);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static void SDL_WarpMouse(short x, short y);

    [SuppressUnmanagedCodeSecurity]
    [CLSCompliant(false)]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static void SDL_WarpMouse(ushort x, ushort y);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_CreateCursor(ref byte data, ref byte mask, int w, int h, int hot_x, int hot_y);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static IntPtr SDL_SetCursor(ref Sdl.SDL_Cursor cursor);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static IntPtr SDL_GetCursor();

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static void SDL_FreeCursor(ref Sdl.SDL_Cursor cursor);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_ShowCursor(int toggle);

    public static byte SDL_BUTTON(byte x)
    {
      return (byte) (1 << (int) x - 1);
    }

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static IntPtr SDL_CreateMutex();

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_mutexP(IntPtr mutex);

    public static int SDL_LockMutex(IntPtr m)
    {
      return Sdl.SDL_mutexP(m);
    }

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_mutexV(IntPtr mutex);

    public static int SDL_UnlockMutex(IntPtr m)
    {
      return Sdl.SDL_mutexV(m);
    }

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static void SDL_DestroyMutex(IntPtr mutex);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static IntPtr SDL_CreateSemaphore(int initial_value);

    [SuppressUnmanagedCodeSecurity]
    [CLSCompliant(false)]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static IntPtr SDL_CreateSemaphore(uint initial_value);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static void SDL_DestroySemaphore(IntPtr sem);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_SemWait(IntPtr sem);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_SemtryWait(IntPtr sem);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_SemWaitTimeout(IntPtr sem, int ms);

    [SuppressUnmanagedCodeSecurity]
    [CLSCompliant(false)]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_SemWaitTimeout(IntPtr sem, uint ms);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_SemPost(IntPtr sem);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_SemValue(IntPtr sem);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static IntPtr SDL_CreateCond();

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static void SDL_DestroyCond(IntPtr cond);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_CondSignal(IntPtr cond);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_CondBroadcast(IntPtr cond);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_CondWait(IntPtr cond, IntPtr mut);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_CondWaitTimeout(IntPtr cond, IntPtr mutex, int ms);

    [SuppressUnmanagedCodeSecurity]
    [CLSCompliant(false)]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_CondWaitTimeout(IntPtr cond, IntPtr mutex, uint ms);

    public static string SDL_NAME(string x)
    {
      return "SDL_" + x;
    }

    public static int SDL_QuitRequested()
    {
      Sdl.SDL_PumpEvents();
      return Sdl.SDL_PeepEvents((Sdl.SDL_Event[]) null, 0, 1, 4096);
    }

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static IntPtr SDL_RWFromFile(string file, string mode);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static IntPtr SDL_RWFromMem(IntPtr mem, int size);

    public static IntPtr SDL_RWFromMem(byte[] mem, int size)
    {
      IntPtr num = Marshal.AllocHGlobal(mem.Length);
      Marshal.Copy(mem, 0, num, mem.Length);
      return Sdl.SDL_RWFromMem(num, size);
    }

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static IntPtr SDL_RWFromFP(IntPtr fp, int autoclose);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static IntPtr SDL_RWFromConstMem(IntPtr mem, int size);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static IntPtr SDL_AllocRW();

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static void SDL_FreeRW(IntPtr context);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static short SDL_ReadLE16(IntPtr src);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static short SDL_ReadBE16(IntPtr src);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_ReadLE32(IntPtr src);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_ReadBE32(IntPtr src);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static long SDL_ReadLE64(IntPtr src);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static long SDL_ReadBE64(IntPtr src);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_WriteLE16(IntPtr dst, short val);

    [SuppressUnmanagedCodeSecurity]
    [CLSCompliant(false)]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_WriteLE16(IntPtr dst, ushort val);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_WriteBE16(IntPtr dst, short val);

    [SuppressUnmanagedCodeSecurity]
    [CLSCompliant(false)]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_WriteBE16(IntPtr dst, ushort val);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_WriteLE32(IntPtr dst, int val);

    [SuppressUnmanagedCodeSecurity]
    [CLSCompliant(false)]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_WriteLE32(IntPtr dst, uint val);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_WriteBE32(IntPtr dst, int val);

    [SuppressUnmanagedCodeSecurity]
    [CLSCompliant(false)]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_WriteBE32(IntPtr dst, uint val);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_WriteLE64(IntPtr dst, long val);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_WriteBE64(IntPtr dst, long val);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_putenv(string variable);

    public static string SDL_getenv(string name)
    {
      StringBuilder name1 = new StringBuilder(name);
      Sdl.__SDL_getenv(name1);
      return ((object) name1).ToString();
    }

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", EntryPoint = "SDL_getenv", CallingConvention = CallingConvention.Cdecl)]
    private static IntPtr __SDL_getenv(StringBuilder name);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_GetWMInfo(out Sdl.SDL_SysWMinfo_Unix info);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_GetWMInfo(out Sdl.SDL_SysWMinfo_Windows info);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_GetWMInfo(out Sdl.SDL_SysWMinfo_RiscOS info);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_GetWMInfo(out Sdl.SDL_SysWMinfo info);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static IntPtr SDL_CreateThread(Sdl.ThreadDelegate fn, object data);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_ThreadID();

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_GetThreadID(IntPtr thread);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static void SDL_WaitThread(IntPtr thread, out int status);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static void SDL_KillThread(IntPtr thread);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_GetTicks();

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static void SDL_Delay(int ms);

    [SuppressUnmanagedCodeSecurity]
    [CLSCompliant(false)]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static void SDL_Delay(uint ms);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_SetTimer(int interval, Sdl.SDL_TimerCallback callback);

    [SuppressUnmanagedCodeSecurity]
    [CLSCompliant(false)]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_SetTimer(uint interval, Sdl.SDL_TimerCallback callback);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static Sdl.SDL_TimerID SDL_AddTimer(int interval, Sdl.SDL_NewTimerCallback callback);

    [SuppressUnmanagedCodeSecurity]
    [CLSCompliant(false)]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static Sdl.SDL_TimerID SDL_AddTimer(uint interval, Sdl.SDL_NewTimerCallback callback);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_RemoveTimer(Sdl.SDL_TimerID t);

    [CLSCompliant(false)]
    public static Sdl.SDL_version SDL_VERSION()
    {
      return new Sdl.SDL_version()
      {
        major = (byte) 1,
        minor = (byte) 2,
        patch = (byte) 11
      };
    }

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", EntryPoint = "SDL_Linked_Version", CallingConvention = CallingConvention.Cdecl)]
    private static IntPtr SDL_Linked_VersionInternal();

    public static Sdl.SDL_version SDL_Linked_Version()
    {
      return (Sdl.SDL_version) Marshal.PtrToStructure(Sdl.SDL_Linked_VersionInternal(), typeof (Sdl.SDL_version));
    }

    private static int SDL_VERSIONNUM(byte major, byte minor, byte patch)
    {
      return (int) major * 1000 + (int) minor * 100 + (int) patch;
    }

    public static bool SDL_VERSION_ATLEAST(byte major, byte minor, byte patch)
    {
      return Sdl.SDL_COMPILEDVERSION >= Sdl.SDL_VERSIONNUM(major, minor, patch);
    }

    public static int SDL_MUSTLOCK(IntPtr surface)
    {
      return (((Sdl.SDL_Surface) Marshal.PtrToStructure(surface, typeof (Sdl.SDL_Surface))).flags & 16389) != 0 ? 1 : 0;
    }

    public static string SDL_VideoDriverName(string namebuf, int maxlen)
    {
      StringBuilder namebuf1 = new StringBuilder(namebuf);
      Sdl.__SDL_VideoDriverName(namebuf1, maxlen);
      return ((object) namebuf1).ToString();
    }

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", EntryPoint = "SDL_VideoDriverName", CallingConvention = CallingConvention.Cdecl)]
    private static IntPtr __SDL_VideoDriverName(StringBuilder namebuf, int maxlen);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static IntPtr SDL_GetVideoSurface();

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static IntPtr SDL_GetVideoInfo();

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_VideoModeOK(int width, int height, int bpp, int flags);

    [SuppressUnmanagedCodeSecurity]
    [CLSCompliant(false)]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_VideoModeOK(int width, int height, int bpp, uint flags);

    [CLSCompliant(false)]
    public static unsafe Sdl.SDL_Rect[] SDL_ListModes(IntPtr format, int flags)
    {
      IntPtr num = Sdl.SDL_ListModesInternal(format, flags);
      if (num == IntPtr.Zero)
        return (Sdl.SDL_Rect[]) null;
      if (num == new IntPtr(-1))
        return new Sdl.SDL_Rect[0];
      Sdl.SDL_Rect** sdlRectPtr = (Sdl.SDL_Rect**) num.ToPointer();
      int index = 0;
      ArrayList arrayList = new ArrayList();
      for (; (IntPtr) sdlRectPtr[index] != IntPtr.Zero; ++index)
      {
        Sdl.SDL_Rect sdlRect = (Sdl.SDL_Rect) Marshal.PtrToStructure(new IntPtr((void*) sdlRectPtr[index]), typeof (Sdl.SDL_Rect));
        arrayList.Insert(0, (object) sdlRect);
      }
      return (Sdl.SDL_Rect[]) arrayList.ToArray(typeof (Sdl.SDL_Rect));
    }

    [CLSCompliant(false)]
    public static unsafe Sdl.SDL_Rect[] SDL_ListModes(IntPtr format, uint flags)
    {
      IntPtr num = Sdl.SDL_ListModesInternal(format, flags);
      if (num == IntPtr.Zero)
        return (Sdl.SDL_Rect[]) null;
      if (num == new IntPtr(-1))
        return new Sdl.SDL_Rect[0];
      Sdl.SDL_Rect** sdlRectPtr = (Sdl.SDL_Rect**) num.ToPointer();
      int index = 0;
      ArrayList arrayList = new ArrayList();
      for (; (IntPtr) sdlRectPtr[index] != IntPtr.Zero; ++index)
      {
        Sdl.SDL_Rect sdlRect = (Sdl.SDL_Rect) Marshal.PtrToStructure(new IntPtr((void*) sdlRectPtr[index]), typeof (Sdl.SDL_Rect));
        arrayList.Insert(0, (object) sdlRect);
      }
      return (Sdl.SDL_Rect[]) arrayList.ToArray(typeof (Sdl.SDL_Rect));
    }

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", EntryPoint = "SDL_ListModes", CallingConvention = CallingConvention.Cdecl)]
    private static IntPtr SDL_ListModesInternal(IntPtr format, int flags);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", EntryPoint = "SDL_ListModes", CallingConvention = CallingConvention.Cdecl)]
    private static IntPtr SDL_ListModesInternal(IntPtr format, uint flags);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static IntPtr SDL_SetVideoMode(int width, int height, int bpp, int flags);

    [SuppressUnmanagedCodeSecurity]
    [CLSCompliant(false)]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static IntPtr SDL_SetVideoMode(int width, int height, int bpp, uint flags);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static void SDL_UpdateRects(IntPtr screen, int numrects, [In, Out] Sdl.SDL_Rect[] rects);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static void SDL_UpdateRect(IntPtr screen, int x, int y, int w, int h);

    [SuppressUnmanagedCodeSecurity]
    [CLSCompliant(false)]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static void SDL_UpdateRect(IntPtr screen, int x, int y, uint w, uint h);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_Flip(IntPtr screen);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_SetGamma(float red, float green, float blue);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_SetGammaRamp([In, Out] short[] red, [In, Out] short[] green, [In, Out] short[] blue);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_GetGammaRamp([In, Out] short[] red, [In, Out] short[] green, [In, Out] short[] blue);

    [SuppressUnmanagedCodeSecurity]
    [CLSCompliant(false)]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_GetGammaRamp([In, Out] ushort[] red, [In, Out] ushort[] green, [In, Out] ushort[] blue);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_SetColors(IntPtr surface, [In, Out] Sdl.SDL_Color[] colors, int firstcolor, int ncolors);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_SetPalette(IntPtr surface, int flags, [In, Out] Sdl.SDL_Color[] colors, int firstcolor, int ncolors);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_MapRGB(IntPtr format, byte r, byte g, byte b);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_MapRGBA(IntPtr format, byte r, byte g, byte b, byte a);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static void SDL_GetRGB(int pixel, IntPtr fmt, out byte r, out byte g, out byte b);

    [SuppressUnmanagedCodeSecurity]
    [CLSCompliant(false)]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static void SDL_GetRGB(uint pixel, IntPtr fmt, out byte r, out byte g, out byte b);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static void SDL_GetRGBA(int pixel, IntPtr fmt, out byte r, out byte g, out byte b, out byte a);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static IntPtr SDL_CreateRGBSurface(int flags, int width, int height, int depth, int Rmask, int Gmask, int Bmask, int Amask);

    [SuppressUnmanagedCodeSecurity]
    [CLSCompliant(false)]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static IntPtr SDL_CreateRGBSurface(uint flags, int width, int height, int depth, uint Rmask, uint Gmask, uint Bmask, uint Amask);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", EntryPoint = "SDL_CreateRGBSurface", CallingConvention = CallingConvention.Cdecl)]
    public static IntPtr SDL_AllocSurface(int flags, int width, int height, int depth, int Rmask, int Gmask, int Bmask, int Amask);

    [SuppressUnmanagedCodeSecurity]
    [CLSCompliant(false)]
    [DllImport("SDL.dll", EntryPoint = "SDL_CreateRGBSurface", CallingConvention = CallingConvention.Cdecl)]
    public static IntPtr SDL_AllocSurface(uint flags, int width, int height, int depth, uint Rmask, uint Gmask, uint Bmask, uint Amask);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static IntPtr SDL_CreateRGBSurfaceFrom(IntPtr pixels, int width, int height, int depth, int pitch, int Rmask, int Gmask, int Bmask, int Amask);

    [SuppressUnmanagedCodeSecurity]
    [CLSCompliant(false)]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static IntPtr SDL_CreateRGBSurfaceFrom(IntPtr pixels, int width, int height, int depth, int pitch, uint Rmask, uint Gmask, uint Bmask, uint Amask);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static void SDL_FreeSurface(IntPtr surface);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_LockSurface(IntPtr surface);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_UnlockSurface(IntPtr surface);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static IntPtr SDL_LoadBMP_RW(IntPtr src, int freesrc);

    public static IntPtr SDL_LoadBMP(string file)
    {
      return Sdl.SDL_LoadBMP_RW(Sdl.SDL_RWFromFile(file, "rb"), 1);
    }

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_SaveBMP_RW(IntPtr surface, IntPtr dst, int freedst);

    public static int SDL_SaveBMP(IntPtr surface, string file)
    {
      return Sdl.SDL_SaveBMP_RW(surface, Sdl.SDL_RWFromFile(file, "wb"), 1);
    }

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_SetColorKey(IntPtr surface, int flag, int key);

    [SuppressUnmanagedCodeSecurity]
    [CLSCompliant(false)]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_SetColorKey(IntPtr surface, uint flag, uint key);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_SetAlpha(IntPtr surface, int flag, byte alpha);

    [SuppressUnmanagedCodeSecurity]
    [CLSCompliant(false)]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_SetAlpha(IntPtr surface, uint flag, byte alpha);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static void SDL_SetClipRect(IntPtr surface, ref Sdl.SDL_Rect rect);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static void SDL_GetClipRect(IntPtr surface, ref Sdl.SDL_Rect rect);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static IntPtr SDL_ConvertSurface(IntPtr src, IntPtr fmt, int flags);

    [SuppressUnmanagedCodeSecurity]
    [CLSCompliant(false)]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static IntPtr SDL_ConvertSurface(IntPtr src, IntPtr fmt, uint flags);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", EntryPoint = "SDL_UpperBlit", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_BlitSurface(IntPtr src, ref Sdl.SDL_Rect srcrect, IntPtr dst, ref Sdl.SDL_Rect dstrect);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_FillRect(IntPtr surface, ref Sdl.SDL_Rect rect, int color);

    [SuppressUnmanagedCodeSecurity]
    [CLSCompliant(false)]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_FillRect(IntPtr surface, ref Sdl.SDL_Rect rect, uint color);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static IntPtr SDL_DisplayFormat(IntPtr surface);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static IntPtr SDL_DisplayFormatAlpha(IntPtr surface);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static IntPtr SDL_CreateYUVOverlay(int width, int height, int format, IntPtr display);

    [SuppressUnmanagedCodeSecurity]
    [CLSCompliant(false)]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static IntPtr SDL_CreateYUVOverlay(int width, int height, uint format, IntPtr display);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_LockYUVOverlay(IntPtr overlay);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static void SDL_UnlockYUVOverlay(IntPtr overlay);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_DisplayYUVOverlay(IntPtr overlay, ref Sdl.SDL_Rect dstrect);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static void SDL_FreeYUVOverlay(IntPtr overlay);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_GL_LoadLibrary(string path);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static IntPtr SDL_GL_GetProcAddress(string proc);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static void SDL_GL_SwapBuffers();

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_GL_SetAttribute(int attr, int val);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_GL_GetAttribute(int attr, out int val);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static void SDL_WM_SetCaption(string title, string icon);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static void SDL_WM_GetCaption(out string title, out string icon);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static void SDL_WM_SetIcon(IntPtr icon, byte[] mask);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_WM_IconifyWindow();

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_WM_ToggleFullScreen(IntPtr surface);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDL_WM_GrabInput(int mode);

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct SDL_AudioSpec
    {
      public int freq;
      public short format;
      public byte channels;
      public byte silence;
      public short samples;
      public short padding;
      public int size;
      public IntPtr callback;
      public object userdata;
    }

    public struct SDL_AudioCVT
    {
      public int needed;
      public short src_format;
      public short dst_format;
      public double rate_incr;
      public IntPtr buf;
      public int len;
      public int len_cvt;
      public int len_mult;
      public double len_ratio;
      public int filter_index;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct SDL_CDtrack
    {
      public byte id;
      public byte type;
      public short unused;
      public int length;
      public int offset;
    }

    [StructLayout(LayoutKind.Sequential)]
    public class SDL_CD
    {
      public int id;
      public int status;
      public int numtracks;
      public int cur_track;
      public int cur_frame;
      public Sdl.SDL_CDTrackData track;

      public SDL_CD()
      {
        this.track = new Sdl.SDL_CDTrackData();
      }
    }

    [StructLayout(LayoutKind.Sequential)]
    public class SDL_CDTrackData
    {
      [MarshalAs(UnmanagedType.ByValArray, SizeConst = 792)]
      private byte[] trackData;

      public Sdl.SDL_CDtrack this[int index]
      {
        get
        {
          if (index < 0 | index >= 99)
            throw new IndexOutOfRangeException();
          GCHandle gcHandle = GCHandle.Alloc((object) this.trackData, GCHandleType.Pinned);
          try
          {
            return (Sdl.SDL_CDtrack) Marshal.PtrToStructure((IntPtr) (gcHandle.AddrOfPinnedObject().ToInt32() + index * Marshal.SizeOf(typeof (Sdl.SDL_CDtrack))), typeof (Sdl.SDL_CDtrack));
          }
          finally
          {
            gcHandle.Free();
          }
        }
      }

      public SDL_CDTrackData()
      {
        this.trackData = new byte[99 * Marshal.SizeOf(typeof (Sdl.SDL_CDtrack))];
      }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct SDL_ActiveEvent
    {
      public byte type;
      public byte gain;
      public byte state;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct SDL_KeyboardEvent
    {
      public byte type;
      public byte which;
      public byte state;
      public Sdl.SDL_keysym keysym;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct SDL_MouseMotionEvent
    {
      public byte type;
      public byte which;
      public byte state;
      public short x;
      public short y;
      public short xrel;
      public short yrel;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct SDL_MouseButtonEvent
    {
      public byte type;
      public byte which;
      public byte button;
      public byte state;
      public short x;
      public short y;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct SDL_JoyAxisEvent
    {
      public byte type;
      public byte which;
      public byte axis;
      public short val;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct SDL_JoyBallEvent
    {
      public byte type;
      public byte which;
      public byte ball;
      public short xrel;
      public short yrel;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct SDL_JoyHatEvent
    {
      public byte type;
      public byte which;
      public byte hat;
      public byte val;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct SDL_JoyButtonEvent
    {
      public byte type;
      public byte which;
      public byte button;
      public byte state;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct SDL_ResizeEvent
    {
      public byte type;
      public int w;
      public int h;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct SDL_ExposeEvent
    {
      public byte type;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct SDL_QuitEvent
    {
      public byte type;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct SDL_UserEvent
    {
      public byte type;
      public int code;
      public IntPtr data1;
      public IntPtr data2;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct SDL_SysWMEvent
    {
      public byte type;
      public IntPtr msg;
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct SDL_Event
    {
      [FieldOffset(0)]
      public byte type;
      [FieldOffset(0)]
      public Sdl.SDL_ActiveEvent active;
      [FieldOffset(0)]
      public Sdl.SDL_KeyboardEvent key;
      [FieldOffset(0)]
      public Sdl.SDL_MouseMotionEvent motion;
      [FieldOffset(0)]
      public Sdl.SDL_MouseButtonEvent button;
      [FieldOffset(0)]
      public Sdl.SDL_JoyAxisEvent jaxis;
      [FieldOffset(0)]
      public Sdl.SDL_JoyBallEvent jball;
      [FieldOffset(0)]
      public Sdl.SDL_JoyHatEvent jhat;
      [FieldOffset(0)]
      public Sdl.SDL_JoyButtonEvent jbutton;
      [FieldOffset(0)]
      public Sdl.SDL_ResizeEvent resize;
      [FieldOffset(0)]
      public Sdl.SDL_ExposeEvent expose;
      [FieldOffset(0)]
      public Sdl.SDL_QuitEvent quit;
      [FieldOffset(0)]
      public Sdl.SDL_UserEvent user;
      [FieldOffset(0)]
      public Sdl.SDL_SysWMEvent syswm;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct SDL_keysym
    {
      public byte scancode;
      public int sym;
      public int mod;
      public short unicode;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct SDL_Cursor
    {
      public Sdl.SDL_Rect area;
      public short hot_x;
      public short hot_y;
      public IntPtr data;
      public IntPtr mask;
      public IntPtr[] save;
      public IntPtr wm_cursor;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct SDL_SysWMmsg_Unix
    {
      public Sdl.SDL_version version;
      public int subsystem;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct SDL_SysWMinfo_Unix
    {
      public Sdl.SDL_version version;
      public int subsystem;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct SDL_SysWMmsg_Windows
    {
      public Sdl.SDL_version version;
      public int hwnd;
      public int msg;
      public int wParam;
      public IntPtr lParam;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct SDL_SysWMinfo_Windows
    {
      public Sdl.SDL_version version;
      public int window;
      public int hglrc;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct SDL_SysWMmsg_RiscOS
    {
      public Sdl.SDL_version version;
      public int eventCode;
      public int[] pollBlock;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct SDL_SysWMinfo_RiscOS
    {
      public Sdl.SDL_version version;
      public int wimpVersion;
      public int taskHandle;
      public int window;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct SDL_SysWMmsg
    {
      public Sdl.SDL_version version;
      public int data;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct SDL_SysWMinfo
    {
      public Sdl.SDL_version version;
      public int data;
    }

    [StructLayout(LayoutKind.Sequential, Size = 1, Pack = 4)]
    public struct SDL_TimerID
    {
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct SDL_version
    {
      public byte major;
      public byte minor;
      public byte patch;

      public override string ToString()
      {
        object[] objArray = new object[5];
        int index1 = 0;
        // ISSUE: variable of a boxed type
        __Boxed<byte> local1 = (ValueType) this.major;
        objArray[index1] = (object) local1;
        int index2 = 1;
        string str1 = ".";
        objArray[index2] = (object) str1;
        int index3 = 2;
        // ISSUE: variable of a boxed type
        __Boxed<byte> local2 = (ValueType) this.minor;
        objArray[index3] = (object) local2;
        int index4 = 3;
        string str2 = ".";
        objArray[index4] = (object) str2;
        int index5 = 4;
        // ISSUE: variable of a boxed type
        __Boxed<byte> local3 = (ValueType) this.patch;
        objArray[index5] = (object) local3;
        return string.Concat(objArray);
      }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct SDL_Rect
    {
      public short x;
      public short y;
      public short w;
      public short h;

      public SDL_Rect(short x, short y, short w, short h)
      {
        this.x = x;
        this.y = y;
        this.w = w;
        this.h = h;
      }

      public override string ToString()
      {
        object[] objArray = new object[8];
        int index1 = 0;
        string str1 = "x: ";
        objArray[index1] = (object) str1;
        int index2 = 1;
        // ISSUE: variable of a boxed type
        __Boxed<short> local1 = (ValueType) this.x;
        objArray[index2] = (object) local1;
        int index3 = 2;
        string str2 = ", y: ";
        objArray[index3] = (object) str2;
        int index4 = 3;
        // ISSUE: variable of a boxed type
        __Boxed<short> local2 = (ValueType) this.y;
        objArray[index4] = (object) local2;
        int index5 = 4;
        string str3 = ", w: ";
        objArray[index5] = (object) str3;
        int index6 = 5;
        // ISSUE: variable of a boxed type
        __Boxed<short> local3 = (ValueType) this.w;
        objArray[index6] = (object) local3;
        int index7 = 6;
        string str4 = ", h: ";
        objArray[index7] = (object) str4;
        int index8 = 7;
        // ISSUE: variable of a boxed type
        __Boxed<short> local4 = (ValueType) this.h;
        objArray[index8] = (object) local4;
        return string.Concat(objArray);
      }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct SDL_Color
    {
      public byte r;
      public byte g;
      public byte b;
      public byte unused;

      public SDL_Color(byte r, byte g, byte b)
      {
        this.r = r;
        this.g = g;
        this.b = b;
        this.unused = (byte) 0;
      }

      public SDL_Color(byte r, byte g, byte b, byte a)
      {
        this.r = r;
        this.g = g;
        this.b = b;
        this.unused = a;
      }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct SDL_Palette
    {
      public int ncolors;
      public Sdl.SDL_Color[] colors;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct SDL_PixelFormat
    {
      public IntPtr palette;
      public byte BitsPerPixel;
      public byte BytesPerPixel;
      public byte Rloss;
      public byte Gloss;
      public byte Bloss;
      public byte Aloss;
      public byte Rshift;
      public byte Gshift;
      public byte Bshift;
      public byte Ashift;
      public int Rmask;
      public int Gmask;
      public int Bmask;
      public int Amask;
      public int colorkey;
      public byte alpha;

      public SDL_PixelFormat(IntPtr palette, byte BitsPerPixel, byte BytesPerPixel, byte Rloss, byte Gloss, byte Bloss, byte Aloss, byte Rshift, byte Gshift, byte Bshift, byte Ashift, int Rmask, int Gmask, int Bmask, int Amask, int colorkey, byte alpha)
      {
        this.palette = (int) BitsPerPixel <= 8 ? palette : IntPtr.Zero;
        this.BitsPerPixel = BitsPerPixel;
        this.BytesPerPixel = BytesPerPixel;
        this.Rloss = Rloss;
        this.Gloss = Gloss;
        this.Bloss = Bloss;
        this.Aloss = Aloss;
        this.Rshift = Rshift;
        this.Gshift = Gshift;
        this.Bshift = Bshift;
        this.Ashift = Ashift;
        this.Rmask = Rmask;
        this.Gmask = Gmask;
        this.Bmask = Bmask;
        this.Amask = Amask;
        this.colorkey = colorkey;
        this.alpha = alpha;
      }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct SDL_Surface
    {
      public int flags;
      public IntPtr format;
      public int w;
      public int h;
      public short pitch;
      public IntPtr pixels;
      public int offset;
      public IntPtr hwdata;
      public Sdl.SDL_Rect clip_rect;
      public int unused1;
      public int locked;
      public IntPtr map;
      public int format_version;
      public int refcount;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct SDL_VideoInfo
    {
      public byte field1;
      public byte field2;
      public short unused;
      public int video_mem;
      public IntPtr vfmt;
      public int current_w;
      public int current_h;

      public int hw_available
      {
        get
        {
          return (int) this.field1 & 1;
        }
      }

      public int wm_available
      {
        get
        {
          return (int) this.field1 >> 1 & 1;
        }
      }

      public int blit_hw
      {
        get
        {
          return (int) this.field2 >> 1 & 1;
        }
      }

      public int blit_hw_CC
      {
        get
        {
          return (int) this.field2 >> 2 & 1;
        }
      }

      public int blit_hw_A
      {
        get
        {
          return (int) this.field2 >> 3 & 1;
        }
      }

      public int blit_sw
      {
        get
        {
          return (int) this.field2 >> 4 & 1;
        }
      }

      public int blit_sw_CC
      {
        get
        {
          return (int) this.field2 >> 5 & 1;
        }
      }

      public int blit_sw_A
      {
        get
        {
          return (int) this.field2 >> 6 & 1;
        }
      }

      public int blit_fill
      {
        get
        {
          return (int) this.field2 >> 7 & 1;
        }
      }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct SDL_Overlay
    {
      public int format;
      public int w;
      public int h;
      public int planes;
      public IntPtr pitches;
      public IntPtr pixels;
      public IntPtr hwfuncs;
      public IntPtr hwdata;
      public int field1;

      public int hw_overlay
      {
        get
        {
          return this.field1 & 1;
        }
      }
    }

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void AudioSpecCallbackDelegate(IntPtr userdata, IntPtr stream, int len);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int SDL_EventFilter([Out] Sdl.SDL_Event evt);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int ThreadDelegate();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int SDL_TimerCallback(int interval);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int SDL_NewTimerCallback(int interval);
  }
}
