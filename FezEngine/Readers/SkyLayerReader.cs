// Type: FezEngine.Readers.SkyLayerReader
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using FezEngine.Structure;
using Microsoft.Xna.Framework.Content;

namespace FezEngine.Readers
{
  public class SkyLayerReader : ContentTypeReader<SkyLayer>
  {
    protected override SkyLayer Read(ContentReader input, SkyLayer existingInstance)
    {
      if (existingInstance == null)
        existingInstance = new SkyLayer();
      existingInstance.Name = input.ReadString();
      existingInstance.InFront = input.ReadBoolean();
      existingInstance.Opacity = input.ReadSingle();
      existingInstance.FogTint = input.ReadSingle();
      return existingInstance;
    }
  }
}
