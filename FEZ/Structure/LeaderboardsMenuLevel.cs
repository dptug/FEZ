// Type: FezGame.Structure.LeaderboardsMenuLevel
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using Common;
using CommunityExpressNS;
using FezEngine.Components;
using FezEngine.Services;
using FezEngine.Structure.Input;
using FezEngine.Tools;
using FezGame.Components;
using FezGame.Services;
using FezGame.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Globalization;
using System.Text;

namespace FezGame.Structure
{
  internal class LeaderboardsMenuLevel : MenuLevel
  {
    private const int EntriesPerPage = 10;
    private LeaderboardView view;
    private CachedLeaderboard leaderboard;
    private bool moveToLast;
    private bool moveToTop;
    private Rectangle? leftArrowRect;
    private Rectangle? rightArrowRect;
    private readonly MenuBase menuBase;

    public IInputManager InputManager { private get; set; }

    public IMouseStateManager MouseState { private get; set; }

    public IGameStateManager GameState { private get; set; }

    public SpriteFont Font { private get; set; }

    public LeaderboardsMenuLevel(MenuBase menuBase)
    {
      this.menuBase = menuBase;
    }

    public override void Initialize()
    {
      base.Initialize();
      this.IsDynamic = true;
      this.OnClose = (Action) (() =>
      {
        if (this.leaderboard == null)
          return;
        this.GameState.LiveConnectionChanged -= new Action(this.ChangeView);
        this.leaderboard = (CachedLeaderboard) null;
      });
      this.XButtonAction = (Action) (() =>
      {
        ++this.view;
        if (this.view > LeaderboardView.Overall)
          this.view = LeaderboardView.Friends;
        this.ChangeView();
      });
    }

    public override void Reset()
    {
      base.Reset();
      this.XButtonString = "{X} " + string.Format(StaticText.GetString("CurrentLeaderboardView"), (object) StaticText.GetString((string) (object) this.view + (object) "LeaderboardView"));
    }

    public override void Update(TimeSpan elapsed)
    {
      base.Update(elapsed);
      if (this.leaderboard == null)
        this.InitLeaderboards();
      if (!this.leaderboard.InError)
      {
        if (this.InputManager.RotateRight == FezButtonState.Pressed)
          this.TryPageUp();
        if (this.InputManager.RotateLeft == FezButtonState.Pressed)
          this.TryPageDown();
      }
      else if (this.GameState.HasActivePlayer && this.GameState.ActiveGamer != null && (!this.leaderboard.Reading && !this.leaderboard.ChangingPage))
      {
        this.leaderboard.ActiveGamer = this.GameState.ActiveGamer;
        this.ChangeView();
      }
      Point position = this.MouseState.Position;
      if (this.rightArrowRect.HasValue && this.rightArrowRect.Value.Contains(position))
      {
        this.menuBase.CursorSelectable = true;
        if (this.MouseState.LeftButton.State == MouseButtonStates.Pressed)
          this.TryPageUp();
      }
      if (this.leftArrowRect.HasValue && this.leftArrowRect.Value.Contains(position))
      {
        this.menuBase.CursorSelectable = true;
        if (this.MouseState.LeftButton.State == MouseButtonStates.Pressed)
          this.TryPageDown();
      }
      if (!this.leaderboard.InError && !this.leaderboard.Reading)
        return;
      this.XButtonString = (string) null;
    }

    private void TryPageUp()
    {
      if (this.leaderboard.Reading || !this.leaderboard.CanPageDown)
        return;
      this.leaderboard.PageDown(new Action(this.Refresh));
    }

    private void TryPageDown()
    {
      if (this.leaderboard.Reading || !this.leaderboard.CanPageUp)
        return;
      this.leaderboard.PageUp(new Action(this.Refresh));
    }

    private void CheckLive()
    {
      if (this.leaderboard != null)
        this.leaderboard.ActiveGamer = this.GameState.ActiveGamer;
      this.ChangeView();
    }

