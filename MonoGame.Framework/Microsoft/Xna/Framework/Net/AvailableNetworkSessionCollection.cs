// Type: Microsoft.Xna.Framework.Net.AvailableNetworkSessionCollection
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Microsoft.Xna.Framework.Net
{
  public sealed class AvailableNetworkSessionCollection : ReadOnlyCollection<AvailableNetworkSession>, IDisposable
  {
    public AvailableNetworkSessionCollection(IList<AvailableNetworkSession> list)
      : base(list)
    {
    }

    public void Dispose()
    {
    }
  }
}
