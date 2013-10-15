// Type: Microsoft.Xna.Framework.GamerServices.GamerServicesComponent
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

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
