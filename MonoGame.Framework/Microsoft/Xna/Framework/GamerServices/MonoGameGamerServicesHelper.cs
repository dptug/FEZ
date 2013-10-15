// Type: Microsoft.Xna.Framework.GamerServices.MonoGameGamerServicesHelper
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

using Microsoft.Xna.Framework;

namespace Microsoft.Xna.Framework.GamerServices
{
  internal class MonoGameGamerServicesHelper
  {
    private static MonoLiveGuide guide;

    static MonoGameGamerServicesHelper()
    {
    }

    public static void ShowSigninSheet()
    {
      MonoGameGamerServicesHelper.guide.Enabled = true;
      MonoGameGamerServicesHelper.guide.Visible = true;
      Guide.IsVisible = true;
    }

    internal static void Initialise(Game game)
    {
      if (MonoGameGamerServicesHelper.guide != null)
        return;
      MonoGameGamerServicesHelper.guide = new MonoLiveGuide(game);
      game.Components.Add((IGameComponent) MonoGameGamerServicesHelper.guide);
    }
  }
}