    private void InitLeaderboards()
    {
      this.leaderboard = new CachedLeaderboard(this.GameState.ActiveGamer, 10);
      this.OnScrollDown = (Action) (() =>
      {
        if (this.leaderboard.InError || !this.leaderboard.CanPageDown)
          return;
        this.moveToTop = true;
        this.leaderboard.PageDown(new Action(this.Refresh));
      });
      this.OnScrollUp = (Action) (() =>
      {
        if (this.leaderboard.InError || !this.leaderboard.CanPageUp)
          return;
        this.moveToLast = true;
        this.leaderboard.PageUp(new Action(this.Refresh));
      });
      this.ChangeView();
      this.GameState.LiveConnectionChanged += new Action(this.CheckLive);
    }

    public override void Dispose()
    {
      if (this.leaderboard != null)
        this.GameState.LiveConnectionChanged -= new Action(this.CheckLive);
      this.OnScrollDown = (Action) null;
      this.OnScrollUp = (Action) null;
      this.OnClose = (Action) null;
    }

    private void ChangeView()
    {
      lock (this)
      {
        this.Items.Clear();
        this.AddItem("LoadingLeaderboard", new Action(Util.NullAction), -1);
      }
      this.leaderboard.ChangeView(this.view, new Action(this.Refresh));
    }

    public override void PostDraw(SpriteBatch batch, SpriteFont font, GlyphTextRenderer tr, float alpha)
    {
      if (this.leaderboard == null)
        this.InitLeaderboards();
      float viewScale = SettingsManager.GetViewScale(batch.GraphicsDevice);
      if (!this.leaderboard.InError)
        tr.DrawString(batch, font, string.Format(StaticText.GetString("LeaderboardEntriesCount").ToUpper(CultureInfo.InvariantCulture), (object) this.leaderboard.TotalEntries), new Vector2(125f, 620f) * viewScale, new Color(1f, 1f, 1f, alpha), (Culture.IsCJK ? 0.2f : 1.5f) * viewScale);
      float num1 = this.leaderboard.InError || this.leaderboard.Reading ? 0.0f : (this.leaderboard.CanPageUp ? 1f : 0.1f);
      float num2 = this.leaderboard.InError || this.leaderboard.Reading ? 0.0f : (this.leaderboard.CanPageDown ? 1f : 0.1f);
      float num3 = Culture.IsCJK ? -15f : 0.0f;
      if (this.Items.Count > 1)
      {
        int num4 = ServiceHelper.Game.GraphicsDevice.Viewport.Width / 2 - (int) viewScale * 20;
        int y = ServiceHelper.Game.GraphicsDevice.Viewport.Height / 2;
        this.leftArrowRect = new Rectangle?(new Rectangle((int) ((double) num4 - (double) num4 * 5.0 / 7.0 + (double) num3 - (double) viewScale * 10.0), y, (int) (40.0 * (double) viewScale), (int) (25.0 * (double) viewScale)));
        this.rightArrowRect = new Rectangle?(new Rectangle((int) ((double) num4 + (double) num4 * 5.0 / 7.0 + (double) num3), y, (int) (40.0 * (double) viewScale), (int) (25.0 * (double) viewScale)));
        tr.DrawString(batch, font, "{LA}", new Vector2((float) this.leftArrowRect.Value.Left + 15f * viewScale, (float) this.leftArrowRect.Value.Top), new Color(1f, 1f, 1f, num1 * alpha), (Culture.IsCJK ? 0.2f : 1f) * viewScale);
        tr.DrawString(batch, font, "{RA}", new Vector2((float) this.rightArrowRect.Value.Left + 15f * viewScale, (float) this.rightArrowRect.Value.Top), new Color(1f, 1f, 1f, num2 * alpha), (Culture.IsCJK ? 0.2f : 1f) * viewScale);
      }
      else
      {
        LeaderboardsMenuLevel leaderboardsMenuLevel1 = this;
        LeaderboardsMenuLevel leaderboardsMenuLevel2 = this;
        LeaderboardsMenuLevel leaderboardsMenuLevel3 = this;
        Rectangle? nullable1 = new Rectangle?();
        Rectangle? nullable2 = nullable1;
        leaderboardsMenuLevel3.rightArrowRect = nullable2;
        Rectangle? nullable3;
        Rectangle? nullable4 = nullable3 = nullable1;
        leaderboardsMenuLevel2.rightArrowRect = nullable3;
        Rectangle? nullable5 = nullable4;
        leaderboardsMenuLevel1.leftArrowRect = nullable5;
      }
      if (!this.leaderboard.CanPageUp)
        this.leftArrowRect = new Rectangle?();
      if (!this.leaderboard.CanPageDown)
        this.rightArrowRect = new Rectangle?();
      if (!this.leaderboard.ChangingPage)
        return;
      tr.DrawCenteredString(batch, font, StaticText.GetString("LoadingLeaderboard"), new Color(1f, 1f, 1f, alpha), new Vector2(0.0f, 205f) * viewScale, (Culture.IsCJK ? 2f : 1f) * viewScale);
    }

