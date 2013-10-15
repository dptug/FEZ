// Type: Newtonsoft.Json.JsonSerializer
// Assembly: Newtonsoft.Json, Version=4.5.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: DB013E7B-84E7-4AE6-A132-56DC73318C48
// Assembly location: F:\Program Files (x86)\FEZ\Newtonsoft.Json.dll

using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;

namespace Newtonsoft.Json
{
  public class JsonSerializer
  {
    private TypeNameHandling _typeNameHandling;
    private FormatterAssemblyStyle _typeNameAssemblyFormat;
    private PreserveReferencesHandling _preserveReferencesHandling;
    private ReferenceLoopHandling _referenceLoopHandling;
    private MissingMemberHandling _missingMemberHandling;
    private ObjectCreationHandling _objectCreationHandling;
    private NullValueHandling _nullValueHandling;
    private DefaultValueHandling _defaultValueHandling;
    private ConstructorHandling _constructorHandling;
    private JsonConverterCollection _converters;
    private IContractResolver _contractResolver;
    private IReferenceResolver _referenceResolver;
    private SerializationBinder _binder;
    private StreamingContext _context;
    private Formatting? _formatting;
    private DateFormatHandling? _dateFormatHandling;
    private DateTimeZoneHandling? _dateTimeZoneHandling;
    private DateParseHandling? _dateParseHandling;
    private CultureInfo _culture;
    private int? _maxDepth;
    private bool _maxDepthSet;
    private bool? _checkAdditionalContent;

    public virtual IReferenceResolver ReferenceResolver
    {
      get
      {
        if (this._referenceResolver == null)
          this._referenceResolver = (IReferenceResolver) new DefaultReferenceResolver();
        return this._referenceResolver;
      }
      set
      {
        if (value == null)
          throw new ArgumentNullException("value", "Reference resolver cannot be null.");
        this._referenceResolver = value;
      }
    }

    public virtual SerializationBinder Binder
    {
      get
      {
        return this._binder;
      }
      set
      {
        if (value == null)
          throw new ArgumentNullException("value", "Serialization binder cannot be null.");
        this._binder = value;
      }
    }

    public virtual TypeNameHandling TypeNameHandling
    {
      get
      {
        return this._typeNameHandling;
      }
      set
      {
        if (value < TypeNameHandling.None || value > TypeNameHandling.Auto)
          throw new ArgumentOutOfRangeException("value");
        this._typeNameHandling = value;
      }
    }

    public virtual FormatterAssemblyStyle TypeNameAssemblyFormat
    {
      get
      {
        return this._typeNameAssemblyFormat;
      }
      set
      {
        if (value < FormatterAssemblyStyle.Simple || value > FormatterAssemblyStyle.Full)
          throw new ArgumentOutOfRangeException("value");
        this._typeNameAssemblyFormat = value;
      }
    }

    public virtual PreserveReferencesHandling PreserveReferencesHandling
    {
      get
      {
        return this._preserveReferencesHandling;
      }
      set
      {
        if (value < PreserveReferencesHandling.None || value > PreserveReferencesHandling.All)
          throw new ArgumentOutOfRangeException("value");
        this._preserveReferencesHandling = value;
      }
    }

    public virtual ReferenceLoopHandling ReferenceLoopHandling
    {
      get
      {
        return this._referenceLoopHandling;
      }
      set
      {
        if (value < ReferenceLoopHandling.Error || value > ReferenceLoopHandling.Serialize)
          throw new ArgumentOutOfRangeException("value");
        this._referenceLoopHandling = value;
      }
    }

    public virtual MissingMemberHandling MissingMemberHandling
    {
      get
      {
        return this._missingMemberHandling;
      }
      set
      {
        if (value < MissingMemberHandling.Ignore || value > MissingMemberHandling.Error)
          throw new ArgumentOutOfRangeException("value");
        this._missingMemberHandling = value;
      }
    }

    public virtual NullValueHandling NullValueHandling
    {
      get
      {
        return this._nullValueHandling;
      }
      set
      {
        if (value < NullValueHandling.Include || value > NullValueHandling.Ignore)
          throw new ArgumentOutOfRangeException("value");
        this._nullValueHandling = value;
      }
    }

    public virtual DefaultValueHandling DefaultValueHandling
    {
      get
      {
        return this._defaultValueHandling;
      }
      set
      {
        if (value < DefaultValueHandling.Include || value > DefaultValueHandling.IgnoreAndPopulate)
          throw new ArgumentOutOfRangeException("value");
        this._defaultValueHandling = value;
      }
    }

