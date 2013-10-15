// Type: FezGame.Structure.SaveManagementLevel
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using EasyStorage;
using FezEngine.Components;
using FezEngine.Tools;
using FezGame.Components;
using FezGame.Services;
using FezGame.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Globalization;

namespace FezGame.Structure
{
  internal class SaveManagementLevel : MenuLevel
  {
    private readonly SaveSlotInfo[] Slots = new SaveSlotInfo[3];
    private readonly MenuBase Menu;
    private SaveSlotInfo CopySourceSlot;
    private MenuLevel CopyDestLevel;
    private IGameStateManager GameState;
    private IFontManager FontManager;

    public SaveManagementLevel(MenuBase menu)
    {
      this.Menu = menu;
    }

    public override void Initialize()
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      SaveManagementLevel.\u003C\u003Ec__DisplayClassd cDisplayClassd = new SaveManagementLevel.\u003C\u003Ec__DisplayClassd();
      // ISSUE: reference to a compiler-generated field
      cDisplayClassd.\u003C\u003E4__this = this;
      base.Initialize();
      this.FontManager = ServiceHelper.Get<IFontManager>();
      this.GameState = ServiceHelper.Get<IGameStateManager>();
      this.ReloadSlots();
      this.Title = "SaveManagementTitle";
      // ISSUE: reference to a compiler-generated field
      cDisplayClassd.sf = this.FontManager.Small;
      // ISSUE: reference to a compiler-generated field
      cDisplayClassd.changeLevel = new MenuLevel()
      {
        Title = "SaveChangeSlot",
        AButtonString = "ChangeWithGlyph"
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      cDisplayClassd.changeLevel.OnPostDraw = new Action<SpriteBatch, SpriteFont, GlyphTextRenderer, float>(cDisplayClassd.\u003CInitialize\u003Eb__4);
      // ISSUE: reference to a compiler-generated field
      cDisplayClassd.changeLevel.Parent = (MenuLevel) this;
      // ISSUE: reference to a compiler-generated field
      cDisplayClassd.changeLevel.Initialize();
      // ISSUE: reference to a compiler-generated field
      cDisplayClassd.copySrcLevel = new MenuLevel()
      {
        Title = "SaveCopySourceTitle",
        AButtonString = "ChooseWithGlyph"
      };
      // ISSUE: reference to a compiler-generated field
      cDisplayClassd.copySrcLevel.Parent = (MenuLevel) this;
      // ISSUE: reference to a compiler-generated field
      cDisplayClassd.copySrcLevel.Initialize();
      this.CopyDestLevel = new MenuLevel()
      {
        Title = "SaveCopyDestTitle",
        AButtonString = "ChooseWithGlyph"
      };
      // ISSUE: reference to a compiler-generated method
      this.CopyDestLevel.OnPostDraw = new Action<SpriteBatch, SpriteFont, GlyphTextRenderer, float>(cDisplayClassd.\u003CInitialize\u003Eb__5);
      // ISSUE: reference to a compiler-generated field
      this.CopyDestLevel.Parent = cDisplayClassd.copySrcLevel;
      this.CopyDestLevel.Initialize();
      // ISSUE: reference to a compiler-generated field
      cDisplayClassd.clearLevel = new MenuLevel()
      {
        Title = "SaveClearTitle",
        AButtonString = "ChooseWithGlyph"
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      cDisplayClassd.clearLevel.OnPostDraw = new Action<SpriteBatch, SpriteFont, GlyphTextRenderer, float>(cDisplayClassd.\u003CInitialize\u003Eb__6);
      // ISSUE: reference to a compiler-generated field
      cDisplayClassd.clearLevel.Parent = (MenuLevel) this;
      // ISSUE: reference to a compiler-generated field
      cDisplayClassd.clearLevel.Initialize();
      // ISSUE: reference to a compiler-generated method
      this.AddItem("SaveChangeSlot", new Action(cDisplayClassd.\u003CInitialize\u003Eb__7), -1);
      // ISSUE: reference to a compiler-generated method
      this.AddItem("SaveCopyTitle", new Action(cDisplayClassd.\u003CInitialize\u003Eb__9), -1);
      // ISSUE: reference to a compiler-generated method
      this.AddItem("SaveClearTitle", new Action(cDisplayClassd.\u003CInitialize\u003Eb__b), -1);
    }

    private void DrawWarning(SpriteBatch batch, SpriteFont font, GlyphTextRenderer tr, float alpha, string locString)
    {
      float scale = this.FontManager.SmallFactor * SettingsManager.GetViewScale(batch.GraphicsDevice);
      float num = (float) batch.GraphicsDevice.Viewport.Height / 2f;
      tr.DrawCenteredString(batch, font, StaticText.GetString(locString), new Color(1f, 1f, 1f, alpha), new Vector2(0.0f, num * 1.5f), scale);
    }

    private void ReloadSlots()
    {
      for (int index = 0; index < 3; ++index)
      {
        SaveSlotInfo saveSlotInfo = this.Slots[index] = new SaveSlotInfo()
        {
          Index = index
        };
        PCSaveDevice pcSaveDevice = new PCSaveDevice("FEZ");
        string fileName = "SaveSlot" + (object) index;
        if (!pcSaveDevice.FileExists(fileName))
        {
          saveSlotInfo.Empty = true;
        }
        else
        {
          SaveData saveData = (SaveData) null;
          if (!pcSaveDevice.Load(fileName, (LoadAction) (stream => saveData = SaveFileOperations.Read(new CrcReader(stream)))) || saveData == null)
          {
            saveSlotInfo.Empty = true;
          }
          else
          {
            saveSlotInfo.Percentage = (float) (((double) (saveData.CubeShards + saveData.SecretCubes + saveData.PiecesOfHeart) + (double) saveData.CollectedParts / 8.0) / 32.0);
            saveSlotInfo.PlayTime = new TimeSpan(saveData.PlayTime);
            saveSlotInfo.SaveData = saveData;
          }
        }
      }
    }

    private void RefreshSlotsFor(MenuLevel level, SaveManagementLevel.SMOperation operation, Func<SaveSlotInfo, bool> condition)
    {
      level.Items.Clear();
      foreach (SaveSlotInfo saveSlotInfo in this.Slots)
      {
        SaveSlotInfo s = saveSlotInfo;
        MenuItem menuItem;
        if (saveSlotInfo.Empty)
          (menuItem = level.AddItem((string) null, (Action) (() => this.ChooseSaveSlot(s, operation)), -1)).SuffixText = (Func<string>) (() => StaticText.GetString("NewSlot"));
        else
          (menuItem = level.AddItem("SaveSlotPrefix", (Action) (() => this.ChooseSaveSlot(s, operation)), -1)).SuffixText = (Func<string>) (() => string.Format((IFormatProvider) CultureInfo.InvariantCulture, " {0} ({1:P1} - {2:dd\\.hh\\:mm})", (object) (s.Index + 1), (object) s.Percentage, (object) s.PlayTime));
        menuItem.Disabled = !condition(saveSlotInfo);
        menuItem.Selectable = condition(saveSlotInfo);
      }
      for (int index = 0; index < this.Items.Count; ++index)
      {
        if (level.Items[index].Selectable)
        {
          level.SelectedIndex = index;
          break;
        }
      }
    }

    private void ChooseSaveSlot(SaveSlotInfo slot, SaveManagementLevel.SMOperation operation)
    {
      switch (operation)
      {
        case SaveManagementLevel.SMOperation.Change:
          this.GameState.SaveSlot = slot.Index;
          this.GameState.LoadSaveFile((Action) (() =>
          {
            this.GameState.Save();
            this.GameState.DoSave();
            this.GameState.SaveToCloud(true);
            this.GameState.Restart();
          }));
          break;
        case SaveManagementLevel.SMOperation.CopySource:
          this.CopySourceSlot = slot;
          this.RefreshSlotsFor(this.CopyDestLevel, SaveManagementLevel.SMOperation.CopyDestination, (Func<SaveSlotInfo, bool>) (s => this.CopySourceSlot != s));
          this.Menu.ChangeMenuLevel(this.CopyDestLevel, false);
          break;
        case SaveManagementLevel.SMOperation.CopyDestination:
          new PCSaveDevice("FEZ").Save("SaveSlot" + (object) slot.Index, (SaveAction) (writer => SaveFileOperations.Write(new CrcWriter(writer), this.CopySourceSlot.SaveData)));
          this.GameState.SaveToCloud(true);
          this.ReloadSlots();
          this.Menu.ChangeMenuLevel((MenuLevel) this, false);
          break;
        case SaveManagementLevel.SMOperation.Clear:
          new PCSaveDevice("FEZ").Delete("SaveSlot" + (object) slot.Index);
          if (this.GameState.SaveSlot == slot.Index)
          {
            this.GameState.LoadSaveFile((Action) (() =>
            {
              this.GameState.Save();
              this.GameState.DoSave();
              this.GameState.SaveToCloud(true);
              this.GameState.Restart();
            }));
            break;
          }
          else
          {
            this.ReloadSlots();
            this.Menu.ChangeMenuLevel((MenuLevel) this, false);
            break;
          }
      }
    }

    private enum SMOperation
    {
      Change,
      CopySource,
      CopyDestination,
      Clear,
    }
  }
}
