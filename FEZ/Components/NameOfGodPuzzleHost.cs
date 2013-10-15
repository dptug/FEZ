// Type: FezGame.Components.NameOfGodPuzzleHost
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine;
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
  internal class NameOfGodPuzzleHost : GameComponent
  {
    private static readonly int[] SpiralTraversal = new int[8]
    {
      -1,
      1,
      -3,
      3,
      -5,
      5,
      -7,
      7
    };
    private static readonly Vector3 PuzzleCenter = new Vector3(13.5f, 57.5f, 14.5f);
    private static readonly NameOfGodPuzzleHost.ZuishSlot[] Slots = new NameOfGodPuzzleHost.ZuishSlot[8]
    {
      new NameOfGodPuzzleHost.ZuishSlot("ZUISH_BLOCKS_0", FaceOrientation.Back),
      new NameOfGodPuzzleHost.ZuishSlot("ZUISH_BLOCKS_4", FaceOrientation.Front),
      new NameOfGodPuzzleHost.ZuishSlot("ZUISH_BLOCKS_1", FaceOrientation.Right),
      new NameOfGodPuzzleHost.ZuishSlot("ZUISH_BLOCKS_0", FaceOrientation.Front),
      new NameOfGodPuzzleHost.ZuishSlot("ZUISH_BLOCKS_1", FaceOrientation.Right),
      new NameOfGodPuzzleHost.ZuishSlot("ZUISH_BLOCKS_5", FaceOrientation.Back),
      new NameOfGodPuzzleHost.ZuishSlot("ZUISH_BLOCKS_2", FaceOrientation.Back),
      new NameOfGodPuzzleHost.ZuishSlot("ZUISH_BLOCKS_1", FaceOrientation.Back)
    };
    private int RightPositions;
    private List<TrileInstance> Blocks;

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

    static NameOfGodPuzzleHost()
    {
    }

    public NameOfGodPuzzleHost(Game game)
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
      if (this.LevelManager.Name == "ZU_ZUISH")
      {
        if (this.GameState.SaveData.ThisLevel.InactiveArtObjects.Contains(0))
        {
          foreach (TrileInstance instance in Enumerable.ToArray<TrileInstance>(Enumerable.Where<TrileInstance>((IEnumerable<TrileInstance>) this.LevelManager.Triles.Values, (Func<TrileInstance, bool>) (x => x.Trile.ActorSettings.Type == ActorType.PickUp))))
          {
            instance.PhysicsState = (InstancePhysicsState) null;
            this.LevelManager.ClearTrile(instance);
          }
          if (!this.GameState.SaveData.ThisLevel.DestroyedTriles.Contains(new TrileEmplacement(NameOfGodPuzzleHost.PuzzleCenter + Vector3.UnitY * 2f + Vector3.UnitZ - FezMath.HalfVector)))
          {
            Trile trile = Enumerable.FirstOrDefault<Trile>(this.LevelManager.ActorTriles(ActorType.PieceOfHeart));
            if (trile != null)
            {
              Vector3 position = NameOfGodPuzzleHost.PuzzleCenter + Vector3.UnitY * 2f - FezMath.HalfVector + Vector3.UnitZ;
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
      this.Blocks.AddRange(Enumerable.Where<TrileInstance>((IEnumerable<TrileInstance>) this.LevelManager.Triles.Values, (Func<TrileInstance, bool>) (x => x.Trile.ActorSettings.Type == ActorType.PickUp)));
    }

    public override void Update(GameTime gameTime)
    {
      if (this.GameState.Paused || this.GameState.InMap || (!this.CameraManager.ActionRunning || !FezMath.IsOrthographic(this.CameraManager.Viewpoint)) || this.GameState.Loading)
        return;
      Vector3 b1 = FezMath.SideMask(this.CameraManager.Viewpoint);
      Vector3 depthMask = FezMath.DepthMask(this.CameraManager.Viewpoint);
      Vector3 vector3_1 = FezMath.ScreenSpaceMask(this.CameraManager.Viewpoint);
      if (this.RightPositions == 8)
      {
        bool flag = false;
        foreach (TrileInstance instance in this.Blocks)
        {
          if (!flag)
          {
            ServiceHelper.AddComponent((IGameComponent) new GlitchyDespawner(this.Game, instance, NameOfGodPuzzleHost.PuzzleCenter + Vector3.UnitY * 2f + Vector3.UnitZ)
            {
              FlashOnSpawn = true,
              ActorToSpawn = ActorType.PieceOfHeart
            });
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
        Vector3 b2 = FezMath.RightVector(this.CameraManager.Viewpoint);
        foreach (TrileInstance trileInstance1 in this.Blocks)
        {
          float num = FezMath.Dot(trileInstance1.Center, b2);
          trileInstance1.LastTreasureSin = 0.0f;
          foreach (TrileInstance trileInstance2 in this.Blocks)
          {
            if (trileInstance2 != trileInstance1 && (double) num > (double) FezMath.Dot(trileInstance2.Center, b2))
              ++trileInstance1.LastTreasureSin;
          }
        }
        using (List<TrileInstance>.Enumerator enumerator = this.Blocks.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            TrileInstance instance = enumerator.Current;
            if (!instance.PhysicsState.Background && instance.Enabled && ((double) instance.Position.Y >= 57.0 && (double) instance.Position.Y < 57.75) && (this.PlayerManager.HeldInstance != instance && this.PlayerManager.PushedInstance != instance && instance.PhysicsState.Grounded) && instance.PhysicsState.Ground.First != this.PlayerManager.PushedInstance)
            {
              Vector3 vector3_2 = Vector3.Min(Vector3.Max(FezMath.Round((instance.Center - NameOfGodPuzzleHost.PuzzleCenter) / 1f), new Vector3(-8f, 0.0f, -8f)), new Vector3(8f, 1f, 8f));
              Vector3 vector3_3 = NameOfGodPuzzleHost.PuzzleCenter + vector3_2 * 1f;
              Vector3 vector3_4 = (vector3_3 - instance.Center) * FezMath.XZMask;
              float num1 = Math.Max(vector3_4.Length(), 0.1f);
              instance.PhysicsState.Velocity += 0.25f * vector3_4 / num1 * (float) gameTime.ElapsedGameTime.TotalSeconds;
              if ((double) num1 <= 0.100000001490116 && (double) vector3_2.Y == 0.0 && instance != this.PlayerManager.Ground.First && !Enumerable.Any<TrileInstance>((IEnumerable<TrileInstance>) this.Blocks, (Func<TrileInstance, bool>) (x => x.PhysicsState.Ground.First == instance)) && Enumerable.Any<TrileInstance>((IEnumerable<TrileInstance>) this.Blocks, (Func<TrileInstance, bool>) (b =>
              {
                if (b != instance && FezMath.AlmostEqual(b.Position.Y, instance.Position.Y))
                  return (double) Math.Abs(FezMath.Dot(b.Center - instance.Center, depthMask)) < 1.0;
                else
                  return false;
              })))
              {
                instance.Enabled = false;
                for (int index = 0; index < NameOfGodPuzzleHost.SpiralTraversal.Length; ++index)
                {
                  int num2 = NameOfGodPuzzleHost.SpiralTraversal[index];
                  NearestTriles nearestTriles = this.LevelManager.NearestTrile(NameOfGodPuzzleHost.PuzzleCenter + (float) num2 * 1f * depthMask, QueryOptions.None, new Viewpoint?(FezMath.GetRotatedView(this.CameraManager.Viewpoint, 1)));
                  if (nearestTriles.Deep == null)
                    nearestTriles.Deep = this.LevelManager.NearestTrile(NameOfGodPuzzleHost.PuzzleCenter + (float) num2 * 1f * depthMask - depthMask * 0.5f, QueryOptions.None, new Viewpoint?(FezMath.GetRotatedView(this.CameraManager.Viewpoint, 1))).Deep;
                  if (nearestTriles.Deep == null)
                    nearestTriles.Deep = this.LevelManager.NearestTrile(NameOfGodPuzzleHost.PuzzleCenter + (float) num2 * 1f * depthMask + depthMask * 0.5f, QueryOptions.None, new Viewpoint?(FezMath.GetRotatedView(this.CameraManager.Viewpoint, 1))).Deep;
                  if (nearestTriles.Deep == null)
                  {
                    vector3_3 = instance.PhysicsState.Center = vector3_1 * instance.PhysicsState.Center + NameOfGodPuzzleHost.PuzzleCenter * depthMask + depthMask * (float) num2 * 1f;
                    break;
                  }
                }
                instance.Enabled = true;
              }
              if ((double) Math.Abs(vector3_4.X) <= 1.0 / 64.0 && (double) Math.Abs(vector3_4.Y) <= 1.0 / 64.0)
              {
                instance.PhysicsState.Velocity = Vector3.Zero;
                instance.PhysicsState.Center = vector3_3;
                instance.PhysicsState.UpdateInstance();
                this.LevelManager.UpdateInstance(instance);
              }
              if ((instance.PhysicsState.Ground.NearLow == null || instance.PhysicsState.Ground.NearLow.PhysicsState != null && (double) Math.Abs(FezMath.Dot(instance.PhysicsState.Ground.NearLow.Center - instance.Center, b1)) > 0.875) && (instance.PhysicsState.Ground.FarHigh == null || instance.PhysicsState.Ground.FarHigh.PhysicsState != null && (double) Math.Abs(FezMath.Dot(instance.PhysicsState.Ground.FarHigh.Center - instance.Center, b1)) > 0.875))
              {
                instance.PhysicsState.Ground = new MultipleHits<TrileInstance>();
                instance.PhysicsState.Center += Vector3.Down * 0.1f;
              }
              if (instance.PhysicsState.Grounded && instance.PhysicsState.Velocity == Vector3.Zero)
              {
                string cubemapPath = instance.Trile.CubemapPath;
                FaceOrientation o = FezMath.OrientationFromPhi(FezMath.ToPhi(this.CameraManager.Viewpoint) + instance.Phi);
                switch (o)
                {
                  case FaceOrientation.Right:
                  case FaceOrientation.Left:
                    o = FezMath.GetOpposite(o);
                    break;
                }
                int index = (int) MathHelper.Clamp(instance.LastTreasureSin, 0.0f, 7f);
                if (NameOfGodPuzzleHost.Slots[index].Face == o && NameOfGodPuzzleHost.Slots[index].TrileName == cubemapPath)
                  ++this.RightPositions;
              }
            }
          }
        }
      }
    }

    private struct ZuishSlot
    {
      public readonly string TrileName;
      public readonly FaceOrientation Face;

      public ZuishSlot(string trileName, FaceOrientation face)
      {
        this.TrileName = trileName;
        this.Face = face;
      }
    }
  }
}
