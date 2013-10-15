// Type: FezEngine.Readers.EntityReader
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using FezEngine.Structure.Scripting;
using Microsoft.Xna.Framework.Content;

namespace FezEngine.Readers
{
  public class EntityReader : ContentTypeReader<Entity>
  {
    protected override Entity Read(ContentReader input, Entity existingInstance)
    {
      if (existingInstance == null)
        existingInstance = new Entity();
      existingInstance.Type = input.ReadString();
      existingInstance.Identifier = input.ReadObject<int?>();
      return existingInstance;
    }
  }
}
