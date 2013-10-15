// Type: OpenTK.Graphics.ES11.ErrorHelper
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using OpenTK.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace OpenTK.Graphics.ES11
{
  internal struct ErrorHelper : IDisposable
  {
    private static readonly object SyncRoot = new object();
    private static readonly Dictionary<GraphicsContext, List<ErrorCode>> ContextErrors = new Dictionary<GraphicsContext, List<ErrorCode>>();
    private readonly GraphicsContext Context;

    static ErrorHelper()
    {
    }

    public ErrorHelper(IGraphicsContext context)
    {
      if (context == null)
        throw new GraphicsContextMissingException();
      this.Context = (GraphicsContext) context;
      lock (ErrorHelper.SyncRoot)
      {
        if (ErrorHelper.ContextErrors.ContainsKey(this.Context))
          return;
        ErrorHelper.ContextErrors.Add(this.Context, new List<ErrorCode>());
      }
    }

    [Conditional("DEBUG")]
    internal void ResetErrors()
    {
      if (!this.Context.ErrorChecking)
        return;
      do
        ;
      while (GL.GetError() != All.False);
    }

    [Conditional("DEBUG")]
    internal void CheckErrors()
    {
      if (!this.Context.ErrorChecking)
        return;
      List<ErrorCode> list = ErrorHelper.ContextErrors[this.Context];
      list.Clear();
      ErrorCode errorCode1;
      do
      {
        errorCode1 = (ErrorCode) GL.GetError();
        list.Add(errorCode1);
      }
      while (errorCode1 != ErrorCode.NoError);
      if (list.Count == 1)
        return;
      StringBuilder stringBuilder = new StringBuilder();
      foreach (ErrorCode errorCode2 in list)
      {
        if (errorCode2 != ErrorCode.NoError)
        {
          stringBuilder.Append(((object) errorCode2).ToString());
          stringBuilder.Append(", ");
        }
        else
          break;
      }
      stringBuilder.Remove(stringBuilder.Length - 2, 2);
      throw new GraphicsErrorException(((object) stringBuilder).ToString());
    }

    public void Dispose()
    {
    }
  }
}
