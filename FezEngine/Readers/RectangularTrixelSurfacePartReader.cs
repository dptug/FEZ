// Type: FezEngine.Readers.RectangularTrixelSurfacePartReader
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using FezEngine;
using FezEngine.Structure;
using Microsoft.Xna.Framework.Content;

namespace FezEngine.Readers
{
  public class RectangularTrixelSurfacePartReader : ContentTypeReader<RectangularTrixelSurfacePart>
  {
    protected override RectangularTrixelSurfacePart Read(ContentReader input, RectangularTrixelSurfacePart existingInstance)
    {
      if (existingInstance == null)
        existingInstance = new RectangularTrixelSurfacePart();
      existingInstance.Start = TrixelIdentifierReader.ReadTrixelIdentifier(input);
      existingInstance.Orientation = input.ReadObject<FaceOrientation>();
      existingInstance.TangentSize = input.ReadInt32();
      existingInstance.BitangentSize = input.ReadInt32();
      return existingInstance;
    }
  }
}
