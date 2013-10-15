// Type: Microsoft.Xna.Framework.Audio.Cue
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

using System;

namespace Microsoft.Xna.Framework.Audio
{
  public class Cue : IDisposable
  {
    private bool paused = false;
    private float volume = 1f;
    private AudioEngine engine;
    private string name;
    private XactSound[] sounds;
    private float[] probs;
    private XactSound curSound;
    private Random variationRand;

    public bool IsPaused
    {
      get
      {
        return this.paused;
      }
    }

    public bool IsPlaying
    {
      get
      {
        if (this.curSound != null)
          return this.curSound.Playing;
        else
          return false;
      }
    }

    public bool IsStopped
    {
      get
      {
        if (this.curSound != null)
          return !this.curSound.Playing;
        else
          return true;
      }
    }

    public string Name
    {
      get
      {
        return this.name;
      }
    }

    public bool IsDisposed
    {
      get
      {
        return false;
      }
    }

    internal Cue(AudioEngine engine, string cuename, XactSound sound)
    {
      this.engine = engine;
      this.name = cuename;
      this.sounds = new XactSound[1];
      this.sounds[0] = sound;
      this.probs = new float[1];
      this.probs[0] = 1f;
      this.variationRand = new Random();
    }

    internal Cue(string cuename, XactSound[] _sounds, float[] _probs)
    {
      this.name = cuename;
      this.sounds = _sounds;
      this.probs = _probs;
      this.variationRand = new Random();
    }

    public void Pause()
    {
      if (this.curSound != null)
        this.curSound.Pause();
      this.paused = true;
    }

    public void Play()
    {
      this.curSound = this.sounds[this.variationRand.Next(this.sounds.Length)];
      this.curSound.Volume = this.volume;
      this.curSound.Play();
      this.paused = false;
    }

    public void Resume()
    {
      if (this.curSound != null)
        this.curSound.Resume();
      this.paused = false;
    }

    public void Stop(AudioStopOptions options)
    {
      if (this.curSound != null)
        this.curSound.Stop();
      this.paused = false;
    }

    public void SetVariable(string name, float value)
    {
      if (name == "Volume")
      {
        this.volume = value;
        if (this.curSound == null)
          return;
        this.curSound.Volume = value;
      }
      else
        this.engine.SetGlobalVariable(name, value);
    }

    public float GetVariable(string name, float value)
    {
      if (name == "Volume")
        return this.volume;
      else
        return this.engine.GetGlobalVariable(name);
    }

    public void Apply3D(AudioListener listener, AudioEmitter emitter)
    {
    }

    public void Dispose()
    {
    }
  }
}
