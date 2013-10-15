// Type: Newtonsoft.Json.Serialization.ErrorContext
// Assembly: Newtonsoft.Json, Version=4.5.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: DB013E7B-84E7-4AE6-A132-56DC73318C48
// Assembly location: F:\Program Files (x86)\FEZ\Newtonsoft.Json.dll

using System;

namespace Newtonsoft.Json.Serialization
{
  public class ErrorContext
  {
    public Exception Error { get; private set; }

    public object OriginalObject { get; private set; }

    public object Member { get; private set; }

    public string Path { get; private set; }

    public bool Handled { get; set; }

    internal ErrorContext(object originalObject, object member, string path, Exception error)
    {
      this.OriginalObject = originalObject;
      this.Member = member;
      this.Error = error;
      this.Path = path;
    }
  }
}
