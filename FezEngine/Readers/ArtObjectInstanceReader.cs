// Type: FezEngine.Readers.ArtObjectInstanceReader
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using FezEngine.Structure;
using Microsoft.Xna.Framework.Content;

namespace FezEngine.Readers
{
  public class ArtObjectInstanceReader : ContentTypeReader<ArtObjectInstance>
  {
    protected override ArtObjectInstance Read(ContentReader input, ArtObjectInstance existingInstance)
    {
      if (existingInstance == null)
        existingInstance = new ArtObjectInstance(input.ReadString());
      existingInstance.Position = input.ReadVector3();
      existingInstance.Rotation = input.ReadQuaternion();
      existingInstance.Scale = input.ReadVector3();
      existingInstance.ActorSettings = input.ReadObject<ArtObjectActorSettings>(existingInstance.ActorSettings);
      return existingInstance;
    }
  }
}