    private void Refresh()
    {
      lock (this)
      {
        int local_0 = this.SelectedIndex;
        this.Items.Clear();
        bool local_1 = false;
        if (this.leaderboard.InError)
        {
          this.AddItem("LeaderboardsNeedLIVE", new Action(Util.NullAction), -1);
        }
        else
        {
          this.SelectedIndex = 0;
          bool local_2 = false;
          int local_3 = 0;
          foreach (LeaderboardEntry item_1 in this.leaderboard.Entries)
          {
            MenuItem local_5 = this.AddItem((string) null, (Action) (() => {}), !local_2, -1);
            LeaderboardEntry e = item_1;
            string personaName = Encoding.UTF8.GetString(Encoding.GetEncoding("iso-8859-1").GetBytes(e.PersonaName));
            local_5.SuffixText = (Func<string>) (() => (string) (object) e.GlobalRank + (object) ". " + personaName + " : " + (string) (object) Math.Round((double) e.Score / 32.0 * 100.0, 1) + " %");
            local_5.Hovered = !local_2;
            local_2 = true;
            if (this.leaderboard.View == LeaderboardView.MyScore && e.PersonaName == CommunityExpress.Instance.User.PersonaName)
            {
              foreach (MenuItem item_0 in this.Items)
                item_0.Hovered = false;
              local_5.Hovered = true;
              this.SelectedIndex = local_3;
              local_1 = true;
            }
            ++local_3;
          }
        }
        if (!local_1)
          this.SelectedIndex = Math.Min(local_0, this.Items.Count - 1);
        if (this.moveToLast)
        {
          this.SelectedIndex = this.Items.Count - 1;
          this.moveToLast = false;
        }
        if (this.moveToTop)
        {
          this.SelectedIndex = 0;
          this.moveToTop = false;
        }
        if (this.Items.Count > 0 && this.SelectedIndex == -1)
          this.SelectedIndex = 0;
        if (!this.leaderboard.InError)
        {
          if (this.Items.Count == 0)
          {
            this.AddItem(this.view == LeaderboardView.MyScore ? "NotRankedInLeaderboard" : "NoEntriesLeaderboard", new Action(Util.NullAction), -1);
            this.SelectedIndex = -1;
          }
          else
          {
            while (this.Items.Count < 10)
              this.AddItem((string) null, new Action(Util.NullAction), -1);
          }
        }
        this.XButtonString = "{X} " + string.Format(StaticText.GetString("CurrentLeaderboardView"), (object) StaticText.GetString((string) (object) this.view + (object) "LeaderboardView"));
      }
    }
  }
}
