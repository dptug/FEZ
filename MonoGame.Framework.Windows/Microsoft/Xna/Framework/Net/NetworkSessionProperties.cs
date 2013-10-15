// Type: Microsoft.Xna.Framework.Net.NetworkSessionProperties
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

using System.Collections.Generic;

namespace Microsoft.Xna.Framework.Net
{
  public class NetworkSessionProperties : List<int?>
  {
    public NetworkSessionProperties()
      : base(8)
    {
      this.Add(new int?());
      this.Add(new int?());
      this.Add(new int?());
      this.Add(new int?());
      this.Add(new int?());
      this.Add(new int?());
      this.Add(new int?());
      this.Add(new int?());
    }

    public static void WriteProperties(NetworkSessionProperties properties, int[] propertyData)
    {
      for (int index1 = 0; index1 < 8; ++index1)
      {
        int? nullable;
        int num1;
        if (properties != null)
        {
          nullable = properties[index1];
          num1 = !nullable.HasValue ? 1 : 0;
        }
        else
          num1 = 1;
        if (num1 == 0)
        {
          propertyData[index1 * 2] = 1;
          int[] numArray = propertyData;
          int index2 = index1 * 2 + 1;
          nullable = properties[index1];
          int num2 = nullable.Value;
          numArray[index2] = num2;
        }
        else
        {
          propertyData[index1 * 2] = 0;
          propertyData[index1 * 2 + 1] = 0;
        }
      }
    }

    public static void ReadProperties(NetworkSessionProperties properties, int[] propertyData)
    {
      for (int index = 0; index < 8; ++index)
      {
        properties[index] = new int?();
        if (propertyData[index * 2] > 0)
          properties[index] = new int?(propertyData[index * 2 + 1]);
      }
    }
  }
}
