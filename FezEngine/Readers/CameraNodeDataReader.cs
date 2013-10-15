// Type: FezEngine.Readers.CameraNodeDataReader
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using FezEngine.Structure;
using Microsoft.Xna.Framework.Content;

namespace FezEngine.Readers
{
  public class CameraNodeDataReader : ContentTypeReader<CameraNodeData>
  {
    protected override CameraNodeData Read(ContentReader input, CameraNodeData existingInstance)
    {
      if (existingInstance == null)
        existingInstance = new CameraNodeData();
      existingInstance.Perspective = input.ReadBoolean();
      existingInstance.PixelsPerTrixel = input.ReadInt32();
      existingInstance.SoundName = input.ReadObject<string>();
      return existingInstance;
    }
  }
}
