// Type: FezEngine.Components.FontManager
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using FezEngine.Services;
using FezEngine.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FezEngine.Components
{
  public class FontManager : GameComponent, IFontManager
  {
    private Language lastLanguage;

    public SpriteFont Big { get; private set; }

    public SpriteFont Small { get; private set; }

    public float SmallFactor { get; private set; }

    public float BigFactor { get; private set; }

    public float TopSpacing { get; private set; }

    public float SideSpacing { get; private set; }

    [ServiceDependency]
    public IContentManagerProvider CMProvider { get; set; }

    public FontManager(Game game)
      : base(game)
    {
    }

    public override void Initialize()
    {
      base.Initialize();
      this.ReloadFont(true);
    }

    private void ReloadFont(bool force = false)
    {
      if (force || Culture.IsCjk(this.lastLanguage) || Culture.IsCjk(Culture.Language))
      {
        switch (Culture.Language)
        {
          case Language.Chinese:
            this.Small = this.CMProvider.Global.Load<SpriteFont>("Fonts/Chinese Small");
            this.Big = this.CMProvider.Global.Load<SpriteFont>("Fonts/Chinese Big");
            this.SmallFactor = 0.5f;
            this.BigFactor = 0.34125f;
            this.SideSpacing = this.TopSpacing = 10f;
            break;
          case Language.Japanese:
            this.Small = this.CMProvider.Global.Load<SpriteFont>("Fonts/Japanese Small");
            this.Big = this.CMProvider.Global.Load<SpriteFont>("Fonts/Japanese Big");
            this.SmallFactor = 0.5f;
            this.BigFactor = 0.34125f;
            this.SideSpacing = this.TopSpacing = 10f;
            break;
          case Language.Korean:
            this.Small = this.CMProvider.Global.Load<SpriteFont>("Fonts/Korean Small");
            this.Big = this.CMProvider.Global.Load<SpriteFont>("Fonts/Korean Big");
            this.SmallFactor = 0.5f;
            this.BigFactor = 0.34125f;
            this.SideSpacing = this.TopSpacing = 10f;
            break;
          default:
            this.Small = this.CMProvider.Global.Load<SpriteFont>("Fonts/Latin Small");
            this.Big = this.CMProvider.Global.Load<SpriteFont>("Fonts/Latin Big");
            this.SmallFactor = 1.5f;
            this.BigFactor = 2f;
            this.Small.LineSpacing = 18;
            this.Big.LineSpacing = 18;
            this.SideSpacing = 8f;
            this.TopSpacing = 0.0f;
            break;
        }
      }
      this.Small.DefaultCharacter = new char?(' ');
      this.Big.DefaultCharacter = new char?(' ');
      this.lastLanguage = Culture.Language;
    }

    public override void Update(GameTime gameTime)
    {
      base.Update(gameTime);
      if (this.lastLanguage == Culture.Language)
        return;
      this.ReloadFont(false);
    }
  }
}
