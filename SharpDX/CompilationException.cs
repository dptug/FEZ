// Type: SharpDX.CompilationException
// Assembly: SharpDX, Version=2.4.2.0, Culture=neutral, PublicKeyToken=627a3d6d1956f55a
// MVID: 578390A1-1524-4146-8C27-2E9750400D7A
// Assembly location: F:\Program Files (x86)\FEZ\SharpDX.dll

namespace SharpDX
{
  public class CompilationException : SharpDXException
  {
    public CompilationException(string message)
      : base(message, new object[0])
    {
    }

    public CompilationException(Result errorCode, string message)
      : base(errorCode, message)
    {
    }
  }
}
