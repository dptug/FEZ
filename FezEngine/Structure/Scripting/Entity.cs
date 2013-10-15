// Type: FezEngine.Structure.Scripting.Entity
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using ContentSerialization.Attributes;

namespace FezEngine.Structure.Scripting
{
  public class Entity
  {
    public string Type { get; set; }

    [Serialization(Optional = true)]
    public int? Identifier { get; set; }

    public override string ToString()
    {
      if (!this.Identifier.HasValue)
        return this.Type;
      return string.Concat(new object[4]
      {
        (object) this.Type,
        (object) "[",
        (object) this.Identifier,
        (object) "]"
      });
    }

    public Entity Clone()
    {
      return new Entity()
      {
        Type = this.Type,
        Identifier = this.Identifier
      };
    }
  }
}
