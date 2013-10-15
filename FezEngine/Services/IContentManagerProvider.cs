// Type: FezEngine.Services.IContentManagerProvider
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;

namespace FezEngine.Services
{
  public interface IContentManagerProvider
  {
    ContentManager Global { get; }

    ContentManager CurrentLevel { get; }

    ContentManager GetForLevel(string levelName);

    ContentManager Get(CM name);

    IEnumerable<string> GetAllIn(string directory);

    void Dispose(CM name);
  }
}
