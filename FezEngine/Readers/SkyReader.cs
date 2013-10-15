// Type: FezEngine.Readers.SkyReader
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using FezEngine.Structure;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;

namespace FezEngine.Readers
{
  public class SkyReader : ContentTypeReader<Sky>
  {
    protected override Sky Read(ContentReader input, Sky existingInstance)
    {
      if (existingInstance == null)
        existingInstance = new Sky();
      existingInstance.Name = input.ReadString();
      existingInstance.Background = input.ReadString();
      existingInstance.WindSpeed = input.ReadSingle();
      existingInstance.Density = input.ReadSingle();
      existingInstance.FogDensity = input.ReadSingle();
      existingInstance.Layers = input.ReadObject<List<SkyLayer>>(existingInstance.Layers);
      existingInstance.Clouds = input.ReadObject<List<string>>(existingInstance.Clouds);
      existingInstance.Shadows = input.ReadObject<string>();
      existingInstance.Stars = input.ReadObject<string>();
      existingInstance.CloudTint = input.ReadObject<string>();
      existingInstance.VerticalTiling = input.ReadBoolean();
      existingInstance.HorizontalScrolling = input.ReadBoolean();
      existingInstance.LayerBaseHeight = input.ReadSingle();
      existingInstance.InterLayerVerticalDistance = input.ReadSingle();
      existingInstance.InterLayerHorizontalDistance = input.ReadSingle();
      existingInstance.HorizontalDistance = input.ReadSingle();
      existingInstance.VerticalDistance = input.ReadSingle();
      existingInstance.LayerBaseSpacing = input.ReadSingle();
      existingInstance.WindParallax = input.ReadSingle();
      existingInstance.WindDistance = input.ReadSingle();
      existingInstance.CloudsParallax = input.ReadSingle();
      existingInstance.ShadowOpacity = input.ReadSingle();
      existingInstance.FoliageShadows = input.ReadBoolean();
      existingInstance.NoPerFaceLayerXOffset = input.ReadBoolean();
      existingInstance.LayerBaseXOffset = input.ReadSingle();
      return existingInstance;
    }
  }
}
