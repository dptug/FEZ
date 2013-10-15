// Type: Tao.Sdl.SdlNet
// Assembly: Tao.Sdl, Version=1.2.13.0, Culture=neutral, PublicKeyToken=9c7a200e36c0094e
// MVID: 45EBE10E-1CB9-425D-83FF-C3B8997BBF28
// Assembly location: F:\Program Files (x86)\FEZ\Tao.Sdl.dll

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace Tao.Sdl
{
  [SuppressUnmanagedCodeSecurity]
  public static class SdlNet
  {
    private const string SDL_NET_NATIVE_LIBRARY = "SDL_net.dll";
    private const CallingConvention CALLING_CONVENTION = CallingConvention.Cdecl;
    public const int SDL_NET_MAJOR_VERSION = 1;
    public const int SDL_NET_MINOR_VERSION = 2;
    public const int SDL_NET_PATCHLEVEL = 6;
    public const byte INADDR_ANY = (byte) 0;
    public const byte INADDR_NONE = (byte) 255;
    public const byte INADDR_BROADCAST = (byte) 255;
    public const byte SDLNET_MAX_UDPCHANNELS = (byte) 32;
    public const byte SDLNET_MAX_UDPADDRESSES = (byte) 4;

    public static Sdl.SDL_version SDL_NET_VERSION()
    {
      return new Sdl.SDL_version()
      {
        major = (byte) 1,
        minor = (byte) 2,
        patch = (byte) 6
      };
    }

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_net.dll", EntryPoint = "SDLNet_Linked_Version", CallingConvention = CallingConvention.Cdecl)]
    private static IntPtr SDLNet_Linked_VersionInternal();

    public static Sdl.SDL_version SDLNet_Linked_Version()
    {
      return (Sdl.SDL_version) Marshal.PtrToStructure(SdlNet.SDLNet_Linked_VersionInternal(), typeof (Sdl.SDL_version));
    }

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_net.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDLNet_Init();

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_net.dll", CallingConvention = CallingConvention.Cdecl)]
    public static void SDLNet_Quit();

    public static string SDLNet_GetError()
    {
      return Sdl.SDL_GetError();
    }

    public static void SDLNet_SetError(string message)
    {
      Sdl.SDL_SetError(message);
    }

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_net.dll", CallingConvention = CallingConvention.Cdecl)]
    public static void SDLNet_Write16(short value, IntPtr area);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_net.dll", CallingConvention = CallingConvention.Cdecl)]
    public static void SDLNet_Write32(int value, IntPtr area);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_net.dll", CallingConvention = CallingConvention.Cdecl)]
    public static short SDLNet_Read16(IntPtr area);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_net.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDLNet_Read32(IntPtr area);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_net.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDLNet_ResolveHost(ref SdlNet.IPaddress address, string host, short port);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_net.dll", CallingConvention = CallingConvention.Cdecl)]
    public static string SDLNet_ResolveIP(ref SdlNet.IPaddress address);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_net.dll", CallingConvention = CallingConvention.Cdecl)]
    public static IntPtr SDLNet_TCP_Open(ref SdlNet.IPaddress ip);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_net.dll", CallingConvention = CallingConvention.Cdecl)]
    public static IntPtr SDLNet_TCP_Accept(SdlNet.TCPsocket server);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_net.dll", CallingConvention = CallingConvention.Cdecl)]
    public static IntPtr SDLNet_TCP_GetPeerAddress(SdlNet.TCPsocket sock);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_net.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDLNet_TCP_Send(SdlNet.TCPsocket sock, IntPtr data, int len);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_net.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDLNet_TCP_Recv(SdlNet.TCPsocket sock, IntPtr data, int maxlen);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_net.dll", CallingConvention = CallingConvention.Cdecl)]
    public static void SDLNet_TCP_Close(IntPtr sock);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_net.dll", CallingConvention = CallingConvention.Cdecl)]
    public static IntPtr SDLNet_AllocPacket(int size);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_net.dll", CallingConvention = CallingConvention.Cdecl)]
    public static IntPtr SDLNet_ResizePacket(IntPtr packet, int newsize);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_net.dll", CallingConvention = CallingConvention.Cdecl)]
    public static IntPtr SDLNet_FreePacket(IntPtr packet);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_net.dll", CallingConvention = CallingConvention.Cdecl)]
    public static IntPtr SDLNet_AllocPacketV(int howmany, int size);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_net.dll", CallingConvention = CallingConvention.Cdecl)]
    public static IntPtr SDLNet_FreePacketV(IntPtr packetV);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_net.dll", CallingConvention = CallingConvention.Cdecl)]
    public static IntPtr SDLNet_UDP_Open(short port);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_net.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDLNet_UDP_Bind(IntPtr sock, int channel, ref SdlNet.IPaddress address);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_net.dll", CallingConvention = CallingConvention.Cdecl)]
    public static void SDLNet_UDP_Unbind(IntPtr sock, int channel);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_net.dll", CallingConvention = CallingConvention.Cdecl)]
    public static IntPtr SDLNet_UDP_GetPeerAddress(IntPtr sock, int channel);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_net.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDLNet_UDP_SendV(IntPtr sock, IntPtr packets, int npackets);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_net.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDLNet_UDP_Send(IntPtr sock, int channel, IntPtr packet);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_net.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDLNet_UDP_RecvV(IntPtr sock, IntPtr packets);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_net.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDLNet_UDP_Recv(IntPtr sock, IntPtr packet);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_net.dll", CallingConvention = CallingConvention.Cdecl)]
    public static void SDLNet_UDP_Close(IntPtr sock);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_net.dll", CallingConvention = CallingConvention.Cdecl)]
    public static SdlNet.SDLNet_SocketSet SDLNet_AllocSocketSet(int maxsockets);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_net.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDLNet_AddSocket(SdlNet.SDLNet_SocketSet set, SdlNet.SDLNet_GenericSocket sock);

    public static int SDLNet_TCP_AddSocket(SdlNet.SDLNet_SocketSet set, IntPtr sock)
    {
      SdlNet.SDLNet_GenericSocket sock1 = (SdlNet.SDLNet_GenericSocket) Marshal.PtrToStructure(sock, typeof (SdlNet.SDLNet_GenericSocket));
      return SdlNet.SDLNet_AddSocket(set, sock1);
    }

    public static int SDLNet_UDP_AddSocket(SdlNet.SDLNet_SocketSet set, IntPtr sock)
    {
      SdlNet.SDLNet_GenericSocket sock1 = (SdlNet.SDLNet_GenericSocket) Marshal.PtrToStructure(sock, typeof (SdlNet.SDLNet_GenericSocket));
      return SdlNet.SDLNet_AddSocket(set, sock1);
    }

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_net.dll", CallingConvention = CallingConvention.Cdecl)]
    public static void SDLNet_DelSocket(SdlNet.SDLNet_SocketSet set, SdlNet.SDLNet_GenericSocket sock);

    public static void SDLNet_TCP_DelSocket(SdlNet.SDLNet_SocketSet set, IntPtr sock)
    {
      SdlNet.SDLNet_GenericSocket sock1 = (SdlNet.SDLNet_GenericSocket) Marshal.PtrToStructure(sock, typeof (SdlNet.SDLNet_GenericSocket));
      SdlNet.SDLNet_DelSocket(set, sock1);
    }

    public static void SDLNet_UDP_DelSocket(SdlNet.SDLNet_SocketSet set, IntPtr sock)
    {
      SdlNet.SDLNet_GenericSocket sock1 = (SdlNet.SDLNet_GenericSocket) Marshal.PtrToStructure(sock, typeof (SdlNet.SDLNet_GenericSocket));
      SdlNet.SDLNet_DelSocket(set, sock1);
    }

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_net.dll", CallingConvention = CallingConvention.Cdecl)]
    public static int SDLNet_CheckSockets(SdlNet.SDLNet_SocketSet set, int timeout);

    public static int SDLNet_SocketReady(IntPtr sock)
    {
      if (sock != IntPtr.Zero)
        return ((SdlNet.SDLNet_GenericSocket) Marshal.PtrToStructure(sock, typeof (SdlNet.SDLNet_GenericSocket))).ready;
      else
        return 0;
    }

    [SuppressUnmanagedCodeSecurity]
    [DllImport("SDL_net.dll", CallingConvention = CallingConvention.Cdecl)]
    public static void SDLNet_FreeSocketSet(SdlNet.SDLNet_SocketSet set);

    public struct IPaddress
    {
      public int host;
      public short port;
    }

    public struct UDPpacket
    {
      public int channel;
      public IntPtr data;
      public int len;
      public int maxlen;
      public int status;
      public SdlNet.IPaddress address;
    }

    [StructLayout(LayoutKind.Sequential, Size = 1)]
    public struct TCPsocket
    {
    }

    [StructLayout(LayoutKind.Sequential, Size = 1)]
    public struct UDPsocket
    {
    }

    [StructLayout(LayoutKind.Sequential, Size = 1)]
    public struct SDLNet_SocketSet
    {
    }

    public struct SDLNet_GenericSocket
    {
      public int ready;
    }
  }
}
