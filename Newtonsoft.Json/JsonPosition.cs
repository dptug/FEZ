// Type: Newtonsoft.Json.JsonPosition
// Assembly: Newtonsoft.Json, Version=4.5.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: DB013E7B-84E7-4AE6-A132-56DC73318C48
// Assembly location: F:\Program Files (x86)\FEZ\Newtonsoft.Json.dll

using System.Collections.Generic;
using System.Text;

namespace Newtonsoft.Json
{
  internal struct JsonPosition
  {
    internal JsonContainerType Type;
    internal int? Position;
    internal string PropertyName;

    internal void WriteTo(StringBuilder sb)
    {
      switch (this.Type)
      {
        case JsonContainerType.Object:
          if (this.PropertyName == null)
            break;
          if (sb.Length > 0)
            sb.Append(".");
          sb.Append(this.PropertyName);
          break;
        case JsonContainerType.Array:
        case JsonContainerType.Constructor:
          if (!this.Position.HasValue)
            break;
          sb.Append("[");
          sb.Append((object) this.Position);
          sb.Append("]");
          break;
      }
    }

    internal bool InsideContainer()
    {
      switch (this.Type)
      {
        case JsonContainerType.Object:
          return this.PropertyName != null;
        case JsonContainerType.Array:
        case JsonContainerType.Constructor:
          return this.Position.HasValue;
        default:
          return false;
      }
    }

    internal static string BuildPath(IEnumerable<JsonPosition> positions)
    {
      StringBuilder sb = new StringBuilder();
      foreach (JsonPosition jsonPosition in positions)
        jsonPosition.WriteTo(sb);
      return ((object) sb).ToString();
    }
  }
}
