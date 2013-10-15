// Type: Newtonsoft.Json.Serialization.JsonContract
// Assembly: Newtonsoft.Json, Version=4.5.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: DB013E7B-84E7-4AE6-A132-56DC73318C48
// Assembly location: F:\Program Files (x86)\FEZ\Newtonsoft.Json.dll

using Newtonsoft.Json;
using Newtonsoft.Json.Utilities;
using System;
using System.Reflection;
using System.Runtime.Serialization;

namespace Newtonsoft.Json.Serialization
{
  public abstract class JsonContract
  {
    internal bool IsNullable;
    internal bool IsConvertable;
    internal Type NonNullableUnderlyingType;
    internal ReadType InternalReadType;
    internal JsonContractType ContractType;

    public Type UnderlyingType { get; private set; }

    public Type CreatedType { get; set; }

    public bool? IsReference { get; set; }

    public JsonConverter Converter { get; set; }

    internal JsonConverter InternalConverter { get; set; }

    public MethodInfo OnDeserialized { get; set; }

    public MethodInfo OnDeserializing { get; set; }

    public MethodInfo OnSerialized { get; set; }

    public MethodInfo OnSerializing { get; set; }

    public Func<object> DefaultCreator { get; set; }

    public bool DefaultCreatorNonPublic { get; set; }

    public MethodInfo OnError { get; set; }

    internal JsonContract(Type underlyingType)
    {
      ValidationUtils.ArgumentNotNull((object) underlyingType, "underlyingType");
      this.UnderlyingType = underlyingType;
      this.IsNullable = ReflectionUtils.IsNullable(underlyingType);
      this.NonNullableUnderlyingType = !this.IsNullable || !ReflectionUtils.IsNullableType(underlyingType) ? underlyingType : Nullable.GetUnderlyingType(underlyingType);
      this.CreatedType = this.NonNullableUnderlyingType;
      this.IsConvertable = ConvertUtils.IsConvertible(this.NonNullableUnderlyingType);
      if (this.NonNullableUnderlyingType == typeof (byte[]))
        this.InternalReadType = ReadType.ReadAsBytes;
      else if (this.NonNullableUnderlyingType == typeof (int))
        this.InternalReadType = ReadType.ReadAsInt32;
      else if (this.NonNullableUnderlyingType == typeof (Decimal))
        this.InternalReadType = ReadType.ReadAsDecimal;
      else if (this.NonNullableUnderlyingType == typeof (string))
        this.InternalReadType = ReadType.ReadAsString;
      else if (this.NonNullableUnderlyingType == typeof (DateTime))
        this.InternalReadType = ReadType.ReadAsDateTime;
      else
        this.InternalReadType = ReadType.Read;
    }

    internal void InvokeOnSerializing(object o, StreamingContext context)
    {
      if (this.OnSerializing == null)
        return;
      this.OnSerializing.Invoke(o, new object[1]
      {
        (object) context
      });
    }

    internal void InvokeOnSerialized(object o, StreamingContext context)
    {
      if (this.OnSerialized == null)
        return;
      this.OnSerialized.Invoke(o, new object[1]
      {
        (object) context
      });
    }

    internal void InvokeOnDeserializing(object o, StreamingContext context)
    {
      if (this.OnDeserializing == null)
        return;
      this.OnDeserializing.Invoke(o, new object[1]
      {
        (object) context
      });
    }

    internal void InvokeOnDeserialized(object o, StreamingContext context)
    {
      if (this.OnDeserialized == null)
        return;
      this.OnDeserialized.Invoke(o, new object[1]
      {
        (object) context
      });
    }

    internal void InvokeOnError(object o, StreamingContext context, ErrorContext errorContext)
    {
      if (this.OnError == null)
        return;
      this.OnError.Invoke(o, new object[2]
      {
        (object) context,
        (object) errorContext
      });
    }
  }
}
