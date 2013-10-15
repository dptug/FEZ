// Type: FezGame.Components.SplitUpCubeHost
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine;
using FezEngine.Components;
using FezEngine.Effects;
using FezEngine.Services;
using FezEngine.Services.Scripting;
using FezEngine.Structure;
using FezEngine.Structure.Geometry;
using FezEngine.Tools;
using FezGame.Services;
using FezGame.Structure;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FezGame.Components
{
  internal class SplitUpCubeHost : DrawableGameComponent
  {
    private readonly List<SplitUpCubeHost.SwooshingCube> TrackedCollects = new List<SplitUpCubeHost.SwooshingCube>();
    private readonly List<TrileInstance> TrackedBits = new List<TrileInstance>();
    private readonly Vector3[] CubeOffsets = new Vector3[8]
    {
      new Vector3(0.25f, -0.25f, 0.25f),
      new Vector3(-0.25f, -0.25f, 0.25f),
      new Vector3(-0.25f, -0.25f, -0.25f),
      new Vector3(0.25f, -0.25f, -0.25f),
      new Vector3(0.25f, 0.25f, 0.25f),
      new Vector3(-0.25f, 0.25f, 0.25f),
      new Vector3(-0.25f, 0.25f, -0.25f),
      new Vector3(0.25f, 0.25f, -0.25f)
    };
    public const int ShineRate = 7;
    private SoundEffect[] CollectSounds;
    private SplitCollectorEffect SplitCollectorEffect;
    private float WireOpacityFactor;
    private bool WirecubeVisible;
    private bool SolidCubesVisible;
    private Mesh WireframeCube;
    private Mesh SolidCubes;
    private SplitUpCubeHost.TrailsRenderer trailsRenderer;
    private float SinceNoTrails;
    private float SinceCollect;
    private bool AssembleScheduled;
    private Mesh ChimeOutline;
    private float UntilNextShine;
    private TrileInstance ShineOn;
    private SoundEffect sBitChime;
    private TimeSpan timeAcc;

    [ServiceDependency]
    public ISoundManager SoundManager { get; set; }

    [ServiceDependency]
    public ITimeManager TimeManager { get; set; }

    [ServiceDependency]
    public IContentManagerProvider CMProvider { get; set; }

    [ServiceDependency]
    public IPlayerManager PlayerManager { get; set; }

    [ServiceDependency]
    public IGameLevelManager LevelManager { get; set; }

    [ServiceDependency]
    public ILevelMaterializer LevelMaterializer { get; set; }

    [ServiceDependency]
    public IGameCameraManager CameraManager { get; set; }

    [ServiceDependency]
    public IGameStateManager GameState { get; set; }

    [ServiceDependency]
    public ILightingPostProcess LightingPostProcess { get; set; }

    [ServiceDependency]
    public IGomezService GomezService { get; set; }

    [ServiceDependency]
    public ISpeechBubbleManager SpeechBubble { get; set; }

    public SplitUpCubeHost(Game game)
      : base(game)
    {
      this.DrawOrder = 75;
    }

    public override void Initialize()
    {
      base.Initialize();
      ServiceHelper.AddComponent((IGameComponent) (this.trailsRenderer = new SplitUpCubeHost.TrailsRenderer(this.Game, this)));
      this.LevelManager.LevelChanged += new Action(this.TryInitialize);
      this.trailsRenderer.Visible = this.Enabled = this.Visible = false;
      this.SoundManager.SongChanged += new Action(this.RefreshSounds);
      this.LightingPostProcess.DrawGeometryLights += new Action(this.DrawLights);
    }

    protected override void LoadContent()
    {
      SplitUpCubeHost splitUpCubeHost1 = this;
      Mesh mesh1 = new Mesh();
      Mesh mesh2 = mesh1;
      DefaultEffect.LitTextured litTextured1 = new DefaultEffect.LitTextured();
      litTextured1.Specular = true;
      litTextured1.Emissive = 0.5f;
      litTextured1.AlphaIsEmissive = true;
      DefaultEffect.LitTextured litTextured2 = litTextured1;
      mesh2.Effect = (BaseEffect) litTextured2;
      mesh1.Blending = new BlendingMode?(BlendingMode.Alphablending);
      Mesh mesh3 = mesh1;
      splitUpCubeHost1.SolidCubes = mesh3;
      SplitUpCubeHost splitUpCubeHost2 = this;
      Mesh mesh4 = new Mesh();
      Mesh mesh5 = mesh4;
      DefaultEffect.VertexColored vertexColored1 = new DefaultEffect.VertexColored();
      vertexColored1.Fullbright = true;
      vertexColored1.AlphaIsEmissive = false;
      DefaultEffect.VertexColored vertexColored2 = vertexColored1;
      mesh5.Effect = (BaseEffect) vertexColored2;
      mesh4.DepthWrites = false;
      Mesh mesh6 = mesh4;
      splitUpCubeHost2.ChimeOutline = mesh6;
      this.ChimeOutline.AddWireframePolygon(Color.Yellow, new Vector3(0.0f, 0.7071068f, 0.0f), new Vector3(0.7071068f, 0.0f, 0.0f), new Vector3(0.0f, -0.7071068f, 0.0f), new Vector3(-0.7071068f, 0.0f, 0.0f), new Vector3(0.0f, 0.7071068f, 0.0f));
      this.ChimeOutline.AddWireframePolygon(new Color(Color.Yellow.ToVector3() * 0.3333333f), new Vector3(0.0f, 0.7071068f, 0.0f), new Vector3(0.7071068f, 0.0f, 0.0f), new Vector3(0.0f, -0.7071068f, 0.0f), new Vector3(-0.7071068f, 0.0f, 0.0f), new Vector3(0.0f, 0.7071068f, 0.0f));
      this.ChimeOutline.AddWireframePolygon(new Color(Color.Yellow.ToVector3() * 0.1111111f), new Vector3(0.0f, 0.7071068f, 0.0f), new Vector3(0.7071068f, 0.0f, 0.0f), new Vector3(0.0f, -0.7071068f, 0.0f), new Vector3(-0.7071068f, 0.0f, 0.0f), new Vector3(0.0f, 0.7071068f, 0.0f));
      this.ChimeOutline.AddWireframePolygon(new Color(Color.Yellow.ToVector3() * 0.03703704f), new Vector3(0.0f, 0.7071068f, 0.0f), new Vector3(0.7071068f, 0.0f, 0.0f), new Vector3(0.0f, -0.7071068f, 0.0f), new Vector3(-0.7071068f, 0.0f, 0.0f), new Vector3(0.0f, 0.7071068f, 0.0f));
      this.sBitChime = this.CMProvider.Global.Load<SoundEffect>("Sounds/Collects/BitChime");
    }

    private void RefreshSounds()
    {
      this.CollectSounds = new SoundEffect[8];
      TrackedSong currentlyPlayingSong = this.SoundManager.CurrentlyPlayingSong;
      ShardNotes[] shardNotesArray;
      if (currentlyPlayingSong != null)
        shardNotesArray = currentlyPlayingSong.Notes;
      else
        shardNotesArray = new ShardNotes[8]
        {
          ShardNotes.C2,
          ShardNotes.D2,
          ShardNotes.E2,
          ShardNotes.F2,
          ShardNotes.G2,
          ShardNotes.A2,
          ShardNotes.B2,
          ShardNotes.C3
        };
      for (int index = 0; index < shardNotesArray.Length; ++index)
        this.CollectSounds[index] = this.CMProvider.CurrentLevel.Load<SoundEffect>("Sounds/Collects/SplitUpCube/" + (object) shardNotesArray[index]);
    }

    private void TryInitialize()
    {
      if (this.WireframeCube == null)
      {
        this.WireframeCube = new Mesh()
        {
          Effect = (BaseEffect) (this.SplitCollectorEffect = new SplitCollectorEffect()),
          Material = {
            Diffuse = Vector3.One,
            Opacity = 1f
          },
          Blending = new BlendingMode?(BlendingMode.Alphablending)
        };
        for (int index = 0; index < 7; ++index)
        {
          this.WireframeCube.AddWireframeBox(Vector3.One, Vector3.Zero, new Color(index == 0 ? 1f : (index == 1 ? 0.0f : 0.5f), index == 2 ? 1f : (index == 3 ? 0.0f : 0.5f), index == 4 ? 1f : (index == 5 ? 0.0f : 0.5f), index == 6 ? 1f : 0.0f), true);
          foreach (Vector3 origin in this.CubeOffsets)
            this.WireframeCube.AddWireframeBox(Vector3.One / 2f, origin, new Color(index == 0 ? 1f : (index == 1 ? 0.0f : 0.5f), index == 2 ? 1f : (index == 3 ? 0.0f : 0.5f), index == 4 ? 1f : (index == 5 ? 0.0f : 0.5f), index == 6 ? 0.625f : 0.375f), true);
        }
        this.WireframeCube.CollapseToBuffer<FezVertexPositionColor>();
      }
      this.SolidCubesVisible = true;
      if (this.TrackedCollects.Count > 0)
      {
        this.GameState.SaveData.CollectedParts += this.TrackedCollects.Count;
        Waiters.Wait(0.5, (Action) (() => Waiters.Wait((Func<bool>) (() =>
        {
          if (this.PlayerManager.CanControl)
            return this.PlayerManager.Grounded;
          else
            return false;
        }), (Action) (() =>
        {
          this.GomezService.OnCollectedSplitUpCube();
          this.GameState.OnHudElementChanged();
          this.GameState.Save();
          this.TryAssembleCube();
        }))));
      }
      foreach (SplitUpCubeHost.SwooshingCube swooshingCube in this.TrackedCollects)
        swooshingCube.Dispose();
      this.TrackedCollects.Clear();
      if (this.LevelManager.TrileSet == null)
      {
        this.trailsRenderer.Visible = this.Enabled = this.Visible = false;
      }
      else
      {
        Trile goldenCubeTrile = Enumerable.FirstOrDefault<Trile>(this.LevelManager.ActorTriles(ActorType.GoldenCube));
        IEnumerable<TrileInstance> source = Enumerable.Union<TrileInstance>((IEnumerable<TrileInstance>) this.LevelManager.Triles.Values, Enumerable.SelectMany<KeyValuePair<TrileEmplacement, TrileInstance>, TrileInstance>((IEnumerable<KeyValuePair<TrileEmplacement, TrileInstance>>) this.LevelManager.Triles, (Func<KeyValuePair<TrileEmplacement, TrileInstance>, IEnumerable<TrileInstance>>) (x => (IEnumerable<TrileInstance>) x.Value.OverlappedTriles ?? Enumerable.Empty<TrileInstance>())));
        SplitUpCubeHost.TrailsRenderer trailsRenderer = this.trailsRenderer;
        SplitUpCubeHost splitUpCubeHost = this;
        bool flag1;
        this.Visible = flag1 = goldenCubeTrile != null && (Enumerable.Count<TrileInstance>(source, (Func<TrileInstance, bool>) (x => x.TrileId == goldenCubeTrile.Id)) != 0 || this.AssembleScheduled || this.GameState.SaveData.CollectedParts == 8);
        int num1;
        bool flag2 = (num1 = flag1 ? 1 : 0) != 0;
        splitUpCubeHost.Enabled = num1 != 0;
        int num2 = flag2 ? 1 : 0;
        trailsRenderer.Visible = num2 != 0;
        if (!this.Enabled)
          return;
        this.RefreshSounds();
        this.TrackedBits.Clear();
        this.TrackedBits.AddRange(Enumerable.Where<TrileInstance>(source, (Func<TrileInstance, bool>) (x => x.TrileId == goldenCubeTrile.Id)));
        this.SolidCubes.ClearGroups();
        ShaderInstancedIndexedPrimitives<VertexPositionNormalTextureInstance, Vector4> geometry = goldenCubeTrile.Geometry;
        this.SolidCubes.Position = Vector3.Zero;
        this.SolidCubes.Rotation = Quaternion.Identity;
        foreach (Vector3 vector3 in this.CubeOffsets)
        {
          Group group = this.SolidCubes.AddGroup();
          group.Geometry = (IIndexedPrimitiveCollection) new IndexedUserPrimitives<VertexPositionNormalTextureInstance>(Enumerable.ToArray<VertexPositionNormalTextureInstance>((IEnumerable<VertexPositionNormalTextureInstance>) geometry.Vertices), geometry.Indices, geometry.PrimitiveType);
          group.Position = vector3;
          group.BakeTransform<VertexPositionNormalTextureInstance>();
        }
        this.SolidCubes.Texture = this.LevelMaterializer.TrilesMesh.Texture;
        this.SolidCubes.Rotation = this.WireframeCube.Rotation = Quaternion.CreateFromAxisAngle(Vector3.Right, (float) Math.Asin(Math.Sqrt(2.0) / Math.Sqrt(3.0))) * Quaternion.CreateFromAxisAngle(Vector3.Up, 0.7853982f);
        this.WireOpacityFactor = 1f;
        this.SinceNoTrails = 3f;
        this.ShineOn = (TrileInstance) null;
        this.UntilNextShine = 7f;
        if (this.LevelManager.WaterType == LiquidType.Sewer || this.LevelManager.WaterType == LiquidType.Lava || this.LevelManager.BlinkingAlpha)
        {
          if (this.SolidCubes.Effect is DefaultEffect.LitTextured)
          {
            Mesh mesh = this.SolidCubes;
            DefaultEffect.Textured textured1 = new DefaultEffect.Textured();
            textured1.AlphaIsEmissive = true;
            textured1.IgnoreCache = true;
            DefaultEffect.Textured textured2 = textured1;
            mesh.Effect = (BaseEffect) textured2;
          }
        }
        else if (this.SolidCubes.Effect is DefaultEffect.Textured)
        {
          Mesh mesh = this.SolidCubes;
          DefaultEffect.LitTextured litTextured1 = new DefaultEffect.LitTextured();
          litTextured1.Specular = true;
          litTextured1.Emissive = 0.5f;
          litTextured1.AlphaIsEmissive = true;
          litTextured1.IgnoreCache = true;
          DefaultEffect.LitTextured litTextured2 = litTextured1;
          mesh.Effect = (BaseEffect) litTextured2;
        }
        this.TryAssembleCube();
      }
    }

    public override void Update(GameTime gameTime)
    {
      if (this.GameState.Loading || this.GameState.TimePaused || !FezMath.IsOrthographic(this.CameraManager.Viewpoint))
        return;
      if (this.SolidCubesVisible || this.WirecubeVisible)
      {
        this.SolidCubes.Position = this.WireframeCube.Position = this.PlayerManager.Position + Vector3.UnitY * (float) (Math.Sin(this.timeAcc.TotalSeconds * 3.14159274101257) * 0.100000001490116 + 2.0);
        Mesh mesh1 = this.SolidCubes;
        Mesh mesh2 = this.WireframeCube;
        Vector3 up = Vector3.Up;
        double totalSeconds = gameTime.ElapsedGameTime.TotalSeconds;
        Quaternion quaternion1;
        Quaternion quaternion2 = quaternion1 = Quaternion.CreateFromAxisAngle(up, (float) totalSeconds) * this.WireframeCube.Rotation;
        mesh2.Rotation = quaternion1;
        Quaternion quaternion3 = quaternion2;
        mesh1.Rotation = quaternion3;
        this.SolidCubes.Position += this.PlayerManager.SplitUpCubeCollectorOffset;
        this.WireframeCube.Position += this.PlayerManager.SplitUpCubeCollectorOffset;
        this.timeAcc += gameTime.ElapsedGameTime;
      }
      if (this.AssembleScheduled)
        return;
      this.ShineOnYouCrazyDiamonds((float) gameTime.ElapsedGameTime.TotalSeconds);
      if (this.GameState.SaveData.CollectedParts + this.TrackedCollects.Count != 8 && this.PlayerManager.Action != ActionType.GateWarp && (this.PlayerManager.Action != ActionType.LesserWarp && !ActionTypeExtensions.IsSwimming(this.PlayerManager.Action)))
      {
        TrileInstance collect = this.PlayerManager.AxisCollision[VerticalDirection.Up].Surface ?? this.PlayerManager.AxisCollision[VerticalDirection.Down].Surface;
        if (collect != null && collect.Trile.ActorSettings.Type == ActorType.GoldenCube && !Enumerable.Any<SplitUpCubeHost.SwooshingCube>((IEnumerable<SplitUpCubeHost.SwooshingCube>) this.TrackedCollects, (Func<SplitUpCubeHost.SwooshingCube, bool>) (x => x.Instance == collect)) && !Enumerable.Any<TrileInstance>((IEnumerable<TrileInstance>) this.LevelManager.Triles.Values, (Func<TrileInstance, bool>) (x =>
        {
          if (x.Overlaps && x.OverlappedTriles.Contains(collect))
            return x.Position == collect.Position;
          else
            return false;
        })))
        {
          int index = this.GameState.SaveData.CollectedParts + this.TrackedCollects.Count;
          SoundEffectExtensions.Emit(this.CollectSounds[index]);
          this.TrackedCollects.Add(new SplitUpCubeHost.SwooshingCube(collect, this.SolidCubes, this.CubeOffsets[index], this.SolidCubes.Rotation));
          this.GameState.SaveData.ThisLevel.DestroyedTriles.Add(collect.OriginalEmplacement);
          ++this.GameState.SaveData.ThisLevel.FilledConditions.SplitUpCount;
          this.LevelManager.ClearTrile(collect);
          this.TrackedBits.Remove(collect);
          if ((double) this.SinceNoTrails == 3.0)
            this.SinceCollect = 0.0f;
        }
      }
      for (int index = this.TrackedCollects.Count - 1; index >= 0; --index)
      {
        SplitUpCubeHost.SwooshingCube swooshingCube = this.TrackedCollects[index];
        swooshingCube.Update(gameTime);
        if (swooshingCube.Spline.Reached)
        {
          ++this.GameState.SaveData.CollectedParts;
          this.GomezService.OnCollectedSplitUpCube();
          this.TrackedCollects.RemoveAt(index);
          this.GameState.OnHudElementChanged();
          this.GameState.Save();
          this.TryAssembleCube();
        }
        this.SinceNoTrails = 0.0f;
      }
      this.SinceNoTrails = Math.Min(3f, this.SinceNoTrails + (float) gameTime.ElapsedGameTime.TotalSeconds);
      this.SinceCollect = Math.Min(1f, this.SinceCollect + (float) gameTime.ElapsedGameTime.TotalSeconds);
      for (int index = 0; index < 8; ++index)
        this.SolidCubes.Groups[index].Enabled = this.GameState.SaveData.CollectedParts > index;
      this.WirecubeVisible = (double) this.SinceNoTrails < 3.0;
      this.WireOpacityFactor = (1f - FezMath.Saturate((float) (((double) this.SinceNoTrails - 1.0) / 1.0))) * this.SinceCollect;
      if (this.GameState.SaveData.CollectedParts + this.TrackedCollects.Count == 0)
        this.WireOpacityFactor = 0.0f;
      this.SolidCubes.Material.Opacity = this.WireOpacityFactor;
      this.WirecubeVisible = (double) this.WireOpacityFactor != 0.0;
    }

    private void ShineOnYouCrazyDiamonds(float elapsedTime)
    {
      this.UntilNextShine -= elapsedTime;
      if ((double) this.UntilNextShine <= 0.0 && this.TrackedBits.Count > 0 && (this.PlayerManager.CanControl && this.CameraManager.ViewTransitionReached))
      {
        this.UntilNextShine = 7f;
        this.ChimeOutline.Scale = new Vector3(0.1f);
        this.ChimeOutline.Groups[0].Scale = Vector3.One;
        this.ChimeOutline.Groups[1].Scale = Vector3.One;
        this.ChimeOutline.Groups[2].Scale = Vector3.One;
        this.ChimeOutline.Groups[3].Scale = Vector3.One;
        this.ShineOn = RandomHelper.InList<TrileInstance>(this.TrackedBits);
        SoundEffectExtensions.EmitAt(this.sBitChime, this.ShineOn.Center);
      }
      if (this.ShineOn == null)
        return;
      this.ChimeOutline.Position = this.ShineOn.Center;
      this.ChimeOutline.Rotation = this.CameraManager.Rotation;
      this.ChimeOutline.Scale = new Vector3((float) ((double) Easing.EaseInOut((double) FezMath.Saturate(7f - this.UntilNextShine), EasingType.Quadratic) * 10.0 + (double) Easing.EaseIn(7.0 - (double) this.UntilNextShine, EasingType.Quadratic) * 7.0)) * 0.75f;
      this.ChimeOutline.Groups[0].Scale /= 1.002f;
      this.ChimeOutline.Groups[1].Scale /= 1.006f;
      this.ChimeOutline.Groups[2].Scale /= 1.012f;
      this.ChimeOutline.Groups[3].Scale /= 1.018f;
      this.ChimeOutline.Material.Diffuse = new Vector3((float) ((double) Easing.EaseIn((double) FezMath.Saturate((float) (1.0 - (double) this.ChimeOutline.Scale.X / 40.0)), EasingType.Quadratic) * (1.0 - (double) this.TimeManager.NightContribution * 0.649999976158142) * (1.0 - (double) this.TimeManager.DawnContribution * 0.699999988079071) * (1.0 - (double) this.TimeManager.DuskContribution * 0.699999988079071)));
      this.ChimeOutline.Blending = new BlendingMode?(BlendingMode.Additive);
      if ((double) this.ChimeOutline.Scale.X <= 40.0)
        return;
      this.ShineOn = (TrileInstance) null;
    }

    private void TryAssembleCube()
    {
      if (this.AssembleScheduled || this.GameState.SaveData.CollectedParts != 8)
        return;
      this.AssembleScheduled = true;
      Waiters.Wait((Func<bool>) (() =>
      {
        if (!this.GameState.Loading && ActionTypeExtensions.AllowsLookingDirectionChange(this.PlayerManager.Action) && (this.SpeechBubble.Hidden && !this.GameState.ForceTimePaused) && (this.PlayerManager.CanControl && !ActionTypeExtensions.DisallowsRespawn(this.PlayerManager.Action) && (this.CameraManager.ViewTransitionReached && !this.PlayerManager.InDoorTransition)))
          return this.PlayerManager.CarriedInstance == null;
        else
          return false;
      }), (Action) (() => Waiters.Wait(0.0, (Action) (() =>
      {
        Vector3 local_0 = FezMath.DepthMask(this.CameraManager.Viewpoint);
        Vector3 local_1 = FezMath.ForwardVector(this.CameraManager.Viewpoint);
        TrileInstance local_3 = new TrileInstance((this.PlayerManager.Position + Vector3.UnitY * (float) (Math.Sin(this.timeAcc.TotalSeconds * 3.14159274101257) * 0.100000001490116 + 2.0) - FezMath.HalfVector) * (Vector3.One - local_0) - local_1 * (this.LevelManager.Size / 2f - local_0 * 2f) + local_0 * this.LevelManager.Size / 2f, Enumerable.Last<Trile>((IEnumerable<Trile>) this.LevelManager.TrileSet.Triles.Values, (Func<Trile, bool>) (x => x.ActorSettings.Type == ActorType.CubeShard)).Id);
        this.LevelManager.RestoreTrile(local_3);
        this.LevelMaterializer.CullInstanceIn(local_3);
        this.PlayerManager.ForcedTreasure = local_3;
        this.PlayerManager.Action = ActionType.FindingTreasure;
        this.AssembleScheduled = false;
      }))));
    }

    private void ClearDepth(Mesh mesh)
    {
      bool depthWrites = mesh.DepthWrites;
      ColorWriteChannels colorWriteChannels = GraphicsDeviceExtensions.GetBlendCombiner(this.GraphicsDevice).ColorWriteChannels;
      mesh.DepthWrites = true;
      GraphicsDeviceExtensions.SetColorWriteChannels(this.GraphicsDevice, ColorWriteChannels.None);
      mesh.AlwaysOnTop = true;
      mesh.Position += this.CameraManager.InverseView.Forward * 2f;
      mesh.Draw();
      mesh.AlwaysOnTop = false;
      mesh.Position -= this.CameraManager.InverseView.Forward * 2f;
      GraphicsDeviceExtensions.SetColorWriteChannels(this.GraphicsDevice, colorWriteChannels);
      mesh.DepthWrites = depthWrites;
    }

    private void DrawLights()
    {
      if (this.GameState.Loading || !this.Visible || this.PlayerManager.Action == ActionType.FindingTreasure)
        return;
      if (this.ShineOn != null)
        this.ChimeOutline.Draw();
      if ((double) this.SolidCubes.Material.Opacity < 0.25)
        return;
      GraphicsDevice graphicsDevice = this.GraphicsDevice;
      foreach (SplitUpCubeHost.SwooshingCube swooshingCube in this.TrackedCollects)
        this.ClearDepth(swooshingCube.Cube);
      if (this.SolidCubesVisible)
      {
        this.ClearDepth(this.SolidCubes);
        GraphicsDeviceExtensions.PrepareStencilWrite(graphicsDevice, new StencilMask?(StencilMask.Level));
        (this.SolidCubes.Effect as DefaultEffect).Pass = LightingEffectPass.Pre;
        this.SolidCubes.Draw();
        (this.SolidCubes.Effect as DefaultEffect).Pass = LightingEffectPass.Main;
      }
      GraphicsDeviceExtensions.PrepareStencilWrite(graphicsDevice, new StencilMask?(StencilMask.Level));
      foreach (SplitUpCubeHost.SwooshingCube swooshingCube in this.TrackedCollects)
      {
        (swooshingCube.Cube.Effect as DefaultEffect).Pass = LightingEffectPass.Pre;
        swooshingCube.Cube.Draw();
        (swooshingCube.Cube.Effect as DefaultEffect).Pass = LightingEffectPass.Main;
      }
      GraphicsDeviceExtensions.PrepareStencilWrite(graphicsDevice, new StencilMask?(StencilMask.None));
    }

    public override void Draw(GameTime gameTime)
    {
      if (this.GameState.Loading || this.PlayerManager.Action == ActionType.FindingTreasure)
        return;
      GraphicsDevice graphicsDevice = this.GraphicsDevice;
      if (this.WirecubeVisible)
        this.ClearDepth(this.WireframeCube);
      foreach (SplitUpCubeHost.SwooshingCube swooshingCube in this.TrackedCollects)
        this.ClearDepth(swooshingCube.Cube);
      if (this.SolidCubesVisible)
      {
        this.ClearDepth(this.SolidCubes);
        GraphicsDeviceExtensions.PrepareStencilWrite(graphicsDevice, new StencilMask?(StencilMask.Level));
        this.SolidCubes.Draw();
      }
      GraphicsDeviceExtensions.PrepareStencilWrite(graphicsDevice, new StencilMask?(StencilMask.Level));
      foreach (SplitUpCubeHost.SwooshingCube swooshingCube in this.TrackedCollects)
        swooshingCube.Cube.Draw();
      GraphicsDeviceExtensions.PrepareStencilWrite(graphicsDevice, new StencilMask?(StencilMask.None));
      if (this.ShineOn != null)
        this.ChimeOutline.Draw();
      if (!this.SolidCubes.Effect.IgnoreCache)
        return;
      this.SolidCubes.Effect.IgnoreCache = false;
    }

    private class TrailsRenderer : DrawableGameComponent
    {
      private readonly SplitUpCubeHost Host;

      public TrailsRenderer(Game game, SplitUpCubeHost host)
        : base(game)
      {
        this.Host = host;
        this.DrawOrder = 101;
      }

      public override void Draw(GameTime gameTime)
      {
        if (this.Host.GameState.Loading || this.Host.PlayerManager.Action == ActionType.FindingTreasure)
          return;
        GraphicsDevice graphicsDevice = this.GraphicsDevice;
        if (this.Host.WirecubeVisible)
        {
          GraphicsDeviceExtensions.PrepareStencilWrite(graphicsDevice, new StencilMask?(StencilMask.Wirecube));
          this.Host.WireframeCube.DepthWrites = false;
          switch (this.Host.LevelManager.WaterType)
          {
            case LiquidType.Lava:
              this.Host.WireframeCube.Material.Diffuse = new Vector3((float) byte.MaxValue, 0.0f, 0.0f) / (float) byte.MaxValue;
              break;
            case LiquidType.Sewer:
              this.Host.WireframeCube.Material.Diffuse = new Vector3(215f, 232f, 148f) / (float) byte.MaxValue;
              break;
            default:
              this.Host.WireframeCube.Material.Diffuse = Vector3.One;
              break;
          }
          this.Host.SplitCollectorEffect.Offset = (float) (1.0 / 16.0 / (double) this.Host.CameraManager.PixelsPerTrixel * (double) Math.Abs((float) Math.Sin(this.Host.timeAcc.TotalSeconds)) * 8.0);
          this.Host.SplitCollectorEffect.VaryingOpacity = (float) (0.0500000007450581 + (double) Math.Abs((float) Math.Cos(this.Host.timeAcc.TotalSeconds * 3.0)) * 0.200000002980232);
          this.Host.WireframeCube.Material.Opacity = this.Host.WireOpacityFactor;
          this.Host.WireframeCube.Draw();
        }
        foreach (SplitUpCubeHost.SwooshingCube swooshingCube in this.Host.TrackedCollects)
        {
          GraphicsDeviceExtensions.PrepareStencilReadWrite(graphicsDevice, CompareFunction.NotEqual, StencilMask.Trails);
          swooshingCube.Trail.Draw();
          GraphicsDeviceExtensions.SetColorWriteChannels(graphicsDevice, ColorWriteChannels.None);
          GraphicsDeviceExtensions.GetDssCombiner(graphicsDevice).DepthBufferWriteEnable = false;
          GraphicsDeviceExtensions.PrepareStencilWrite(graphicsDevice, new StencilMask?(StencilMask.None));
          swooshingCube.Trail.Draw();
          GraphicsDeviceExtensions.SetColorWriteChannels(graphicsDevice, ColorWriteChannels.All);
          GraphicsDeviceExtensions.GetDssCombiner(graphicsDevice).DepthBufferWriteEnable = true;
        }
        GraphicsDeviceExtensions.PrepareStencilWrite(graphicsDevice, new StencilMask?(StencilMask.None));
      }
    }

    private class SwooshingCube
    {
      private readonly Vector3 RedTrail = new Vector3(254f, 1f, 0.0f) / (float) byte.MaxValue;
      private readonly Vector3 StandardTrail = new Vector3((float) byte.MaxValue, 230f, 96f) / (float) byte.MaxValue;
      private readonly Vector3 SewerTrail = new Vector3(215f, 232f, 148f) / (float) byte.MaxValue;
      private readonly Vector3 CMYTrail = new Vector3(1f, 1f, 0.0f);
      private const float TrailRadius = 0.5f;
      public readonly Mesh Trail;
      public readonly Mesh Cube;
      public readonly TrileInstance Instance;
      public readonly Vector3SplineInterpolation Spline;
      private readonly IndexedUserPrimitives<FezVertexPositionColor> TrailGeometry;
      private readonly Mesh DestinationMesh;
      private readonly Vector3 sideDirection;
      private readonly Vector3 color;
      private readonly Vector3 positionOffset;
      private Vector3 lastPoint;
      private float lastStep;
      private Quaternion rotation;
      private FezVertexPositionColor[] TrailVertices;
      private int[] TrailIndices;

      [ServiceDependency]
      public IGameCameraManager CameraManager { private get; set; }

      [ServiceDependency]
      public ILevelManager LevelManager { private get; set; }

      [ServiceDependency]
      public ILevelMaterializer LevelMaterializer { private get; set; }

      public SwooshingCube(TrileInstance instance, Mesh destinationMesh, Vector3 Offset, Quaternion Rotation)
      {
        this.CameraManager = ServiceHelper.Get<IGameCameraManager>();
        this.LevelManager = ServiceHelper.Get<ILevelManager>();
        this.LevelMaterializer = ServiceHelper.Get<ILevelMaterializer>();
        this.rotation = Rotation;
        this.positionOffset = Offset;
        this.color = this.StandardTrail;
        switch (this.LevelManager.WaterType)
        {
          case LiquidType.Lava:
            this.color = this.RedTrail;
            break;
          case LiquidType.Sewer:
            this.color = this.SewerTrail;
            break;
        }
        if (this.LevelManager.BlinkingAlpha)
          this.color = this.CMYTrail;
        this.Trail = new Mesh()
        {
          Effect = (BaseEffect) new DefaultEffect.VertexColored(),
          Culling = CullMode.None,
          Blending = new BlendingMode?(BlendingMode.Additive),
          AlwaysOnTop = true
        };
        this.Cube = new Mesh()
        {
          Texture = this.LevelMaterializer.TrilesMesh.Texture
        };
        if (this.LevelManager.WaterType == LiquidType.Sewer || this.LevelManager.WaterType == LiquidType.Lava || this.LevelManager.BlinkingAlpha)
        {
          Mesh mesh = this.Cube;
          DefaultEffect.Textured textured1 = new DefaultEffect.Textured();
          textured1.AlphaIsEmissive = true;
          DefaultEffect.Textured textured2 = textured1;
          mesh.Effect = (BaseEffect) textured2;
        }
        else
        {
          Mesh mesh = this.Cube;
          DefaultEffect.LitTextured litTextured1 = new DefaultEffect.LitTextured();
          litTextured1.Specular = true;
          litTextured1.Emissive = 0.5f;
          litTextured1.AlphaIsEmissive = true;
          DefaultEffect.LitTextured litTextured2 = litTextured1;
          mesh.Effect = (BaseEffect) litTextured2;
        }
        ShaderInstancedIndexedPrimitives<VertexPositionNormalTextureInstance, Vector4> geometry = instance.Trile.Geometry;
        this.Cube.AddGroup().Geometry = (IIndexedPrimitiveCollection) new IndexedUserPrimitives<VertexPositionNormalTextureInstance>(geometry.Vertices, geometry.Indices, geometry.PrimitiveType);
        this.Trail.AddGroup().Geometry = (IIndexedPrimitiveCollection) (this.TrailGeometry = new IndexedUserPrimitives<FezVertexPositionColor>(this.TrailVertices = new FezVertexPositionColor[0], this.TrailIndices = new int[0], PrimitiveType.TriangleList));
        this.Instance = instance;
        this.lastPoint = instance.Center;
        this.DestinationMesh = destinationMesh;
        this.sideDirection = (RandomHelper.Probability(0.5) ? -1f : 1f) * FezMath.RightVector(this.CameraManager.Viewpoint);
        this.Spline = new Vector3SplineInterpolation(TimeSpan.FromSeconds(3.0), new Vector3[10]);
        this.Spline.Start();
        this.AddSegment();
      }

      public void Dispose()
      {
        this.Cube.Dispose();
        this.Trail.Dispose();
      }

      private void AddSegment()
      {
        bool flag = this.TrailVertices.Length == 0;
        Array.Resize<FezVertexPositionColor>(ref this.TrailVertices, this.TrailVertices.Length + (flag ? 12 : 6));
        Array.Resize<int>(ref this.TrailIndices, this.TrailIndices.Length + 36);
        this.TrailGeometry.Vertices = this.TrailVertices;
        this.TrailGeometry.Indices = this.TrailIndices;
        int num = this.TrailVertices.Length - 12;
        for (int index1 = 0; index1 < 6; ++index1)
        {
          int index2 = index1 * 6 + this.TrailIndices.Length - 36;
          this.TrailIndices[index2] = index1 + num;
          this.TrailIndices[index2 + 1] = (index1 + 6) % 12 + num;
          this.TrailIndices[index2 + 2] = (index1 + 1) % 12 + num;
          this.TrailIndices[index2 + 3] = (index1 + 1) % 12 + num;
          this.TrailIndices[index2 + 4] = (index1 + 6) % 12 + num;
          this.TrailIndices[index2 + 5] = (index1 + 7) % 12 + num;
        }
        if (!flag)
          return;
        for (int index = 0; index < 6; ++index)
        {
          this.TrailVertices[index].Position = this.lastPoint;
          this.TrailVertices[index].Color = Color.Black;
        }
      }

      private void AlignLastSegment()
      {
        Vector3 current = this.Spline.Current;
        Vector3 vector3_1 = Vector3.Normalize(current - this.lastPoint);
        Vector3 vector2 = Vector3.Normalize(Vector3.Cross(FezMath.Slerp(Vector3.Up, Vector3.Forward, Math.Abs(FezMath.Dot(vector3_1, Vector3.Up))), vector3_1));
        Vector3 vector3_2 = Vector3.Normalize(Vector3.Cross(vector3_1, vector2));
        Quaternion quat = Quaternion.Inverse(Quaternion.CreateFromRotationMatrix(new Matrix(vector2.X, vector3_2.X, vector3_1.X, 0.0f, vector2.Y, vector3_2.Y, vector3_1.Y, 0.0f, vector2.Z, vector3_2.Z, vector3_1.Z, 0.0f, 0.0f, 0.0f, 0.0f, 1f)));
        int num1 = this.TrailVertices.Length - 6;
        for (int index = 0; index < 6; ++index)
        {
          float num2 = (float) ((double) index / 6.0 * 6.28318548202515);
          this.TrailVertices[num1 + index].Position = Vector3.Transform(new Vector3((float) (Math.Sin((double) num2) * 0.5 / 2.0), (float) (Math.Cos((double) num2) * 0.5 / 2.0), 0.0f), quat) + current;
        }
      }

      private void ColorSegments()
      {
        int num1 = this.TrailVertices.Length / 6;
        for (int index1 = 0; index1 < num1; ++index1)
        {
          for (int index2 = 0; index2 < 6; ++index2)
          {
            float num2 = Easing.EaseIn((double) Math.Max((float) (index1 - (num1 - 9)) / 10f, 0.0f), EasingType.Sine) * (float) Math.Pow(1.0 - (double) this.Spline.TotalStep, 0.5);
            this.TrailVertices[index1 * 6 + index2].Color = new Color(new Vector3(num2) * this.color);
          }
        }
      }

      public void Update(GameTime gameTime)
      {
        for (int index = 0; index < this.Spline.Points.Length; ++index)
        {
          float amount = Easing.EaseOut((double) index / (double) (this.Spline.Points.Length - 1), EasingType.Sine);
          this.Spline.Points[index] = Vector3.Lerp(this.Instance.Center, this.DestinationMesh.Position, amount);
          Vector3 vector3 = Vector3.Zero + this.sideDirection * 3.5f * (0.7f - (float) Math.Sin((double) amount * 6.28318548202515 + 0.785398185253143)) + Vector3.Up * 2f * (0.7f - (float) Math.Cos((double) amount * 6.28318548202515 + 0.785398185253143));
          if (index != 0 && index != this.Spline.Points.Length - 1)
            this.Spline.Points[index] += vector3;
        }
        this.Spline.Update(gameTime);
        if ((double) this.Spline.TotalStep - (double) this.lastStep > 0.025)
        {
          this.lastPoint = this.Spline.Current;
          this.lastStep = this.Spline.TotalStep;
          this.AddSegment();
        }
        this.AlignLastSegment();
        this.ColorSegments();
        this.rotation = Quaternion.CreateFromAxisAngle(Vector3.Up, (float) gameTime.ElapsedGameTime.TotalSeconds) * this.rotation;
        this.Cube.Position = this.Spline.Current + Vector3.Transform(this.positionOffset, this.rotation) * this.Spline.TotalStep;
        this.Cube.Rotation = Quaternion.Slerp(Quaternion.Identity, this.rotation, this.Spline.TotalStep);
      }
    }
  }
}
