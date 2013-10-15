// Type: SharpDX.SharpDXException
// Assembly: SharpDX, Version=2.4.2.0, Culture=neutral, PublicKeyToken=627a3d6d1956f55a
// MVID: 578390A1-1524-4146-8C27-2E9750400D7A
// Assembly location: F:\Program Files (x86)\FEZ\SharpDX.dll

using System;
using System.Globalization;

namespace SharpDX
{
  [Serializable]
  public class SharpDXException : Exception
  {
    private ResultDescriptor descriptor;

    public Result ResultCode
    {
      get
      {
        return this.descriptor.Result;
      }
    }

    public ResultDescriptor Descriptor
    {
      get
      {
        return this.descriptor;
      }
    }

    public SharpDXException()
      : base("A SharpDX exception occurred.")
    {
      this.descriptor = ResultDescriptor.Find(Result.Fail);
      this.HResult = (int) Result.Fail;
    }

    public SharpDXException(Result result)
      : this(ResultDescriptor.Find(result))
    {
      this.HResult = (int) result;
    }

    public SharpDXException(ResultDescriptor descriptor)
      : base(descriptor.ToString())
    {
      this.descriptor = descriptor;
      this.HResult = (int) descriptor.Result;
    }

    public SharpDXException(Result result, string message)
      : base(message)
    {
      this.descriptor = ResultDescriptor.Find(result);
      this.HResult = (int) result;
    }

    public SharpDXException(Result result, string message, params object[] args)
      : base(string.Format((IFormatProvider) CultureInfo.InvariantCulture, message, args))
    {
      this.descriptor = ResultDescriptor.Find(result);
      this.HResult = (int) result;
    }

    public SharpDXException(string message, params object[] args)
      : this(Result.Fail, message, args)
    {
    }

    public SharpDXException(string message, Exception innerException, params object[] args)
      : base(string.Format((IFormatProvider) CultureInfo.InvariantCulture, message, args), innerException)
    {
      this.descriptor = ResultDescriptor.Find(Result.Fail);
      this.HResult = (int) Result.Fail;
    }
  }
}
