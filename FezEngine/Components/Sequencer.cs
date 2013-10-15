// Type: FezEngine.Components.Sequencer
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using Common;
using FezEngine.Services;
using FezEngine.Structure;
using FezEngine.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FezEngine.Components
{
  public abstract class Sequencer : GameComponent
  {
    protected readonly Dictionary<TrileInstance, Sequencer.CrystalState> crystals = new Dictionary<TrileInstance, Sequencer.CrystalState>();
    private const float WarningTime = 0.45f;
    private const int WarningBlinks = 6;
    private TimeSpan measureLength;
    private int step;

    [ServiceDependency]
    public IEngineStateManager EngineState { protected get; set; }

    [ServiceDependency]
    public IDefaultCameraManager CameraManager { protected get; set; }

    [ServiceDependency]
    public ILevelManager LevelManager { protected get; set; }

    [ServiceDependency]
    public ILevelMaterializer LevelMaterializer { protected get; set; }

    [ServiceDependency]
    public IDebuggingBag DebuggingBag { protected get; set; }

    [ServiceDependency]
    public ISoundManager SoundManager { protected get; set; }

    [ServiceDependency]
    public IContentManagerProvider CMProvider { private get; set; }

    protected Sequencer(Game game)
      : base(game)
    {
    }

    public override void Initialize()
    {
      this.Enabled = false;
      this.LevelManager.LevelChanged += new Action(this.TryStartSequence);
      this.TryStartSequence();
    }

    private void TryStartSequence()
    {
      this.crystals.Clear();
      this.Enabled = this.LevelManager.SequenceSamplesPath != null && this.LevelManager.Song != null && this.LevelManager.BlinkingAlpha;
      if (!this.Enabled)
        return;
      foreach (TrileInstance key in Enumerable.Where<TrileInstance>((IEnumerable<TrileInstance>) this.LevelManager.Triles.Values, (Func<TrileInstance, bool>) (x => x.Trile.ActorSettings.Type == ActorType.Crystal)))
        this.crystals.Add(key, new Sequencer.CrystalState());
      this.StartSequence();
    }

    protected void StartSequence()
    {
      this.LoadCrystalSamples();
      this.UpdateTempo();
    }

    protected void UpdateTempo()
    {
      this.measureLength = TimeSpan.FromMinutes(1.0 / ((double) this.LevelManager.Song.Tempo / 4.0) * 4.0);
    }

    protected void LoadCrystalSamples()
    {
      ContentManager forLevel = this.CMProvider.GetForLevel(this.LevelManager.Name);
      string path1 = Path.Combine("Sounds", this.LevelManager.SequenceSamplesPath ?? "");
      foreach (TrileInstance index1 in Enumerable.Where<TrileInstance>((IEnumerable<TrileInstance>) this.crystals.Keys, (Func<TrileInstance, bool>) (x => x.ActorSettings.SequenceSampleName != null)))
      {
        Sequencer.CrystalState crystalState = this.crystals[index1];
        InstanceActorSettings actorSettings = index1.ActorSettings;
        try
        {
          crystalState.Sample = forLevel.Load<SoundEffect>(Path.Combine(path1, actorSettings.SequenceSampleName));
          if (actorSettings.SequenceAlternateSampleName != null)
            crystalState.AlternateSample = forLevel.Load<SoundEffect>(Path.Combine(path1, actorSettings.SequenceAlternateSampleName));
        }
        catch (Exception ex)
        {
          Logger.Log("Sequencer", LogSeverity.Warning, string.Concat(new object[4]
          {
            (object) "Could not find crystal sample : ",
            (object) crystalState.Sample,
            (object) " or ",
            (object) crystalState.AlternateSample
          }));
        }
        bool flag1 = actorSettings.Sequence[15];
        bool flag2 = false;
        for (int index2 = 0; index2 < 16; ++index2)
        {
          if (!flag1 && actorSettings.Sequence[index2])
          {
            crystalState.Alternate[index2] = flag2 && crystalState.AlternateSample != null;
            flag2 = !flag2;
          }
          flag1 = actorSettings.Sequence[index2];
        }
      }
    }

    public override void Update(GameTime gameTime)
    {
      if (!this.Enabled || this.EngineState.Loading || (this.EngineState.Paused || this.EngineState.InMap))
        return;
      double num1 = FezMath.Frac((double) this.SoundManager.PlayPosition.Ticks / (double) this.measureLength.Ticks);
      this.step = (int) Math.Floor(num1 * 16.0);
      double num2 = FezMath.Frac(num1 * 16.0);
      int index1 = (this.step + 1) % 16;
      bool flag = false;
      foreach (TrileInstance index2 in this.crystals.Keys)
      {
        if (index2.ActorSettings.Sequence != null)
        {
          Sequencer.CrystalState crystalState = this.crystals[index2];
          bool enabled = index2.Enabled;
          index2.Enabled = index2.ActorSettings.Sequence.Length > this.step && index2.ActorSettings.Sequence[this.step];
          if (!enabled && index2.Enabled)
          {
            this.LevelManager.RestoreTrile(index2);
            index2.Hidden = false;
            index2.Enabled = true;
            if (index2.InstanceId == -1)
              this.LevelMaterializer.CullInstanceIn(index2);
            if (crystalState.Sample != null)
            {
              Vector3 position = index2.Position + new Vector3(0.5f);
              SoundEffectExtensions.EmitAt(crystalState.Alternate[this.step] ? crystalState.AlternateSample : crystalState.Sample, position);
            }
            flag = true;
          }
          else if (enabled && !index2.Enabled)
          {
            this.LevelMaterializer.UnregisterViewedInstance(index2);
            this.LevelMaterializer.CullInstanceOut(index2, true);
            this.LevelManager.ClearTrile(index2, true);
            this.OnDisappear(index2);
            flag = true;
          }
          else if (num2 > 0.449999988079071 && index2.Enabled && (index2.ActorSettings.Sequence.Length > index1 && !index2.ActorSettings.Sequence[index1]))
          {
            if ((int) Math.Round((num2 - 0.449999988079071) / 0.550000011920929 * 6.0) % 3 == 0)
            {
              if (!index2.Hidden)
              {
                index2.Hidden = true;
                this.LevelMaterializer.UnregisterViewedInstance(index2);
                this.LevelMaterializer.CullInstanceOut(index2, true);
                flag = true;
              }
            }
            else if (index2.Hidden)
            {
              index2.Hidden = false;
              if (index2.InstanceId == -1)
              {
                this.LevelMaterializer.CullInstanceIn(index2);
                flag = true;
              }
            }
          }
        }
      }
      if (!flag)
        return;
      this.LevelMaterializer.CommitBatchesIfNeeded();
    }

    protected virtual void OnDisappear(TrileInstance crystal)
    {
    }

    protected class CrystalState
    {
      public SoundEffect Sample { get; set; }

      public SoundEffect AlternateSample { get; set; }

      public bool[] Alternate { get; private set; }

      public CrystalState()
      {
        this.Alternate = new bool[16];
      }
    }
  }
}
