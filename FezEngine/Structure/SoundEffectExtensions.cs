// Type: FezEngine.Structure.SoundEffectExtensions
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using FezEngine.Services;
using FezEngine.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace FezEngine.Structure
{
  public static class SoundEffectExtensions
  {
    public static ISoundManager SoundManager;

    public static SoundEmitter Emit(this SoundEffect soundEffect)
    {
      if (SoundEffectExtensions.SoundManager == null)
        SoundEffectExtensions.SoundManager = ServiceHelper.Get<ISoundManager>();
      return SoundEffectExtensions.SoundManager.AddEmitter(new SoundEmitter(soundEffect, false, 0.0f, 1f, false, new Vector3?()));
    }

    public static SoundEmitter Emit(this SoundEffect soundEffect, bool loop)
    {
      if (SoundEffectExtensions.SoundManager == null)
        SoundEffectExtensions.SoundManager = ServiceHelper.Get<ISoundManager>();
      return SoundEffectExtensions.SoundManager.AddEmitter(new SoundEmitter(soundEffect, loop, 0.0f, 1f, false, new Vector3?()));
    }

    public static SoundEmitter Emit(this SoundEffect soundEffect, bool loop, bool paused)
    {
      if (SoundEffectExtensions.SoundManager == null)
        SoundEffectExtensions.SoundManager = ServiceHelper.Get<ISoundManager>();
      return SoundEffectExtensions.SoundManager.AddEmitter(new SoundEmitter(soundEffect, loop, 0.0f, 1f, paused, new Vector3?()));
    }

    public static SoundEmitter Emit(this SoundEffect soundEffect, float pitch)
    {
      if (SoundEffectExtensions.SoundManager == null)
        SoundEffectExtensions.SoundManager = ServiceHelper.Get<ISoundManager>();
      return SoundEffectExtensions.SoundManager.AddEmitter(new SoundEmitter(soundEffect, false, pitch, 1f, false, new Vector3?()));
    }

    public static SoundEmitter Emit(this SoundEffect soundEffect, float pitch, bool paused)
    {
      if (SoundEffectExtensions.SoundManager == null)
        SoundEffectExtensions.SoundManager = ServiceHelper.Get<ISoundManager>();
      return SoundEffectExtensions.SoundManager.AddEmitter(new SoundEmitter(soundEffect, false, pitch, 1f, paused, new Vector3?()));
    }

    public static SoundEmitter Emit(this SoundEffect soundEffect, bool loop, float pitch)
    {
      if (SoundEffectExtensions.SoundManager == null)
        SoundEffectExtensions.SoundManager = ServiceHelper.Get<ISoundManager>();
      return SoundEffectExtensions.SoundManager.AddEmitter(new SoundEmitter(soundEffect, loop, pitch, 1f, false, new Vector3?()));
    }

    public static SoundEmitter Emit(this SoundEffect soundEffect, bool loop, float pitch, bool paused)
    {
      if (SoundEffectExtensions.SoundManager == null)
        SoundEffectExtensions.SoundManager = ServiceHelper.Get<ISoundManager>();
      return SoundEffectExtensions.SoundManager.AddEmitter(new SoundEmitter(soundEffect, loop, pitch, 1f, paused, new Vector3?()));
    }

    public static SoundEmitter Emit(this SoundEffect soundEffect, float pitch, float volume)
    {
      if (SoundEffectExtensions.SoundManager == null)
        SoundEffectExtensions.SoundManager = ServiceHelper.Get<ISoundManager>();
      return SoundEffectExtensions.SoundManager.AddEmitter(new SoundEmitter(soundEffect, false, pitch, volume, false, new Vector3?()));
    }

    public static SoundEmitter Emit(this SoundEffect soundEffect, float pitch, float volume, bool paused)
    {
      if (SoundEffectExtensions.SoundManager == null)
        SoundEffectExtensions.SoundManager = ServiceHelper.Get<ISoundManager>();
      return SoundEffectExtensions.SoundManager.AddEmitter(new SoundEmitter(soundEffect, false, pitch, volume, paused, new Vector3?()));
    }

    public static SoundEmitter Emit(this SoundEffect soundEffect, bool loop, float pitch, float volume)
    {
      if (SoundEffectExtensions.SoundManager == null)
        SoundEffectExtensions.SoundManager = ServiceHelper.Get<ISoundManager>();
      return SoundEffectExtensions.SoundManager.AddEmitter(new SoundEmitter(soundEffect, loop, pitch, volume, false, new Vector3?()));
    }

    public static SoundEmitter Emit(this SoundEffect soundEffect, bool loop, float pitch, float volume, bool paused)
    {
      if (SoundEffectExtensions.SoundManager == null)
        SoundEffectExtensions.SoundManager = ServiceHelper.Get<ISoundManager>();
      return SoundEffectExtensions.SoundManager.AddEmitter(new SoundEmitter(soundEffect, loop, pitch, volume, paused, new Vector3?()));
    }

    public static SoundEmitter EmitAt(this SoundEffect soundEffect, Vector3 position)
    {
      if (SoundEffectExtensions.SoundManager == null)
        SoundEffectExtensions.SoundManager = ServiceHelper.Get<ISoundManager>();
      return SoundEffectExtensions.SoundManager.AddEmitter(new SoundEmitter(soundEffect, false, 0.0f, 1f, false, new Vector3?(position)));
    }

    public static SoundEmitter EmitAt(this SoundEffect soundEffect, Vector3 position, bool loop)
    {
      if (SoundEffectExtensions.SoundManager == null)
        SoundEffectExtensions.SoundManager = ServiceHelper.Get<ISoundManager>();
      return SoundEffectExtensions.SoundManager.AddEmitter(new SoundEmitter(soundEffect, loop, 0.0f, 1f, false, new Vector3?(position)));
    }

    public static SoundEmitter EmitAt(this SoundEffect soundEffect, Vector3 position, bool loop, bool paused)
    {
      if (SoundEffectExtensions.SoundManager == null)
        SoundEffectExtensions.SoundManager = ServiceHelper.Get<ISoundManager>();
      return SoundEffectExtensions.SoundManager.AddEmitter(new SoundEmitter(soundEffect, loop, 0.0f, 1f, paused, new Vector3?(position)));
    }

    public static SoundEmitter EmitAt(this SoundEffect soundEffect, Vector3 position, float pitch)
    {
      if (SoundEffectExtensions.SoundManager == null)
        SoundEffectExtensions.SoundManager = ServiceHelper.Get<ISoundManager>();
      return SoundEffectExtensions.SoundManager.AddEmitter(new SoundEmitter(soundEffect, false, pitch, 1f, false, new Vector3?(position)));
    }

    public static SoundEmitter EmitAt(this SoundEffect soundEffect, Vector3 position, float pitch, bool paused)
    {
      if (SoundEffectExtensions.SoundManager == null)
        SoundEffectExtensions.SoundManager = ServiceHelper.Get<ISoundManager>();
      return SoundEffectExtensions.SoundManager.AddEmitter(new SoundEmitter(soundEffect, false, pitch, 1f, paused, new Vector3?(position)));
    }

    public static SoundEmitter EmitAt(this SoundEffect soundEffect, Vector3 position, bool loop, float pitch)
    {
      if (SoundEffectExtensions.SoundManager == null)
        SoundEffectExtensions.SoundManager = ServiceHelper.Get<ISoundManager>();
      return SoundEffectExtensions.SoundManager.AddEmitter(new SoundEmitter(soundEffect, loop, pitch, 1f, false, new Vector3?(position)));
    }

    public static SoundEmitter EmitAt(this SoundEffect soundEffect, Vector3 position, bool loop, float pitch, bool paused)
    {
      if (SoundEffectExtensions.SoundManager == null)
        SoundEffectExtensions.SoundManager = ServiceHelper.Get<ISoundManager>();
      return SoundEffectExtensions.SoundManager.AddEmitter(new SoundEmitter(soundEffect, loop, pitch, 1f, paused, new Vector3?(position)));
    }

    public static SoundEmitter EmitAt(this SoundEffect soundEffect, Vector3 position, float pitch, float volume)
    {
      if (SoundEffectExtensions.SoundManager == null)
        SoundEffectExtensions.SoundManager = ServiceHelper.Get<ISoundManager>();
      return SoundEffectExtensions.SoundManager.AddEmitter(new SoundEmitter(soundEffect, false, pitch, volume, false, new Vector3?(position)));
    }

    public static SoundEmitter EmitAt(this SoundEffect soundEffect, Vector3 position, float pitch, float volume, bool paused)
    {
      if (SoundEffectExtensions.SoundManager == null)
        SoundEffectExtensions.SoundManager = ServiceHelper.Get<ISoundManager>();
      return SoundEffectExtensions.SoundManager.AddEmitter(new SoundEmitter(soundEffect, false, pitch, volume, paused, new Vector3?(position)));
    }

    public static SoundEmitter EmitAt(this SoundEffect soundEffect, Vector3 position, bool loop, float pitch, float volume)
    {
      if (SoundEffectExtensions.SoundManager == null)
        SoundEffectExtensions.SoundManager = ServiceHelper.Get<ISoundManager>();
      return SoundEffectExtensions.SoundManager.AddEmitter(new SoundEmitter(soundEffect, loop, pitch, volume, false, new Vector3?(position)));
    }

    public static SoundEmitter EmitAt(this SoundEffect soundEffect, Vector3 position, bool loop, float pitch, float volume, bool paused)
    {
      if (SoundEffectExtensions.SoundManager == null)
        SoundEffectExtensions.SoundManager = ServiceHelper.Get<ISoundManager>();
      return SoundEffectExtensions.SoundManager.AddEmitter(new SoundEmitter(soundEffect, loop, pitch, volume, paused, new Vector3?(position)));
    }
  }
}
