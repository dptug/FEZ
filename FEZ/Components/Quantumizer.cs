// Type: FezGame.Components.Quantumizer
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine;
using FezEngine.Services;
using FezEngine.Structure;
using FezEngine.Tools;
using FezGame.Services;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FezGame.Components
{
  public class Quantumizer : GameComponent
  {
    private static readonly Random Random = new Random();
    private readonly List<TrileInstance> BatchedInstances = new List<TrileInstance>();
    private readonly HashSet<TrileInstance> BatchedInstancesSet = new HashSet<TrileInstance>();
    private readonly List<TrileInstance> RandomInstances = new List<TrileInstance>();
    private readonly List<TrileInstance> CleanInstances = new List<TrileInstance>();
    private readonly List<Vector4> AllEmplacements = new List<Vector4>();
    private readonly HashSet<Point> SsPosToRecull = new HashSet<Point>();
    private int[] RandomTrileIds;
    private int FreezeFrames;

    [ServiceDependency]
    public IGameCameraManager CameraManager { private get; set; }

    [ServiceDependency]
    public IPlayerManager PlayerManager { private get; set; }

    [ServiceDependency]
    public IGameLevelManager LevelManager { private get; set; }

    [ServiceDependency]
    public ILevelMaterializer LevelMaterializer { private get; set; }

    [ServiceDependency]
    public IGameStateManager GameState { private get; set; }

    static Quantumizer()
    {
    }

    public Quantumizer(Game game)
      : base(game)
    {
      this.UpdateOrder = 1000;
    }

    public override void Initialize()
    {
      base.Initialize();
      this.LevelManager.LevelChanged += new Action(this.TryInitialize);
      this.TryInitialize();
    }

    private void TryInitialize()
    {
      this.BatchedInstances.Clear();
      this.BatchedInstancesSet.Clear();
      this.CleanInstances.Clear();
      this.AllEmplacements.Clear();
      this.AllEmplacements.TrimExcess();
      this.CleanInstances.TrimExcess();
      this.BatchedInstances.TrimExcess();
      this.RandomTrileIds = (int[]) null;
      if (this.Enabled)
        this.LevelMaterializer.TrileInstanceBatched -= new Action<TrileInstance>(this.BatchInstance);
      this.Enabled = false;
      if (!this.LevelManager.Quantum || this.LevelManager.TrileSet == null)
        return;
      this.Enabled = true;
      List<int> list1 = Enumerable.ToList<int>(Enumerable.Select<Trile, int>(Enumerable.Where<Trile>(this.LevelMaterializer.MaterializedTriles, (Func<Trile, bool>) (x =>
      {
        if (x.Geometry != null && !x.Geometry.Empty && !ActorTypeExtensions.IsTreasure(x.ActorSettings.Type))
          return x.ActorSettings.Type != ActorType.SplitUpCube;
        else
          return false;
      })), (Func<Trile, int>) (x => x.Id)));
      this.RandomTrileIds = new int[250];
      int num = 0;
      for (int index1 = 0; index1 < 250; ++index1)
      {
        int index2 = Quantumizer.Random.Next(0, list1.Count);
        int index3 = list1[index2];
        this.RandomTrileIds[num++] = index3;
        this.LevelManager.TrileSet[index3].ForceKeep = true;
        list1.RemoveAt(index2);
      }
      Trile trile = Enumerable.FirstOrDefault<Trile>((IEnumerable<Trile>) this.LevelManager.TrileSet.Triles.Values, (Func<Trile, bool>) (x => x.Name == "__QIPT"));
      if (trile == null)
      {
        trile = new Trile(CollisionType.None)
        {
          Name = "__QIPT",
          Immaterial = true,
          SeeThrough = true,
          Thin = true,
          TrileSet = this.LevelManager.TrileSet,
          MissingTrixels = (TrixelCluster) null,
          Id = IdentifierPool.FirstAvailable<Trile>((IDictionary<int, Trile>) this.LevelManager.TrileSet.Triles)
        };
        this.LevelManager.TrileSet.Triles.Add(trile.Id, trile);
        this.LevelMaterializer.RebuildTrile(trile);
      }
      List<int> list2 = new List<int>();
      bool flag = (double) this.LevelManager.Size.X > (double) this.LevelManager.Size.Z;
      float[] numArray = new float[4]
      {
        0.0f,
        1.570796f,
        3.141593f,
        4.712389f
      };
      for (int y = 0; (double) y < (double) this.LevelManager.Size.Y; ++y)
      {
        if (flag)
        {
          list2.Clear();
          list2.AddRange(Enumerable.Range(0, (int) this.LevelManager.Size.Z));
          for (int x = 0; (double) x < (double) this.LevelManager.Size.X; ++x)
          {
            int z;
            if (list2.Count > 0)
            {
              int index = RandomHelper.Random.Next(0, list2.Count);
              z = list2[index];
              list2.RemoveAt(index);
            }
            else
              z = RandomHelper.Random.Next(0, (int) this.LevelManager.Size.Z);
            this.LevelManager.RestoreTrile(new TrileInstance(new TrileEmplacement(x, y, z), trile.Id)
            {
              Phi = numArray[Quantumizer.Random.Next(0, 4)]
            });
          }
          while (list2.Count > 0)
          {
            int index = RandomHelper.Random.Next(0, list2.Count);
            int z = list2[index];
            list2.RemoveAt(index);
            this.LevelManager.RestoreTrile(new TrileInstance(new TrileEmplacement(RandomHelper.Random.Next(0, (int) this.LevelManager.Size.X), y, z), trile.Id)
            {
              Phi = numArray[Quantumizer.Random.Next(0, 4)]
            });
          }
        }
        else
        {
          list2.Clear();
          list2.AddRange(Enumerable.Range(0, (int) this.LevelManager.Size.X));
          for (int z = 0; (double) z < (double) this.LevelManager.Size.Z; ++z)
          {
            int x;
            if (list2.Count > 0)
            {
              int index = RandomHelper.Random.Next(0, list2.Count);
              x = list2[index];
              list2.RemoveAt(index);
            }
            else
              x = RandomHelper.Random.Next(0, (int) this.LevelManager.Size.X);
            this.LevelManager.RestoreTrile(new TrileInstance(new TrileEmplacement(x, y, z), trile.Id)
            {
              Phi = numArray[Quantumizer.Random.Next(0, 4)]
            });
          }
          while (list2.Count > 0)
          {
            int index = RandomHelper.Random.Next(0, list2.Count);
            int x = list2[index];
            list2.RemoveAt(index);
            this.LevelManager.RestoreTrile(new TrileInstance(new TrileEmplacement(x, y, RandomHelper.Random.Next(0, (int) this.LevelManager.Size.Z)), trile.Id)
            {
              Phi = numArray[Quantumizer.Random.Next(0, 4)]
            });
          }
        }
      }
      foreach (TrileInstance trileInstance in (IEnumerable<TrileInstance>) this.LevelManager.Triles.Values)
      {
        trileInstance.VisualTrileId = new int?(this.RandomTrileIds[Quantumizer.Random.Next(0, this.RandomTrileIds.Length)]);
        trileInstance.RefreshTrile();
        trileInstance.NeedsRandomCleanup = true;
      }
      this.LevelMaterializer.CleanUp();
      this.LevelMaterializer.TrileInstanceBatched += new Action<TrileInstance>(this.BatchInstance);
    }

    private void BatchInstance(TrileInstance instance)
    {
      if (!this.CameraManager.ViewTransitionReached || this.BatchedInstancesSet.Contains(instance))
        return;
      this.BatchedInstances.Add(instance);
      this.BatchedInstancesSet.Add(instance);
    }

    public override void Update(GameTime gameTime)
    {
      if (this.GameState.Loading || this.GameState.Paused || (this.GameState.InMap || this.GameState.InFpsMode) || (this.GameState.InMenuCube || !FezMath.IsOrthographic(this.CameraManager.Viewpoint)))
        return;
      Viewpoint viewpoint = this.CameraManager.Viewpoint;
      Vector3 vector3_1 = FezMath.ScreenSpaceMask(viewpoint);
      Vector3 vector3_2 = Vector3.One - vector3_1 + Vector3.UnitY;
      bool flag1 = FezMath.SideMask(viewpoint) == Vector3.Right;
      Vector3 position = this.PlayerManager.Position;
      this.RandomInstances.Clear();
      this.CleanInstances.Clear();
      this.AllEmplacements.Clear();
      for (int index = this.BatchedInstances.Count - 1; index >= 0; --index)
      {
        TrileInstance trileInstance = this.BatchedInstances[index];
        if (trileInstance.InstanceId == -1)
        {
          this.BatchedInstances.RemoveAt(index);
          this.BatchedInstancesSet.Remove(trileInstance);
        }
        else
        {
          Vector4 vector4 = trileInstance.Data.PositionPhi;
          Vector3 vector3_3 = position - new Vector3(vector4.X, vector4.Y, vector4.Z);
          if ((double) (vector3_3 * vector3_1).LengthSquared() > 30.0 && (double) (vector3_3 * vector3_2).LengthSquared() > 30.0)
          {
            this.AllEmplacements.Add(vector4);
            this.RandomInstances.Add(trileInstance);
          }
          else
            this.CleanInstances.Add(trileInstance);
        }
      }
      if (this.BatchedInstances.Count == 0)
        return;
      bool flag2 = false;
      if (this.FreezeFrames-- < 0)
      {
        if (RandomHelper.Probability(0.02))
          this.FreezeFrames = Quantumizer.Random.Next(0, 15);
      }
      else
        flag2 = true;
      bool transitionReached = this.CameraManager.ViewTransitionReached;
      if (RandomHelper.Probability(0.899999976158142))
      {
        int num = Quantumizer.Random.Next(0, flag2 ? this.RandomInstances.Count / 50 : this.RandomInstances.Count);
        while (num-- >= 0)
        {
          int count = this.RandomInstances.Count;
          int index1 = Quantumizer.Random.Next(0, count);
          TrileInstance trileInstance = this.RandomInstances[index1];
          this.RandomInstances.RemoveAt(index1);
          if (trileInstance.VisualTrileId.HasValue)
          {
            int trileId = trileInstance.TrileId;
            int? visualTrileId = trileInstance.VisualTrileId;
            if ((trileId != visualTrileId.GetValueOrDefault() ? 0 : (visualTrileId.HasValue ? 1 : 0)) == 0)
              goto label_20;
          }
          this.LevelMaterializer.CullInstanceOut(trileInstance);
          trileInstance.VisualTrileId = new int?(RandomHelper.InList<int>(this.RandomTrileIds));
          trileInstance.RefreshTrile();
          this.LevelMaterializer.CullInstanceIn(trileInstance);
label_20:
          trileInstance.NeedsRandomCleanup = true;
          if (trileInstance.InstanceId != -1)
          {
            int index2 = Quantumizer.Random.Next(0, count);
            Vector4 data = this.AllEmplacements[index2];
            this.AllEmplacements.RemoveAt(index2);
            this.LevelMaterializer.GetTrileMaterializer(trileInstance.VisualTrile).FakeUpdate(trileInstance.InstanceId, data);
          }
        }
      }
      this.SsPosToRecull.Clear();
      foreach (TrileInstance trileInstance in this.CleanInstances)
      {
        if (trileInstance.VisualTrileId.HasValue)
        {
          this.LevelMaterializer.CullInstanceOut(trileInstance);
          trileInstance.VisualTrileId = new int?();
          trileInstance.RefreshTrile();
          if (transitionReached)
          {
            TrileEmplacement emplacement = trileInstance.Emplacement;
            this.SsPosToRecull.Add(new Point(flag1 ? emplacement.X : emplacement.Z, emplacement.Y));
          }
          else
            this.LevelMaterializer.CullInstanceIn(trileInstance);
        }
        else if (trileInstance.NeedsRandomCleanup)
        {
          this.LevelMaterializer.GetTrileMaterializer(trileInstance.Trile).UpdateInstance(trileInstance);
          trileInstance.NeedsRandomCleanup = false;
        }
      }
      if (this.SsPosToRecull.Count > 0)
      {
        foreach (Point ssPos in this.SsPosToRecull)
          this.LevelManager.RecullAt(ssPos);
      }
      base.Update(gameTime);
    }
  }
}
