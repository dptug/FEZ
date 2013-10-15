// Type: SharpDX.CompilationResultBase`1
// Assembly: SharpDX, Version=2.4.2.0, Culture=neutral, PublicKeyToken=627a3d6d1956f55a
// MVID: 578390A1-1524-4146-8C27-2E9750400D7A
// Assembly location: F:\Program Files (x86)\FEZ\SharpDX.dll

using System;

namespace SharpDX
{
  public abstract class CompilationResultBase<T> : DisposeBase where T : class, IDisposable
  {
    public T Bytecode { get; private set; }

    public Result ResultCode { get; private set; }

    public bool HasErrors
    {
      get
      {
        return this.ResultCode.Failure;
      }
    }

    public string Message { get; private set; }

    protected CompilationResultBase(T bytecode, Result resultCode, string message = null)
    {
      this.Bytecode = bytecode;
      this.ResultCode = resultCode;
      this.Message = message;
    }

    protected override void Dispose(bool disposing)
    {
      if (!disposing || (object) this.Bytecode == null)
        return;
      this.Bytecode.Dispose();
      this.Bytecode = default (T);
    }
  }
}
