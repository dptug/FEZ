// Type: MonoGame.Framework.MonoLive.SignInCompletedEventArgs
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;

namespace MonoGame.Framework.MonoLive
{
  [DesignerCategory("code")]
  [DebuggerStepThrough]
  [GeneratedCode("System.Web.Services", "4.0.30319.1")]
  public class SignInCompletedEventArgs : AsyncCompletedEventArgs
  {
    private object[] results;

    public Result Result
    {
      get
      {
        this.RaiseExceptionIfNecessary();
        return (Result) this.results[0];
      }
    }

    internal SignInCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState)
      : base(exception, cancelled, userState)
    {
      this.results = results;
    }
  }
}
