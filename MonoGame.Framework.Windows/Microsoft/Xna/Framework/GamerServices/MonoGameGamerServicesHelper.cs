// Type: Microsoft.Xna.Framework.GamerServices.MonoGameGamerServicesHelper
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

using Microsoft.Xna.Framework;

namespace Microsoft.Xna.Framework.GamerServices
{
  internal class MonoGameGamerServicesHelper
  {
    private static MonoLiveGuide guide = (MonoLiveGuide) null;

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
