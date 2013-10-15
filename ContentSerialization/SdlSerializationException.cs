// Type: ContentSerialization.SdlSerializationException
// Assembly: ContentSerialization, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 458E2D00-F22E-495B-9E0A-A944909C9571
// Assembly location: F:\Program Files (x86)\FEZ\ContentSerialization.dll

using System;
using System.Reflection;

namespace ContentSerialization
{
  public class SdlSerializationException : Exception
  {
    public SdlSerializationException(string message, Type type, string memberName)
      : base(message + "\nType : " + type.Name + "\nMember : " + memberName)
    {
    }

    public SdlSerializationException(string message, Type type, MemberInfo member)
      : this(message, type, member.Name)
    {
    }

    public SdlSerializationException(string message, Type type, string memberName, Exception cause)
      : base(message + "\nType : " + type.Name + "\nMember : " + memberName, cause)
    {
    }

    public SdlSerializationException(string message, Type type, MemberInfo member, Exception cause)
      : this(message, type, member.Name)
    {
    }
  }
}
