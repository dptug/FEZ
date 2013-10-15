// Type: FezGame.Components.TetrisPuzzleHost
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine.Services;
using FezEngine.Services.Scripting;
using FezEngine.Structure;
using FezEngine.Tools;
using FezGame.Services;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FezGame.Components
{
  internal class TetrisPuzzleHost : GameComponent
  {
    private static readonly int[] SpiralTraversal = new int[5]
    {
      0,
      1,
      -1,
      2,
      -2
    };
    private static readonly Vector3 PuzzleCenter = new Vector3(14.5f, 19.5f, 13.5f);
    private static readonly Vector3 TwoHigh = new Vector3(-1f, 0.0f, 0.0f);
    private static readonly Vector3 Interchangeable1_1 = new Vector3(0.0f, 0.0f, -1f);
    private static readonly Vector3 Interchangeable1_2 = new Vector3(1f, 0.0f, 1f);
    private static readonly Vector3 Interchangeable2_1 = new Vector3(0.0f, 0.0f, 1f);
    private static readonly Vector3 Interchangeable2_2 = new Vector3(1f, 0.0f, -1f);
    private int RightPositions;
    private List<TrileInstance> Blocks;
    private float SinceSolved;
    private float SinceStarted;

    [ServiceDependency]
    public ILevelService LevelService { get; set; }

    [ServiceDependency]
    public ILevelMaterializer LevelMaterializer { get; set; }

    [ServiceDependency]
    public IPlayerManager PlayerManager { get; set; }

    [ServiceDependency]
    public IGameStateManager GameState { get; set; }

    [ServiceDependency]
    public IGameCameraManager CameraManager { get; set; }

    [ServiceDependency]
    public IGameLevelManager LevelManager { get; set; }

    static TetrisPuzzleHost()
    {
    }

    public TetrisPuzzleHost(Game game)
      : base(game)
    {
    }

    public override void Initialize()
    {
      base.Initialize();
      this.LevelManager.LevelChanged += new Action(this.TryInitialize);
      this.TryInitialize();
    }

    private void TryInitialize()
    {
      this.Blocks = (List<TrileInstance>) null;
      if (this.LevelManager.Name == "ZU_TETRIS")
      {
        if (this.GameState.SaveData.ThisLevel.InactiveArtObjects.Contains(0))
        {
          foreach (TrileInstance instance in Enumerable.ToArray<TrileInstance>(Enumerable.Where<TrileInstance>((IEnumerable<TrileInstance>) this.LevelManager.Triles.Values, (Func<TrileInstance, bool>) (x => x.Trile.ActorSettings.Type == ActorType.SinkPickup))))
          {
            instance.PhysicsState = (InstancePhysicsState) null;
            this.LevelManager.ClearTrile(instance);
          }
          if (!this.GameState.SaveData.ThisLevel.DestroyedTriles.Contains(new TrileEmplacement(TetrisPuzzleHost.PuzzleCenter + Vector3.UnitY - FezMath.HalfVector)))
          {
            Trile trile = Enumerable.FirstOrDefault<Trile>(this.LevelManager.ActorTriles(ActorType.SecretCube));
            if (trile != null)
            {
              Vector3 position = TetrisPuzzleHost.PuzzleCenter + Vector3.UnitY - FezMath.HalfVector;
              this.LevelManager.ClearTrile(new TrileEmplacement(position));
              TrileInstance toAdd;
              this.LevelManager.RestoreTrile(toAdd = new TrileInstance(position, trile.Id)
              {
                OriginalEmplacement = new TrileEmplacement(position)
              });
              if (toAdd.InstanceId == -1)
                this.LevelMaterializer.CullInstanceIn(toAdd);
            }
          }
          this.Enabled = false;
        }
        else
          this.Enabled = true;
      }
      else
        this.Enabled = false;
      if (!this.Enabled)
        return;
      this.Blocks = new List<TrileInstance>();
      this.Blocks.AddRange(Enumerable.Where<TrileInstance>((IEnumerable<TrileInstance>) this.LevelManager.Triles.Values, (Func<TrileInstance, bool>) (x => x.Trile.ActorSettings.Type == ActorType.SinkPickup)));
    }

    public override void Update(GameTime gameTime)
    {
      if (this.GameState.Paused || this.GameState.InMap || (!this.CameraManager.ActionRunning || !FezMath.IsOrthographic(this.CameraManager.Viewpoint)) || this.GameState.Loading)
        return;
      Vector3 depthMask = FezMath.DepthMask(this.CameraManager.Viewpoint);
      Vector3 vector3_1 = FezMath.ScreenSpaceMask(this.CameraManager.Viewpoint);
      if (this.RightPositions == 4)
      {
        this.SinceSolved += (float) gameTime.ElapsedGameTime.TotalSeconds;
        float num = FezMath.Saturate(this.SinceSolved);
        float amount = Easing.EaseIn((double) num, EasingType.Cubic);
        foreach (TrileInstance instance in this.Blocks)
        {
          Vector3 v = instance.Center - TetrisPuzzleHost.PuzzleCenter;
          if ((double) v.Length() > 0.5)
          {
            Vector3 vector3_2 = FezMath.Sign(FezMath.AlmostClamp(v, 0.1f));
            Vector3 vector3_3 = (TetrisPuzzleHost.PuzzleCenter + vector3_2 * 1.75f) * FezMath.XZMask + Vector3.UnitY * instance.Center;
            Vector3 vector3_4 = (TetrisPuzzleHost.PuzzleCenter + vector3_2) * FezMath.XZMask + Vector3.UnitY * instance.Center;
            instance.PhysicsState.Center = Vector3.Lerp(vector3_3, vector3_4, amount);
            instance.PhysicsState.UpdateInstance();
            this.LevelManager.UpdateInstance(instance);
          }
        }
        if ((double) num != 1.0)
          return;
        bool flag = false;
        foreach (TrileInstance instance in this.Blocks)
        {
          if (!flag)
          {
            ServiceHelper.AddComponent((IGameComponent) new GlitchyDespawner(this.Game, instance, TetrisPuzzleHost.PuzzleCenter + Vector3.UnitY));
            flag = true;
          }
          else
            ServiceHelper.AddComponent((IGameComponent) new GlitchyDespawner(this.Game, instance));
        }
        this.GameState.SaveData.ThisLevel.InactiveArtObjects.Add(0);
        foreach (Volume volume in Enumerable.Where<Volume>((IEnumerable<Volume>) this.LevelManager.Volumes.Values, (Func<Volume, bool>) (x =>
        {
          if (x.ActorSettings != null && x.ActorSettings.IsPointOfInterest)
            return x.Enabled;
          else
            return false;
        })))
        {
          volume.Enabled = false;
          this.GameState.SaveData.ThisLevel.InactiveVolumes.Add(volume.Id);
        }
        this.LevelService.ResolvePuzzle();
        this.Enabled = false;
      }
      else
      {
        this.RightPositions = 0;
        int num1 = 0;
        int num2 = 0;
        this.Blocks.Sort((Comparison<TrileInstance>) ((a, b) => b.LastTreasureSin.CompareTo(a.LastTreasureSin)));
        using (List<TrileInstance>.Enumerator enumerator = this.Blocks.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            TrileInstance instance = enumerator.Current;
            if (!instance.PhysicsState.Grounded)
              instance.LastTreasureSin = this.SinceStarted;
            if ((double) instance.Position.Y >= 19.0 && (double) instance.Position.Y <= 20.5 && (this.PlayerManager.HeldInstance != instance && this.PlayerManager.PushedInstance != instance) && instance.PhysicsState.Grounded && instance.PhysicsState.Ground.First != this.PlayerManager.PushedInstance)
            {
              Vector3 vector3_2 = Vector3.Min(Vector3.Max(FezMath.Round((instance.Center - TetrisPuzzleHost.PuzzleCenter) / 1.75f), new Vector3(-3f, 0.0f, -3f)), new Vector3(3f, 1f, 3f));
              Vector3 vector3_3 = (TetrisPuzzleHost.PuzzleCenter + vector3_2 * 1.75f - instance.Center) * FezMath.XZMask;
              float num3 = Math.Max(vector3_3.Length(), 0.1f);
              instance.PhysicsState.Velocity += 0.25f * vector3_3 / num3 * (float) gameTime.ElapsedGameTime.TotalSeconds;
              if ((double) num3 <= 0.100000001490116 && (double) vector3_2.Y == 0.0 && instance != this.PlayerManager.Ground.First && !Enumerable.Any<TrileInstance>((IEnumerable<TrileInstance>) this.Blocks, (Func<TrileInstance, bool>) (x => x.PhysicsState.Ground.First == instance)) && Enumerable.Any<TrileInstance>((IEnumerable<TrileInstance>) this.Blocks, (Func<TrileInstance, bool>) (b =>
              {
                if (b != instance && FezMath.AlmostEqual(b.Position.Y, instance.Position.Y))
                  return (double) Math.Abs(FezMath.Dot(b.Center - instance.Center, depthMask)) < 1.75;
                else
                  return false;
              })))
              {
                int num4 = Math.Sign(FezMath.Dot(instance.Center - TetrisPuzzleHost.PuzzleCenter, depthMask));
                if (num4 == 0)
                  num4 = 1;
                for (int index = -2; index <= 2; ++index)
                {
                  int num5 = TetrisPuzzleHost.SpiralTraversal[index + 2] * num4;
                  Vector3 tetativePosition = vector3_1 * instance.PhysicsState.Center + TetrisPuzzleHost.PuzzleCenter * depthMask + depthMask * (float) num5 * 1.75f;
                  if (Enumerable.All<TrileInstance>((IEnumerable<TrileInstance>) this.Blocks, (Func<TrileInstance, bool>) (b =>
                  {
                    if (b != instance && FezMath.AlmostEqual(b.Position.Y, instance.Position.Y))
                      return (double) Math.Abs(FezMath.Dot(b.Center - tetativePosition, depthMask)) >= 1.57499992847443;
                    else
                      return true;
                  })))
                  {
                    instance.PhysicsState.Center = tetativePosition;
                    break;
                  }
                }
              }
              if (this.RightPositions < 4 && (double) num3 <= 0.100000001490116 && instance.PhysicsState.Grounded)
              {
                if ((double) instance.Position.Y == 20.0)
                {
                  if ((double) vector3_2.X == (double) TetrisPuzzleHost.TwoHigh.X && (double) vector3_2.Z == (double) TetrisPuzzleHost.TwoHigh.Z)
                    this.RightPositions += 2;
                }
                else
                {
                  if ((double) vector3_2.X == (double) TetrisPuzzleHost.Interchangeable1_1.X && (double) vector3_2.Z == (double) TetrisPuzzleHost.Interchangeable1_1.Z || (double) vector3_2.X == (double) TetrisPuzzleHost.Interchangeable1_2.X && (double) vector3_2.Z == (double) TetrisPuzzleHost.Interchangeable1_2.Z)
                    ++num1;
                  if ((double) vector3_2.X == (double) TetrisPuzzleHost.Interchangeable2_1.X && (double) vector3_2.Z == (double) TetrisPuzzleHost.Interchangeable2_1.Z || (double) vector3_2.X == (double) TetrisPuzzleHost.Interchangeable2_2.X && (double) vector3_2.Z == (double) TetrisPuzzleHost.Interchangeable2_2.Z)
                    ++num2;
                }
              }
            }
          }
        }
        if (this.RightPositions >= 4 || num1 != 2 && num2 != 2)
          return;
        this.RightPositions += 2;
      }
    }
  }
}
