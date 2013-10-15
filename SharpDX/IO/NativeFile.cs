// Type: SharpDX.IO.NativeFile
// Assembly: SharpDX, Version=2.4.2.0, Culture=neutral, PublicKeyToken=627a3d6d1956f55a
// MVID: 578390A1-1524-4146-8C27-2E9750400D7A
// Assembly location: F:\Program Files (x86)\FEZ\SharpDX.dll

using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace SharpDX.IO
{
  public static class NativeFile
  {
    public static byte[] ReadAllBytes(string path)
    {
      byte[] buffer;
      using (NativeFileStream nativeFileStream = new NativeFileStream(path, NativeFileMode.Open, NativeFileAccess.Read, NativeFileShare.Read))
      {
        int offset = 0;
        long length = nativeFileStream.Length;
        if (length > (long) int.MaxValue)
          throw new IOException("File too long");
        int count = (int) length;
        buffer = new byte[count];
        while (count > 0)
        {
          int num = ((Stream) nativeFileStream).Read(buffer, offset, count);
          if (num == 0)
            throw new EndOfStreamException();
          offset += num;
          count -= num;
        }
      }
      return buffer;
    }

    public static string ReadAllText(string path)
    {
      return NativeFile.ReadAllText(path, Encoding.UTF8);
    }

    public static string ReadAllText(string path, Encoding encoding)
    {
      using (NativeFileStream nativeFileStream = new NativeFileStream(path, NativeFileMode.Open, NativeFileAccess.Read, NativeFileShare.Read))
      {
        using (StreamReader streamReader = new StreamReader((Stream) nativeFileStream, encoding, true, 1024))
          return streamReader.ReadToEnd();
      }
    }

    [DllImport("kernel32.dll", CharSet = CharSet.Ansi, SetLastError = true)]
    internal static bool ReadFile(IntPtr fileHandle, IntPtr buffer, int numberOfBytesToRead, out int numberOfBytesRead, IntPtr overlapped);

    [DllImport("kernel32.dll", SetLastError = true)]
    internal static bool FlushFileBuffers(IntPtr hFile);

    [DllImport("kernel32.dll", CharSet = CharSet.Ansi, SetLastError = true)]
    internal static bool WriteFile(IntPtr fileHandle, IntPtr buffer, int numberOfBytesToRead, out int numberOfBytesRead, IntPtr overlapped);

    [DllImport("kernel32.dll", CharSet = CharSet.Ansi, SetLastError = true)]
    internal static bool SetFilePointerEx(IntPtr handle, long distanceToMove, out long distanceToMoveHigh, SeekOrigin seekOrigin);

    [DllImport("kernel32.dll", CharSet = CharSet.Ansi, SetLastError = true)]
    internal static bool SetEndOfFile(IntPtr handle);

    [DllImport("kernel32.dll", EntryPoint = "CreateFile", CharSet = CharSet.Ansi, SetLastError = true)]
    internal static IntPtr Create(string fileName, NativeFileAccess desiredAccess, NativeFileShare shareMode, IntPtr securityAttributes, NativeFileMode mode, NativeFileOptions flagsAndOptions, IntPtr templateFile);

    [DllImport("kernel32.dll", CharSet = CharSet.Ansi, SetLastError = true)]
    internal static bool GetFileSizeEx(IntPtr handle, out long fileSize);
  }
}
