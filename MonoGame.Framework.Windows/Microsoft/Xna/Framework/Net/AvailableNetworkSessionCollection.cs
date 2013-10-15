// Type: Microsoft.Xna.Framework.Net.AvailableNetworkSessionCollection
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

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
