// Type: Newtonsoft.Json.Serialization.JsonSerializerProxy
// Assembly: Newtonsoft.Json, Version=4.5.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: DB013E7B-84E7-4AE6-A132-56DC73318C48
// Assembly location: F:\Program Files (x86)\FEZ\Newtonsoft.Json.dll

using Newtonsoft.Json;
using Newtonsoft.Json.Utilities;
using System;
using System.Globalization;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;

namespace Newtonsoft.Json.Serialization
{
  internal class JsonSerializerProxy : JsonSerializer
  {
    private readonly JsonSerializerInternalReader _serializerReader;
    private readonly JsonSerializerInternalWriter _serializerWriter;
    private readonly JsonSerializer _serializer;

    public override IReferenceResolver ReferenceResolver
    {
      get
      {
        return this._serializer.ReferenceResolver;
      }
      set
      {
        this._serializer.ReferenceResolver = value;
      }
    }

    public override JsonConverterCollection Converters
    {
      get
      {
        return this._serializer.Converters;
      }
    }

    public override DefaultValueHandling DefaultValueHandling
    {
      get
      {
        return this._serializer.DefaultValueHandling;
      }
      set
      {
        this._serializer.DefaultValueHandling = value;
      }
    }

    public override IContractResolver ContractResolver
    {
      get
      {
        return this._serializer.ContractResolver;
      }
      set
      {
        this._serializer.ContractResolver = value;
      }
    }

    public override MissingMemberHandling MissingMemberHandling
    {
      get
      {
        return this._serializer.MissingMemberHandling;
      }
      set
      {
        this._serializer.MissingMemberHandling = value;
      }
    }

    public override NullValueHandling NullValueHandling
    {
      get
      {
        return this._serializer.NullValueHandling;
      }
      set
      {
        this._serializer.NullValueHandling = value;
      }
    }

    public override ObjectCreationHandling ObjectCreationHandling
    {
      get
      {
        return this._serializer.ObjectCreationHandling;
      }
      set
      {
        this._serializer.ObjectCreationHandling = value;
      }
    }

    public override ReferenceLoopHandling ReferenceLoopHandling
    {
      get
      {
        return this._serializer.ReferenceLoopHandling;
      }
      set
      {
        this._serializer.ReferenceLoopHandling = value;
      }
    }

    public override PreserveReferencesHandling PreserveReferencesHandling
    {
      get
      {
        return this._serializer.PreserveReferencesHandling;
      }
      set
      {
        this._serializer.PreserveReferencesHandling = value;
      }
    }

    public override TypeNameHandling TypeNameHandling
    {
      get
      {
        return this._serializer.TypeNameHandling;
      }
      set
      {
        this._serializer.TypeNameHandling = value;
      }
    }

    public override FormatterAssemblyStyle TypeNameAssemblyFormat
    {
      get
      {
        return this._serializer.TypeNameAssemblyFormat;
      }
      set
      {
        this._serializer.TypeNameAssemblyFormat = value;
      }
    }

    public override ConstructorHandling ConstructorHandling
    {
      get
      {
        return this._serializer.ConstructorHandling;
      }
      set
      {
        this._serializer.ConstructorHandling = value;
      }
    }

    public override SerializationBinder Binder
    {
      get
      {
        return this._serializer.Binder;
      }
      set
      {
        this._serializer.Binder = value;
      }
    }

    public override StreamingContext Context
    {
      get
      {
        return this._serializer.Context;
      }
      set
      {
        this._serializer.Context = value;
      }
    }

    public override Formatting Formatting
    {
      get
      {
        return this._serializer.Formatting;
      }
      set
      {
        this._serializer.Formatting = value;
      }
    }

    public override DateFormatHandling DateFormatHandling
    {
      get
      {
        return this._serializer.DateFormatHandling;
      }
      set
      {
        this._serializer.DateFormatHandling = value;
      }
    }

    public override DateTimeZoneHandling DateTimeZoneHandling
    {
      get
      {
        return this._serializer.DateTimeZoneHandling;
      }
      set
      {
        this._serializer.DateTimeZoneHandling = value;
      }
    }

    public override DateParseHandling DateParseHandling
    {
      get
      {
        return this._serializer.DateParseHandling;
      }
      set
      {
        this._serializer.DateParseHandling = value;
      }
    }

    public override CultureInfo Culture
    {
      get
      {
        return this._serializer.Culture;
      }
      set
      {
        this._serializer.Culture = value;
      }
    }

    public override int? MaxDepth
    {
      get
      {
        return this._serializer.MaxDepth;
      }
      set
      {
        this._serializer.MaxDepth = value;
      }
    }

    public override bool CheckAdditionalContent
    {
      get
      {
        return this._serializer.CheckAdditionalContent;
      }
      set
      {
        this._serializer.CheckAdditionalContent = value;
      }
    }

    public override event EventHandler<ErrorEventArgs> Error
    {
      add
      {
        this._serializer.Error += value;
      }
      remove
      {
        this._serializer.Error -= value;
      }
    }

    public JsonSerializerProxy(JsonSerializerInternalReader serializerReader)
    {
      ValidationUtils.ArgumentNotNull((object) serializerReader, "serializerReader");
      this._serializerReader = serializerReader;
      this._serializer = serializerReader.Serializer;
    }

    public JsonSerializerProxy(JsonSerializerInternalWriter serializerWriter)
    {
      ValidationUtils.ArgumentNotNull((object) serializerWriter, "serializerWriter");
      this._serializerWriter = serializerWriter;
      this._serializer = serializerWriter.Serializer;
    }

    internal JsonSerializerInternalBase GetInternalSerializer()
    {
      if (this._serializerReader != null)
        return (JsonSerializerInternalBase) this._serializerReader;
      else
        return (JsonSerializerInternalBase) this._serializerWriter;
    }

    internal override object DeserializeInternal(JsonReader reader, Type objectType)
    {
      if (this._serializerReader != null)
        return this._serializerReader.Deserialize(reader, objectType, false);
      else
        return this._serializer.Deserialize(reader, objectType);
    }

    internal override void PopulateInternal(JsonReader reader, object target)
    {
      if (this._serializerReader != null)
        this._serializerReader.Populate(reader, target);
      else
        this._serializer.Populate(reader, target);
    }

    internal override void SerializeInternal(JsonWriter jsonWriter, object value)
    {
      if (this._serializerWriter != null)
        this._serializerWriter.Serialize(jsonWriter, value);
      else
        this._serializer.Serialize(jsonWriter, value);
    }
  }
}
