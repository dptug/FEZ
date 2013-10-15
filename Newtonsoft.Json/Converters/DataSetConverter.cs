// Type: Newtonsoft.Json.Converters.DataSetConverter
// Assembly: Newtonsoft.Json, Version=4.5.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: DB013E7B-84E7-4AE6-A132-56DC73318C48
// Assembly location: F:\Program Files (x86)\FEZ\Newtonsoft.Json.dll

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Data;

namespace Newtonsoft.Json.Converters
{
  public class DataSetConverter : JsonConverter
  {
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
      DataSet dataSet = (DataSet) value;
      DefaultContractResolver contractResolver = serializer.ContractResolver as DefaultContractResolver;
      DataTableConverter dataTableConverter = new DataTableConverter();
      writer.WriteStartObject();
      foreach (DataTable dataTable in (InternalDataCollectionBase) dataSet.Tables)
      {
        writer.WritePropertyName(contractResolver != null ? contractResolver.GetResolvedPropertyName(dataTable.TableName) : dataTable.TableName);
        dataTableConverter.WriteJson(writer, (object) dataTable, serializer);
      }
      writer.WriteEndObject();
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
      DataSet dataSet = new DataSet();
      DataTableConverter dataTableConverter = new DataTableConverter();
      reader.Read();
      while (reader.TokenType == JsonToken.PropertyName)
      {
        DataTable table = (DataTable) dataTableConverter.ReadJson(reader, typeof (DataTable), (object) null, serializer);
        dataSet.Tables.Add(table);
        reader.Read();
      }
      return (object) dataSet;
    }

    public override bool CanConvert(Type valueType)
    {
      return valueType == typeof (DataSet);
    }
  }
}
