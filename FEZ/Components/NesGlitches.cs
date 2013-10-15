// Type: FezGame.Components.NesGlitches
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine.Effects;
using FezEngine.Services;
using FezEngine.Structure;
using FezEngine.Structure.Geometry;
using FezEngine.Tools;
using FezGame.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FezGame.Components
{
  internal class NesGlitches : DrawableGameComponent
  {
    private readonly List<SoundEmitter> eGlitches = new List<SoundEmitter>();
    private const int MaxGlitches = 1000;
    private RenderTargetHandle FreezeRth;
    private Mesh GlitchMesh;
    private SoundEffect[] sGlitches;
    private SoundEffect[] sTimestretches;
    private SoundEmitter eTimeStretch;
    private float freezeForFrames;
    private float disappearRF;
    private float resetRF;
    private Random random;
    private ShaderInstancedIndexedPrimitives<VertexPositionTextureInstance, Matrix> Geometry;

    public float FreezeProbability { get; set; }

    public float DisappearProbability { get; set; }

    public int ActiveGlitches { get; set; }

    [ServiceDependency]
    public ISoundManager SoundManager { private get; set; }

    [ServiceDependency]
    public IGameStateManager GameState { private get; set; }

    [ServiceDependency]
    public ITargetRenderingManager TargetRenderingManager { private get; set; }

    [ServiceDependency]
    public IDefaultCameraManager CameraManager { private get; set; }

    [ServiceDependency]
    public IContentManagerProvider CMProvider { private get; set; }

    public NesGlitches(Game game)
      : base(game)
    {
      this.DrawOrder = 1000;
    }

    public override void Initialize()
    {
      base.Initialize();
      this.FreezeRth = this.TargetRenderingManager.TakeTarget();
      this.GlitchMesh = new Mesh()
      {
        Effect = (BaseEffect) new GlitchyPostEffect(),
        AlwaysOnTop = true,
        DepthWrites = false,
        Blending = new BlendingMode?(BlendingMode.Opaque),
        Culling = CullMode.None,
        SamplerState = SamplerState.PointWrap,
        Texture = (Dirtyable<Texture>) ((Texture) this.CMProvider.CurrentLevel.Load<Texture2D>("Other Textures/glitches/glitch_atlas"))
      };
      this.GlitchMesh.AddGroup().Geometry = (IIndexedPrimitiveCollection) (this.Geometry = new ShaderInstancedIndexedPrimitives<VertexPositionTextureInstance, Matrix>(PrimitiveType.TriangleList, 60));
      this.Geometry.Vertices = new VertexPositionTextureInstance[4]
      {
        new VertexPositionTextureInstance(Vector3.Zero, Vector2.Zero),
        new VertexPositionTextureInstance(new Vector3(0.0f, 1f, 0.0f), new Vector2(0.0f, 1f)),
        new VertexPositionTextureInstance(new Vector3(1f, 1f, 0.0f), new Vector2(1f, 1f)),
        new VertexPositionTextureInstance(new Vector3(1f, 0.0f, 0.0f), new Vector2(1f, 0.0f))
      };
      this.Geometry.Indices = new int[6]
      {
        0,
        1,
        2,
        0,
        2,
        3
      };
      this.random = RandomHelper.Random;
      this.Geometry.Instances = new Matrix[1000];
      for (int index = 0; index < 1000; ++index)
      {
        float num1 = (float) this.random.Next(1, 5);
        float num2 = (float) this.random.Next(1, 3);
        bool flag = 0.75 > this.random.NextDouble();
        this.Geometry.Instances[index] = new Matrix((float) RandomHelper.Random.Next(-8, 54), (float) RandomHelper.Random.Next(-8, 30), flag ? num1 : num2, flag ? num2 : num1, this.random.Next(0, 3) == 0 ? 1f : 0.0f, this.random.Next(0, 3) == 0 ? 1f : 0.0f, this.random.Next(0, 3) == 0 ? 1f : 0.0f, (float) this.random.Next(0, 2), this.random.Next(0, 3) == 0 ? 1f : 0.0f, this.random.Next(0, 3) == 0 ? 1f : 0.0f, this.random.Next(0, 3) == 0 ? 1f : 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f);
      }
      this.Geometry.MaximizeBuffers(1000);
      this.sGlitches = Enumerable.ToArray<SoundEffect>(Enumerable.Select<string, SoundEffect>(this.CMProvider.GetAllIn("Sounds/Intro\\Elders\\Glitches"), (Func<string, SoundEffect>) (x => this.CMProvider.CurrentLevel.Load<SoundEffect>(x))));
      this.sTimestretches = Enumerable.ToArray<SoundEffect>(Enumerable.Select<string, SoundEffect>(this.CMProvider.GetAllIn("Sounds/Intro\\Elders\\Timestretches"), (Func<string, SoundEffect>) (x => this.CMProvider.CurrentLevel.Load<SoundEffect>(x))));
    }

    protected override void Dispose(bool disposing)
    {
      base.Dispose(disposing);
      this.TargetRenderingManager.ReturnTarget(this.FreezeRth);
      this.GlitchMesh.Dispose();
      this.GlitchMesh = (Mesh) null;
      if (this.eTimeStretch != null)
        this.eTimeStretch.FadeOutAndDie(0.0f);
      this.Enabled = this.Visible = false;
    }

    public override void Update(GameTime gameTime)
    {
      if (this.GameState.Paused || this.GameState.Loading)
        return;
      if (RandomHelper.Probability(0.05))
        this.resetRF = Easing.EaseIn(this.random.NextDouble(), EasingType.Sine) * 10f;
      this.ActiveGlitches = Math.Min(this.ActiveGlitches, 1000);
      this.Geometry.InstanceCount = this.ActiveGlitches;
      for (int index = 0; index < this.Geometry.InstanceCount; ++index)
      {
        Matrix matrix = this.Geometry.Instances[index];
        if ((double) matrix.M34 != 0.0)
        {
          --matrix.M43;
          if ((double) matrix.M43 <= 0.0)
          {
            matrix.M34 = 0.0f;
            this.EmitFor(matrix.M11, matrix.M13);
          }
          else
          {
            this.Geometry.Instances[index].M43 = matrix.M43;
            continue;
          }
        }
        else if ((double) this.DisappearProbability * (double) this.disappearRF >= this.random.NextDouble())
        {
          matrix.M34 = 1f;
          matrix.M43 = (float) this.random.Next(1, 30);
        }
        if (0.05 * (double) this.resetRF >= this.random.NextDouble())
        {
          matrix.M21 = this.random.Next(0, 3) == 0 ? 1f : 0.0f;
          matrix.M22 = this.random.Next(0, 3) == 0 ? 1f : 0.0f;
          matrix.M23 = this.random.Next(0, 3) == 0 ? 1f : 0.0f;
          matrix.M24 = this.random.Next(0, 2) == 0 ? 1f : 0.0f;
          matrix.M31 = this.random.Next(0, 3) == 0 ? 1f : 0.0f;
          matrix.M32 = this.random.Next(0, 3) == 0 ? 1f : 0.0f;
          matrix.M33 = this.random.Next(0, 3) == 0 ? 1f : 0.0f;
        }
        if (0.075 * (double) this.resetRF >= this.random.NextDouble())
        {
          Vector2 vector2 = new Vector2(matrix.M41, matrix.M42) * 32f;
          vector2 += new Vector2((float) RandomHelper.Random.Next(-2, 3), (float) RandomHelper.Random.Next(-1, 2));
          vector2 = Vector2.Clamp(vector2, Vector2.Zero, new Vector2(32f));
          matrix.M41 = vector2.X / 32f;
          matrix.M42 = vector2.Y / 32f;
        }
        if (0.075 * (double) this.resetRF >= this.random.NextDouble())
        {
          bool flag = (double) matrix.M13 < (double) matrix.M14;
          matrix.M13 += (float) this.random.Next(-1, 2);
          matrix.M14 += (float) this.random.Next(-1, 2);
          matrix.M13 = MathHelper.Clamp(matrix.M13, 1f, flag ? 2f : 4f);
          matrix.M14 = MathHelper.Clamp(matrix.M14, 1f, flag ? 4f : 2f);
          if (RandomHelper.Probability(0.75))
          {
            matrix.M11 += (float) RandomHelper.Random.Next(-1, 2);
            matrix.M12 += (float) RandomHelper.Random.Next(-1, 2);
            matrix.M11 = MathHelper.Clamp(matrix.M11, -8f, 54f);
            matrix.M12 = MathHelper.Clamp(matrix.M12, -8f, 30f);
          }
        }
        if (0.015 * (double) this.resetRF >= this.random.NextDouble())
        {
          matrix.M11 = (float) RandomHelper.Random.Next(-8, 54);
          matrix.M12 = (float) RandomHelper.Random.Next(-8, 30);
          this.EmitFor(matrix.M11, matrix.M13);
        }
        this.Geometry.Instances[index] = matrix;
      }
      for (int index = this.eGlitches.Count - 1; index >= 0; --index)
      {
        if (this.eGlitches[index].Dead)
          this.eGlitches.RemoveAt(index);
      }
    }

    private void EmitFor(float xp, float xs)
    {
      if ((double) this.freezeForFrames > 0.0 || !RandomHelper.Probability(1.0 / Math.Sqrt((double) this.ActiveGlitches)))
        return;
      SoundEmitter soundEmitter = SoundEffectExtensions.Emit(this.sGlitches[this.random.Next(0, this.sGlitches.Length)], (float) (this.random.NextDouble() * 0.5 - 0.25));
      soundEmitter.Pan = MathHelper.Clamp((float) (((double) xp + (double) xs / 2.0) / 27.0 - 1.0), -1f, 1f);
      this.eGlitches.Add(soundEmitter);
    }

    public override void Draw(GameTime gameTime)
    {
      if (this.GameState.Paused || this.GameState.Loading)
        return;
      if (RandomHelper.Probability(0.1))
        this.disappearRF = (float) Math.Pow((double) RandomHelper.Unit(), 2.0) * 10f;
      if (this.TargetRenderingManager.IsHooked(this.FreezeRth.Target))
      {
        this.GlitchMesh.Draw();
        this.TargetRenderingManager.Resolve(this.FreezeRth.Target, false);
        this.GameState.SkipRendering = true;
        foreach (SoundEmitter soundEmitter in this.eGlitches)
        {
          if (!soundEmitter.Dead)
            soundEmitter.FadeOutAndDie(0.0f);
        }
        this.eGlitches.Clear();
        this.SoundManager.Pause();
        this.eTimeStretch = SoundEffectExtensions.Emit(RandomHelper.InList<SoundEffect>(this.sTimestretches), true);
      }
      if ((double) this.freezeForFrames > 0.0)
      {
        this.GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
        GraphicsDeviceExtensions.SetBlendingMode(this.GraphicsDevice, BlendingMode.Opaque);
        this.TargetRenderingManager.DrawFullscreen((Texture) this.FreezeRth.Target);
        if ((double) this.FreezeProbability != 1.0 && (double) --this.freezeForFrames == 0.0)
        {
          this.GameState.SkipRendering = false;
          this.eTimeStretch.FadeOutAndDie(0.0f);
          this.SoundManager.Resume();
          this.eTimeStretch = (SoundEmitter) null;
        }
        GraphicsDeviceExtensions.SetBlendingMode(this.GraphicsDevice, BlendingMode.Alphablending);
      }
      else
      {
        this.GlitchMesh.Draw();
        if (!RandomHelper.Probability((double) this.FreezeProbability))
          return;
        this.TargetRenderingManager.ScheduleHook(this.DrawOrder, this.FreezeRth.Target);
        this.freezeForFrames = (float) RandomHelper.Random.Next(1, 30);
      }
    }
  }
}