    public virtual ObjectCreationHandling ObjectCreationHandling
    {
      get
      {
        return this._objectCreationHandling;
      }
      set
      {
        if (value < ObjectCreationHandling.Auto || value > ObjectCreationHandling.Replace)
          throw new ArgumentOutOfRangeException("value");
        this._objectCreationHandling = value;
      }
    }

    public virtual ConstructorHandling ConstructorHandling
    {
      get
      {
        return this._constructorHandling;
      }
      set
      {
        if (value < ConstructorHandling.Default || value > ConstructorHandling.AllowNonPublicDefaultConstructor)
          throw new ArgumentOutOfRangeException("value");
        this._constructorHandling = value;
      }
    }

    public virtual JsonConverterCollection Converters
    {
      get
      {
        if (this._converters == null)
          this._converters = new JsonConverterCollection();
        return this._converters;
      }
    }

    public virtual IContractResolver ContractResolver
    {
      get
      {
        if (this._contractResolver == null)
          this._contractResolver = DefaultContractResolver.Instance;
        return this._contractResolver;
      }
      set
      {
        this._contractResolver = value;
      }
    }

    public virtual StreamingContext Context
    {
      get
      {
        return this._context;
      }
      set
      {
        this._context = value;
      }
    }

    public virtual Formatting Formatting
    {
      get
      {
        return this._formatting ?? Formatting.None;
      }
      set
      {
        this._formatting = new Formatting?(value);
      }
    }

    public virtual DateFormatHandling DateFormatHandling
    {
      get
      {
        return this._dateFormatHandling ?? DateFormatHandling.IsoDateFormat;
      }
      set
      {
        this._dateFormatHandling = new DateFormatHandling?(value);
      }
    }

    public virtual DateTimeZoneHandling DateTimeZoneHandling
    {
      get
      {
        return this._dateTimeZoneHandling ?? DateTimeZoneHandling.RoundtripKind;
      }
      set
      {
        this._dateTimeZoneHandling = new DateTimeZoneHandling?(value);
      }
    }

    public virtual DateParseHandling DateParseHandling
    {
      get
      {
        return this._dateParseHandling ?? DateParseHandling.DateTime;
      }
      set
      {
        this._dateParseHandling = new DateParseHandling?(value);
      }
    }

    public virtual CultureInfo Culture
    {
      get
      {
        return this._culture ?? JsonSerializerSettings.DefaultCulture;
      }
      set
      {
        this._culture = value;
      }
    }

    public virtual int? MaxDepth
    {
      get
      {
        return this._maxDepth;
      }
      set
      {
        int? nullable = value;
        if ((nullable.GetValueOrDefault() > 0 ? 0 : (nullable.HasValue ? 1 : 0)) != 0)
          throw new ArgumentException("Value must be positive.", "value");
        this._maxDepth = value;
        this._maxDepthSet = true;
      }
    }

    public virtual bool CheckAdditionalContent
    {
      get
      {
        return this._checkAdditionalContent ?? false;
      }
      set
      {
        this._checkAdditionalContent = new bool?(value);
      }
    }

    public virtual event EventHandler<Newtonsoft.Json.Serialization.ErrorEventArgs> Error;

    public JsonSerializer()
    {
      this._referenceLoopHandling = ReferenceLoopHandling.Error;
      this._missingMemberHandling = MissingMemberHandling.Ignore;
      this._nullValueHandling = NullValueHandling.Include;
      this._defaultValueHandling = DefaultValueHandling.Include;
      this._objectCreationHandling = ObjectCreationHandling.Auto;
      this._preserveReferencesHandling = PreserveReferencesHandling.None;
      this._constructorHandling = ConstructorHandling.Default;
      this._typeNameHandling = TypeNameHandling.None;
      this._context = JsonSerializerSettings.DefaultContext;
      this._binder = (SerializationBinder) DefaultSerializationBinder.Instance;
    }

    internal bool IsCheckAdditionalContentSet()
    {
      return this._checkAdditionalContent.HasValue;
    }

