﻿// Type: MonoGame.Framework.MonoLive.RegisterCompletedEventArgs
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;

namespace MonoGame.Framework.MonoLive
{
  [DesignerCategory("code")]
  [DebuggerStepThrough]
  [GeneratedCode("System.Web.Services", "4.0.30319.1")]
  public class RegisterCompletedEventArgs : AsyncCompletedEventArgs
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

    internal RegisterCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState)
      : base(exception, cancelled, userState)
    {
      this.results = results;
    }
  }
}
