// Type: FezGame.Components.CryptHost
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine.Services.Scripting;
using FezEngine.Structure;
using FezEngine.Structure.Scripting;
using FezEngine.Tools;
using FezGame.Services;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FezGame.Components
{
  internal class CryptHost : GameComponent
  {
    private static readonly int[] VolumeSequence = new int[4]
    {
      12,
      36,
      42,
      14
    };
    private readonly List<int> TraversedVolumes = new List<int>();
    private bool isHooked;

    [ServiceDependency]
    public ILevelService LevelService { get; set; }

    [ServiceDependency]
    public IGomezService Gomez { private get; set; }

    [ServiceDependency]
    public IPlayerManager PlayerManager { private get; set; }

    [ServiceDependency]
    public IGameLevelManager LevelManager { private get; set; }

    static CryptHost()
    {
    }

    public CryptHost(Game game)
      : base(game)
    {
    }

    public override void Initialize()
    {
      this.Enabled = false;
      this.TryInitialize();
      this.LevelManager.LevelChanging += new Action(this.TryInitialize);
      this.LevelManager.LevelChanged += new Action(this.TryHook);
    }

    private void TryInitialize()
    {
      if (this.isHooked)
      {
        this.Gomez.EnteredDoor -= new Action(this.CheckWinCondition);
        this.isHooked = false;
      }
      if (this.LevelManager.Name != "CRYPT")
      {
        this.TraversedVolumes.Clear();
      }
      else
      {
        if (this.LevelManager.LastLevelName == "CRYPT")
        {
          this.TraversedVolumes.Add(this.PlayerManager.DoorVolume.Value);
          if (this.TraversedVolumes.Count > 4)
            this.TraversedVolumes.RemoveAt(0);
          for (int index = 0; index < this.TraversedVolumes.Count; ++index)
          {
            if (CryptHost.VolumeSequence[this.TraversedVolumes.Count - 1 - index] != this.TraversedVolumes[this.TraversedVolumes.Count - 1 - index])
            {
              this.TraversedVolumes.Clear();
              break;
            }
          }
        }
        else
          this.TraversedVolumes.Clear();
        ICollection<int> keys = this.LevelManager.Scripts.Keys;
        int[] numArray = new int[4]
        {
          0,
          1,
          2,
          3
        };
        foreach (int key in Enumerable.ToArray<int>(Enumerable.Except<int>((IEnumerable<int>) keys, (IEnumerable<int>) numArray)))
          this.LevelManager.Scripts.Remove(key);
        foreach (Volume volume in (IEnumerable<Volume>) this.LevelManager.Volumes.Values)
        {
          if (volume.Id > 1 && (volume.Id != 14 || this.TraversedVolumes.Count != 3))
          {
            int key = IdentifierPool.FirstAvailable<Script>(this.LevelManager.Scripts);
            int num = RandomHelper.InList<int>(Enumerable.Except<int>((IEnumerable<int>) this.LevelManager.Volumes.Keys, (IEnumerable<int>) new int[3]
            {
              0,
              1,
              volume.Id
            }));
            Script script1 = new Script();
            script1.Id = key;
            List<ScriptTrigger> triggers = script1.Triggers;
            ScriptTrigger scriptTrigger1 = new ScriptTrigger();
            scriptTrigger1.Event = "Enter";
            scriptTrigger1.Object = new Entity()
            {
              Type = "Volume",
              Identifier = new int?(volume.Id)
            };
            ScriptTrigger scriptTrigger2 = scriptTrigger1;
            triggers.Add(scriptTrigger2);
            List<ScriptAction> actions = script1.Actions;
            ScriptAction scriptAction1 = new ScriptAction();
            scriptAction1.Operation = "ChangeLevelToVolume";
            scriptAction1.Arguments = new string[4]
            {
              "CRYPT",
              num.ToString(),
              "True",
              "False"
            };
            scriptAction1.Object = new Entity()
            {
              Type = "Level"
            };
            ScriptAction scriptAction2 = scriptAction1;
            actions.Add(scriptAction2);
            Script script2 = script1;
            foreach (ScriptAction scriptAction3 in script2.Actions)
              scriptAction3.Process();
            this.LevelManager.Scripts.Add(key, script2);
          }
        }
        this.LevelManager.Scripts[2].Disabled = this.TraversedVolumes.Count != 3;
      }
    }

    private void TryHook()
    {
      if (this.TraversedVolumes.Count != 3)
        return;
      this.Gomez.EnteredDoor += new Action(this.CheckWinCondition);
      this.isHooked = true;
    }

    private void CheckWinCondition()
    {
      if (this.PlayerManager.DoorVolume.Value == 14)
        this.LevelManager.Scripts[3].ScheduleEvalulation = true;
      this.Gomez.EnteredDoor -= new Action(this.CheckWinCondition);
      this.isHooked = false;
    }
  }
}
