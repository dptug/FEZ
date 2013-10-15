// Type: SharpDX.Result
// Assembly: SharpDX, Version=2.4.2.0, Culture=neutral, PublicKeyToken=627a3d6d1956f55a
// MVID: 578390A1-1524-4146-8C27-2E9750400D7A
// Assembly location: F:\Program Files (x86)\FEZ\SharpDX.dll

using SharpDX.Serialization;
using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace SharpDX
{
  [Serializable]
  public struct Result : IEquatable<Result>, IDataSerializable
  {
    public static Result Ok = new Result(0);
    public static Result False = new Result(1);
    public static Result Abord = new Result(-2147467260);
    public static Result AccessDenied = new Result(-2147024891);
    public static Result Fail = new Result(-2147467259);
    public static Result Handle = new Result(-2147024890);
    public static Result InvalidArg = new Result(-2147024809);
    public static Result NoInterface = new Result(-2147467262);
    public static Result NotImplemented = new Result(-2147467263);
    public static Result OutOfMemory = new Result(-2147024882);
    public static Result InvalidPointer = new Result(-2147467261);
    public static Result UnexpectedFailure = new Result(-2147418113);
    private int _code;

    public int Code
    {
      get
      {
        return this._code;
      }
    }

    public bool Success
    {
      get
      {
        return this.Code >= 0;
      }
    }

    public bool Failure
    {
      get
      {
        return this.Code < 0;
      }
    }

    static Result()
    {
    }

    public Result(int code)
    {
      this._code = code;
    }

    public Result(uint code)
    {
      this._code = (int) code;
    }

    public static explicit operator int(Result result)
    {
      return result.Code;
    }

    public static explicit operator uint(Result result)
    {
      return (uint) result.Code;
    }

    public static implicit operator Result(int result)
    {
      return new Result(result);
    }

    public static implicit operator Result(uint result)
    {
      return new Result(result);
    }

    public static bool operator ==(Result left, Result right)
    {
      return left.Code == right.Code;
    }

    public static bool operator !=(Result left, Result right)
    {
      return left.Code != right.Code;
    }

    public bool Equals(Result other)
    {
      return this.Code == other.Code;
    }

    public override bool Equals(object obj)
    {
      if (!(obj is Result))
        return false;
      else
        return this.Equals((Result) obj);
    }

    public override int GetHashCode()
    {
      return this.Code;
    }

    void IDataSerializable.Serialize(BinarySerializer serializer)
    {
      serializer.Serialize(ref this._code);
    }

    public override string ToString()
    {
      return string.Format((IFormatProvider) CultureInfo.InvariantCulture, "HRESULT = 0x{0:X}", new object[1]
      {
        (object) this._code
      });
    }

    public void CheckError()
    {
      if (this._code < 0)
        throw new SharpDXException(this);
    }

    public static Result GetResultFromException(Exception ex)
    {
      return new Result(Marshal.GetHRForException(ex));
    }

    public static Result GetResultFromWin32Error(int win32Error)
    {
      return (Result) (win32Error <= 0 ? win32Error : (int) ((long) (win32Error & (int) ushort.MaxValue | 458752) | 2147483648L));
    }
  }
}
