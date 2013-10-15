// Type: Newtonsoft.Json.Converters.DataTableConverter
// Assembly: Newtonsoft.Json, Version=4.5.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: DB013E7B-84E7-4AE6-A132-56DC73318C48
// Assembly location: F:\Program Files (x86)\FEZ\Newtonsoft.Json.dll

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Data;

namespace Newtonsoft.Json.Converters
{
  public class DataTableConverter : JsonConverter
  {
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
      DataTable dataTable = (DataTable) value;
      DefaultContractResolver contractResolver = serializer.ContractResolver as DefaultContractResolver;
      writer.WriteStartArray();
      foreach (DataRow dataRow in (InternalDataCollectionBase) dataTable.Rows)
      {
        writer.WriteStartObject();
        foreach (DataColumn index in (InternalDataCollectionBase) dataRow.Table.Columns)
        {
          if (serializer.NullValueHandling != NullValueHandling.Ignore || dataRow[index] != null && dataRow[index] != DBNull.Value)
          {
            writer.WritePropertyName(contractResolver != null ? contractResolver.GetResolvedPropertyName(index.ColumnName) : index.ColumnName);
            serializer.Serialize(writer, dataRow[index]);
          }
        }
        writer.WriteEndObject();
      }
      writer.WriteEndArray();
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
      DataTable dataTable;
      if (reader.TokenType == JsonToken.PropertyName)
      {
        dataTable = new DataTable((string) reader.Value);
        reader.Read();
      }
      else
        dataTable = new DataTable();
      reader.Read();
      while (reader.TokenType == JsonToken.StartObject)
      {
        DataRow row = dataTable.NewRow();
        reader.Read();
        while (reader.TokenType == JsonToken.PropertyName)
        {
          string index = (string) reader.Value;
          reader.Read();
          if (!dataTable.Columns.Contains(index))
          {
            Type columnDataType = DataTableConverter.GetColumnDataType(reader.TokenType);
            dataTable.Columns.Add(new DataColumn(index, columnDataType));
          }
          row[index] = reader.Value ?? (object) DBNull.Value;
          reader.Read();
        }
        row.EndEdit();
        dataTable.Rows.Add(row);
        reader.Read();
      }
      return (object) dataTable;
    }

    private static Type GetColumnDataType(JsonToken tokenType)
    {
      switch (tokenType)
      {
        case JsonToken.Integer:
          return typeof (long);
        case JsonToken.Float:
          return typeof (double);
        case JsonToken.String:
        case JsonToken.Null:
        case JsonToken.Undefined:
          return typeof (string);
        case JsonToken.Boolean:
          return typeof (bool);
        case JsonToken.Date:
          return typeof (DateTime);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }

    public override bool CanConvert(Type valueType)
    {
      return valueType == typeof (DataTable);
    }
  }
}
