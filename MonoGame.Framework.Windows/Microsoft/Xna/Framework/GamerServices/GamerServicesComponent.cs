// Type: Microsoft.Xna.Framework.GamerServices.GamerServicesComponent
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Net;

namespace Microsoft.Xna.Framework.GamerServices
{
  public class GamerServicesComponent : GameComponent
  {
    private static LocalNetworkGamer lng;

    internal static LocalNetworkGamer LocalNetworkGamer
    {
      get
      {
        return GamerServicesComponent.lng;
      }
      set
      {
        GamerServicesComponent.lng = value;
      }
    }

    public GamerServicesComponent(Game game)
      : base(game)
    {
      Guide.Initialise(game);
    }

    public override void Update(GameTime gameTime)
    {
    }
  }
}
