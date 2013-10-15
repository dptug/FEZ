// Type: FezEngine.Readers.TrixelIdentifierReader
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using FezEngine.Structure;
using Microsoft.Xna.Framework.Content;

namespace FezEngine.Readers
{
  public static class TrixelIdentifierReader
  {
    public static TrixelEmplacement ReadTrixelIdentifier(this ContentReader input)
    {
      return new TrixelEmplacement()
      {
        X = input.ReadInt32(),
        Y = input.ReadInt32(),
        Z = input.ReadInt32()
      };
    }
  }
}