    public static JsonSerializer Create(JsonSerializerSettings settings)
    {
      JsonSerializer jsonSerializer = new JsonSerializer();
      if (settings != null)
      {
        if (!CollectionUtils.IsNullOrEmpty<JsonConverter>((ICollection<JsonConverter>) settings.Converters))
          CollectionUtils.AddRange<JsonConverter>((IList<JsonConverter>) jsonSerializer.Converters, (IEnumerable<JsonConverter>) settings.Converters);
        jsonSerializer.TypeNameHandling = settings.TypeNameHandling;
        jsonSerializer.TypeNameAssemblyFormat = settings.TypeNameAssemblyFormat;
        jsonSerializer.PreserveReferencesHandling = settings.PreserveReferencesHandling;
        jsonSerializer.ReferenceLoopHandling = settings.ReferenceLoopHandling;
        jsonSerializer.MissingMemberHandling = settings.MissingMemberHandling;
        jsonSerializer.ObjectCreationHandling = settings.ObjectCreationHandling;
        jsonSerializer.NullValueHandling = settings.NullValueHandling;
        jsonSerializer.DefaultValueHandling = settings.DefaultValueHandling;
        jsonSerializer.ConstructorHandling = settings.ConstructorHandling;
        jsonSerializer.Context = settings.Context;
        jsonSerializer._checkAdditionalContent = settings._checkAdditionalContent;
        jsonSerializer._formatting = settings._formatting;
        jsonSerializer._dateFormatHandling = settings._dateFormatHandling;
        jsonSerializer._dateTimeZoneHandling = settings._dateTimeZoneHandling;
        jsonSerializer._dateParseHandling = settings._dateParseHandling;
        jsonSerializer._culture = settings._culture;
        jsonSerializer._maxDepth = settings._maxDepth;
        jsonSerializer._maxDepthSet = settings._maxDepthSet;
        if (settings.Error != null)
          jsonSerializer.Error += settings.Error;
        if (settings.ContractResolver != null)
          jsonSerializer.ContractResolver = settings.ContractResolver;
        if (settings.ReferenceResolver != null)
          jsonSerializer.ReferenceResolver = settings.ReferenceResolver;
        if (settings.Binder != null)
          jsonSerializer.Binder = settings.Binder;
      }
      return jsonSerializer;
    }

    public void Populate(TextReader reader, object target)
    {
      this.Populate((JsonReader) new JsonTextReader(reader), target);
    }

    public void Populate(JsonReader reader, object target)
    {
      this.PopulateInternal(reader, target);
    }

    internal virtual void PopulateInternal(JsonReader reader, object target)
    {
      ValidationUtils.ArgumentNotNull((object) reader, "reader");
      ValidationUtils.ArgumentNotNull(target, "target");
      new JsonSerializerInternalReader(this).Populate(reader, target);
    }

    public object Deserialize(JsonReader reader)
    {
      return this.Deserialize(reader, (Type) null);
    }

    public object Deserialize(TextReader reader, Type objectType)
    {
      return this.Deserialize((JsonReader) new JsonTextReader(reader), objectType);
    }

    public T Deserialize<T>(JsonReader reader)
    {
      return (T) this.Deserialize(reader, typeof (T));
    }

    public object Deserialize(JsonReader reader, Type objectType)
    {
      return this.DeserializeInternal(reader, objectType);
    }

    internal virtual object DeserializeInternal(JsonReader reader, Type objectType)
    {
      ValidationUtils.ArgumentNotNull((object) reader, "reader");
      CultureInfo cultureInfo = (CultureInfo) null;
      if (this._culture != null && reader.Culture != this._culture)
      {
        cultureInfo = reader.Culture;
        reader.Culture = this._culture;
      }
      DateTimeZoneHandling? nullable1 = new DateTimeZoneHandling?();
      if (this._dateTimeZoneHandling.HasValue)
      {
        DateTimeZoneHandling timeZoneHandling = reader.DateTimeZoneHandling;
        DateTimeZoneHandling? nullable2 = this._dateTimeZoneHandling;
        if ((timeZoneHandling != nullable2.GetValueOrDefault() ? 1 : (!nullable2.HasValue ? 1 : 0)) != 0)
        {
          nullable1 = new DateTimeZoneHandling?(reader.DateTimeZoneHandling);
          reader.DateTimeZoneHandling = this._dateTimeZoneHandling.Value;
        }
      }
      DateParseHandling? nullable3 = new DateParseHandling?();
      if (this._dateParseHandling.HasValue)
      {
        DateParseHandling dateParseHandling = reader.DateParseHandling;
        DateParseHandling? nullable2 = this._dateParseHandling;
        if ((dateParseHandling != nullable2.GetValueOrDefault() ? 1 : (!nullable2.HasValue ? 1 : 0)) != 0)
        {
          nullable3 = new DateParseHandling?(reader.DateParseHandling);
          reader.DateParseHandling = this._dateParseHandling.Value;
        }
      }
      int? nullable4 = new int?();
      if (this._maxDepthSet)
      {
        int? maxDepth = reader.MaxDepth;
        int? nullable2 = this._maxDepth;
        if ((maxDepth.GetValueOrDefault() != nullable2.GetValueOrDefault() ? 1 : (maxDepth.HasValue != nullable2.HasValue ? 1 : 0)) != 0)
        {
          nullable4 = reader.MaxDepth;
          reader.MaxDepth = this._maxDepth;
        }
      }
      object obj = new JsonSerializerInternalReader(this).Deserialize(reader, objectType, this.CheckAdditionalContent);
      if (cultureInfo != null)
        reader.Culture = cultureInfo;
      if (nullable1.HasValue)
        reader.DateTimeZoneHandling = nullable1.Value;
      if (nullable3.HasValue)
        reader.DateParseHandling = nullable3.Value;
      if (this._maxDepthSet)
        reader.MaxDepth = nullable4;
      return obj;
    }

