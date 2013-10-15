// Type: FezGame.Structure.SaveSlotSelectionLevel
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using EasyStorage;
using FezEngine.Tools;
using FezGame.Services;
using FezGame.Tools;
using System;

namespace FezGame.Structure
{
  internal class SaveSlotSelectionLevel : MenuLevel
  {
    private readonly SaveSlotInfo[] Slots = new SaveSlotInfo[3];
    public Func<bool> RecoverMainMenu;
    private IGameStateManager GameState;

    public override void Initialize()
    {
      base.Initialize();
      this.Title = "SaveSlotTitle";
      this.AButtonString = "ChooseWithGlyph";
      this.BButtonString = "ExitWithGlyph";
      this.GameState = ServiceHelper.Get<IGameStateManager>();
      for (int index = 0; index < 3; ++index)
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        SaveSlotSelectionLevel.\u003C\u003Ec__DisplayClassa cDisplayClassa = new SaveSlotSelectionLevel.\u003C\u003Ec__DisplayClassa();
        // ISSUE: reference to a compiler-generated field
        cDisplayClassa.\u003C\u003E4__this = this;
        // ISSUE: reference to a compiler-generated field
        cDisplayClassa.slot = this.Slots[index] = new SaveSlotInfo()
        {
          Index = index
        };
        PCSaveDevice pcSaveDevice = new PCSaveDevice("FEZ");
        string fileName = "SaveSlot" + (object) index;
        if (!pcSaveDevice.FileExists(fileName))
        {
          // ISSUE: reference to a compiler-generated field
          cDisplayClassa.slot.Empty = true;
        }
        else
        {
          // ISSUE: object of a compiler-generated type is created
          // ISSUE: variable of a compiler-generated type
          SaveSlotSelectionLevel.\u003C\u003Ec__DisplayClassc cDisplayClassc = new SaveSlotSelectionLevel.\u003C\u003Ec__DisplayClassc();
          // ISSUE: reference to a compiler-generated field
          cDisplayClassc.CS\u0024\u003C\u003E8__localsb = cDisplayClassa;
          // ISSUE: reference to a compiler-generated field
          cDisplayClassc.saveData = (SaveData) null;
          // ISSUE: reference to a compiler-generated method
          // ISSUE: reference to a compiler-generated field
          if (!pcSaveDevice.Load(fileName, new LoadAction(cDisplayClassc.\u003CInitialize\u003Eb__4)) || cDisplayClassc.saveData == null)
          {
            // ISSUE: reference to a compiler-generated field
            cDisplayClassa.slot.Empty = true;
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            cDisplayClassa.slot.Percentage = (float) (((double) (cDisplayClassc.saveData.CubeShards + cDisplayClassc.saveData.SecretCubes + cDisplayClassc.saveData.PiecesOfHeart) + (double) cDisplayClassc.saveData.CollectedParts / 8.0) / 32.0);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            cDisplayClassa.slot.PlayTime = new TimeSpan(cDisplayClassc.saveData.PlayTime);
          }
        }
        if (this.Slots[index].Empty)
        {
          // ISSUE: reference to a compiler-generated method
          this.AddItem((string) null, new Action(cDisplayClassa.\u003CInitialize\u003Eb__5), -1).SuffixText = (Func<string>) (() => StaticText.GetString("NewSlot"));
        }
        else
        {
          // ISSUE: reference to a compiler-generated method
          // ISSUE: reference to a compiler-generated method
          this.AddItem("SaveSlotPrefix", new Action(cDisplayClassa.\u003CInitialize\u003Eb__7), -1).SuffixText = new Func<string>(cDisplayClassa.\u003CInitialize\u003Eb__8);
        }
      }
    }

    private void ChooseSaveSlot(SaveSlotInfo slot)
    {
      this.GameState.SaveSlot = slot.Index;
      this.GameState.LoadSaveFile((Action) (() =>
      {
        this.GameState.Save();
        this.GameState.DoSave();
        if (this.RecoverMainMenu == null || !this.RecoverMainMenu())
          return;
        this.RecoverMainMenu = (Func<bool>) null;
      }));
    }
  }
}
