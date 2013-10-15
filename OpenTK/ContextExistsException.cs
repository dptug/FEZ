// Type: OpenTK.ContextExistsException
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using System;

namespace OpenTK
{
  public class ContextExistsException : ApplicationException
  {
    private string msg;

    public override string Message
    {
      get
      {
        return this.msg;
      }
    }

    public ContextExistsException(string message)
    {
      this.msg = message;
    }
  }
}
