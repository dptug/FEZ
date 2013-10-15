// Type: Newtonsoft.Json.JsonSerializerSettings
// Assembly: Newtonsoft.Json, Version=4.5.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: DB013E7B-84E7-4AE6-A132-56DC73318C48
// Assembly location: F:\Program Files (x86)\FEZ\Newtonsoft.Json.dll

using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;

namespace Newtonsoft.Json
{
  public class JsonSerializerSettings
  {
    internal static readonly StreamingContext DefaultContext = new StreamingContext();
    internal static readonly CultureInfo DefaultCulture = CultureInfo.InvariantCulture;
    internal const ReferenceLoopHandling DefaultReferenceLoopHandling = ReferenceLoopHandling.Error;
    internal const MissingMemberHandling DefaultMissingMemberHandling = MissingMemberHandling.Ignore;
    internal const NullValueHandling DefaultNullValueHandling = NullValueHandling.Include;
    internal const DefaultValueHandling DefaultDefaultValueHandling = DefaultValueHandling.Include;
    internal const ObjectCreationHandling DefaultObjectCreationHandling = ObjectCreationHandling.Auto;
    internal const PreserveReferencesHandling DefaultPreserveReferencesHandling = PreserveReferencesHandling.None;
    internal const ConstructorHandling DefaultConstructorHandling = ConstructorHandling.Default;
    internal const TypeNameHandling DefaultTypeNameHandling = TypeNameHandling.None;
    internal const FormatterAssemblyStyle DefaultTypeNameAssemblyFormat = FormatterAssemblyStyle.Simple;
    internal const Formatting DefaultFormatting = Formatting.None;
    internal const DateFormatHandling DefaultDateFormatHandling = DateFormatHandling.IsoDateFormat;
    internal const DateTimeZoneHandling DefaultDateTimeZoneHandling = DateTimeZoneHandling.RoundtripKind;
    internal const DateParseHandling DefaultDateParseHandling = DateParseHandling.DateTime;
    internal const bool DefaultCheckAdditionalContent = false;
    internal Formatting? _formatting;
    internal DateFormatHandling? _dateFormatHandling;
    internal DateTimeZoneHandling? _dateTimeZoneHandling;
    internal DateParseHandling? _dateParseHandling;
    internal CultureInfo _culture;
    internal bool? _checkAdditionalContent;
    internal int? _maxDepth;
    internal bool _maxDepthSet;

    public ReferenceLoopHandling ReferenceLoopHandling { get; set; }

    public MissingMemberHandling MissingMemberHandling { get; set; }

    public ObjectCreationHandling ObjectCreationHandling { get; set; }

    public NullValueHandling NullValueHandling { get; set; }

    public DefaultValueHandling DefaultValueHandling { get; set; }

    public IList<JsonConverter> Converters { get; set; }

    public PreserveReferencesHandling PreserveReferencesHandling { get; set; }

    public TypeNameHandling TypeNameHandling { get; set; }

    public FormatterAssemblyStyle TypeNameAssemblyFormat { get; set; }

    public ConstructorHandling ConstructorHandling { get; set; }

    public IContractResolver ContractResolver { get; set; }

    public IReferenceResolver ReferenceResolver { get; set; }

    public SerializationBinder Binder { get; set; }

    public EventHandler<ErrorEventArgs> Error { get; set; }

    public StreamingContext Context { get; set; }

    public int? MaxDepth
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

    public Formatting Formatting
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

    public DateFormatHandling DateFormatHandling
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

    public DateTimeZoneHandling DateTimeZoneHandling
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

    public DateParseHandling DateParseHandling
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

    public CultureInfo Culture
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

    public bool CheckAdditionalContent
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

    static JsonSerializerSettings()
    {
    }

    public JsonSerializerSettings()
    {
      this.ReferenceLoopHandling = ReferenceLoopHandling.Error;
      this.MissingMemberHandling = MissingMemberHandling.Ignore;
      this.ObjectCreationHandling = ObjectCreationHandling.Auto;
      this.NullValueHandling = NullValueHandling.Include;
      this.DefaultValueHandling = DefaultValueHandling.Include;
      this.PreserveReferencesHandling = PreserveReferencesHandling.None;
      this.TypeNameHandling = TypeNameHandling.None;
      this.TypeNameAssemblyFormat = FormatterAssemblyStyle.Simple;
      this.Context = JsonSerializerSettings.DefaultContext;
      this.Converters = (IList<JsonConverter>) new List<JsonConverter>();
    }
  }
}
