// Type: SharpDX.Direct3D.ShaderMacro
// Assembly: SharpDX, Version=2.4.2.0, Culture=neutral, PublicKeyToken=627a3d6d1956f55a
// MVID: 578390A1-1524-4146-8C27-2E9750400D7A
// Assembly location: F:\Program Files (x86)\FEZ\SharpDX.dll

using SharpDX;
using System;
using System.Runtime.InteropServices;

namespace SharpDX.Direct3D
{
  public struct ShaderMacro : IEquatable<ShaderMacro>
  {
    public string Name;
    public string Definition;

    public ShaderMacro(string name, object definition)
    {
      this.Name = name;
      this.Definition = definition == null ? (string) null : definition.ToString();
    }

    public static bool operator ==(ShaderMacro left, ShaderMacro right)
    {
      return left.Equals(right);
    }

    public static bool operator !=(ShaderMacro left, ShaderMacro right)
    {
      return !left.Equals(right);
    }

    internal void __MarshalFree(ref ShaderMacro.__Native @ref)
    {
      @ref.__MarshalFree();
    }

    internal void __MarshalFrom(ref ShaderMacro.__Native @ref)
    {
      this.Name = @ref.Name == IntPtr.Zero ? (string) null : Marshal.PtrToStringAnsi(@ref.Name);
      this.Definition = @ref.Definition == IntPtr.Zero ? (string) null : Marshal.PtrToStringAnsi(@ref.Definition);
    }

    internal void __MarshalTo(ref ShaderMacro.__Native @ref)
    {
      @ref.Name = this.Name == null ? IntPtr.Zero : Utilities.StringToHGlobalAnsi(this.Name);
      @ref.Definition = this.Definition == null ? IntPtr.Zero : Utilities.StringToHGlobalAnsi(this.Definition);
    }

    public bool Equals(ShaderMacro other)
    {
      if (string.Equals(this.Name, other.Name))
        return string.Equals(this.Definition, other.Definition);
      else
        return false;
    }

    public override bool Equals(object obj)
    {
      if (object.ReferenceEquals((object) null, obj) || !(obj is ShaderMacro))
        return false;
      else
        return this.Equals((ShaderMacro) obj);
    }

    public override int GetHashCode()
    {
      return (this.Name != null ? this.Name.GetHashCode() : 0) * 397 ^ (this.Definition != null ? this.Definition.GetHashCode() : 0);
    }

    internal struct __Native
    {
      public IntPtr Name;
      public IntPtr Definition;

      internal void __MarshalFree()
      {
        if (this.Name != IntPtr.Zero)
          Marshal.FreeHGlobal(this.Name);
        if (!(this.Definition != IntPtr.Zero))
          return;
        Marshal.FreeHGlobal(this.Definition);
      }
    }
  }
}