    public void Serialize(TextWriter textWriter, object value)
    {
      this.Serialize((JsonWriter) new JsonTextWriter(textWriter), value);
    }

    public void Serialize(JsonWriter jsonWriter, object value)
    {
      this.SerializeInternal(jsonWriter, value);
    }

    internal virtual void SerializeInternal(JsonWriter jsonWriter, object value)
    {
      ValidationUtils.ArgumentNotNull((object) jsonWriter, "jsonWriter");
      Formatting? nullable1 = new Formatting?();
      if (this._formatting.HasValue)
      {
        Formatting formatting = jsonWriter.Formatting;
        Formatting? nullable2 = this._formatting;
        if ((formatting != nullable2.GetValueOrDefault() ? 1 : (!nullable2.HasValue ? 1 : 0)) != 0)
        {
          nullable1 = new Formatting?(jsonWriter.Formatting);
          jsonWriter.Formatting = this._formatting.Value;
        }
      }
      DateFormatHandling? nullable3 = new DateFormatHandling?();
      if (this._dateFormatHandling.HasValue)
      {
        DateFormatHandling dateFormatHandling = jsonWriter.DateFormatHandling;
        DateFormatHandling? nullable2 = this._dateFormatHandling;
        if ((dateFormatHandling != nullable2.GetValueOrDefault() ? 1 : (!nullable2.HasValue ? 1 : 0)) != 0)
        {
          nullable3 = new DateFormatHandling?(jsonWriter.DateFormatHandling);
          jsonWriter.DateFormatHandling = this._dateFormatHandling.Value;
        }
      }
      DateTimeZoneHandling? nullable4 = new DateTimeZoneHandling?();
      if (this._dateTimeZoneHandling.HasValue)
      {
        DateTimeZoneHandling timeZoneHandling = jsonWriter.DateTimeZoneHandling;
        DateTimeZoneHandling? nullable2 = this._dateTimeZoneHandling;
        if ((timeZoneHandling != nullable2.GetValueOrDefault() ? 1 : (!nullable2.HasValue ? 1 : 0)) != 0)
        {
          nullable4 = new DateTimeZoneHandling?(jsonWriter.DateTimeZoneHandling);
          jsonWriter.DateTimeZoneHandling = this._dateTimeZoneHandling.Value;
        }
      }
      new JsonSerializerInternalWriter(this).Serialize(jsonWriter, value);
      if (nullable1.HasValue)
        jsonWriter.Formatting = nullable1.Value;
      if (nullable3.HasValue)
        jsonWriter.DateFormatHandling = nullable3.Value;
      if (!nullable4.HasValue)
        return;
      jsonWriter.DateTimeZoneHandling = nullable4.Value;
    }

    internal JsonConverter GetMatchingConverter(Type type)
    {
      return JsonSerializer.GetMatchingConverter((IList<JsonConverter>) this._converters, type);
    }

    internal static JsonConverter GetMatchingConverter(IList<JsonConverter> converters, Type objectType)
    {
      if (converters != null)
      {
        for (int index = 0; index < converters.Count; ++index)
        {
          JsonConverter jsonConverter = converters[index];
          if (jsonConverter.CanConvert(objectType))
            return jsonConverter;
        }
      }
      return (JsonConverter) null;
    }

    internal void OnError(Newtonsoft.Json.Serialization.ErrorEventArgs e)
    {
      EventHandler<Newtonsoft.Json.Serialization.ErrorEventArgs> eventHandler = this.Error;
      if (eventHandler == null)
        return;
      eventHandler((object) this, e);
    }
  }
}
