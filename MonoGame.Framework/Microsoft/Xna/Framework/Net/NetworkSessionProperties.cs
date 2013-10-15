// Type: Microsoft.Xna.Framework.Net.NetworkSessionProperties
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

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
      for (int index = 0; index < 8; ++index)
      {
        if (properties != null && properties[index].HasValue)
        {
          propertyData[index * 2] = 1;
          propertyData[index * 2 + 1] = properties[index].Value;
        }
        else
        {
          propertyData[index * 2] = 0;
          propertyData[index * 2 + 1] = 0;
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
