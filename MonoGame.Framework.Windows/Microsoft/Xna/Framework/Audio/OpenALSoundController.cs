// Type: Microsoft.Xna.Framework.Audio.OpenALSoundController
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

using Microsoft.Xna.Framework;
using OpenTK.Audio;
using OpenTK.Audio.OpenAL;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Microsoft.Xna.Framework.Audio
{
  internal sealed class OpenALSoundController : IDisposable
  {
    private static OpenALSoundController instance = new OpenALSoundController();
    private float lowpassGainHf = 1f;
    private const int PreallocatedBuffers = 24;
    private const int PreallocatedSources = 16;
    private const float BufferTimeout = 10f;
    private readonly AudioContext context;
    private readonly Stack<int> freeSources;
    private readonly HashSet<int> filteredSources;
    private readonly List<SoundEffectInstance> activeSoundEffects;
    private readonly Stack<int> freeBuffers;
    private readonly Dictionary<SoundEffect, OpenALSoundController.BufferAllocation> allocatedBuffers;
    private readonly int filterId;
    private int totalSources;
    private int totalBuffers;
    private readonly List<KeyValuePair<SoundEffect, OpenALSoundController.BufferAllocation>> staleAllocations;

    public static OpenALSoundController Instance
    {
      get
      {
        return OpenALSoundController.instance;
      }
    }

    public float LowPassHFGain
    {
      set
      {
        if (!ALHelper.Efx.IsInitialized)
          return;
        lock (this.filteredSources)
        {
          foreach (int item_0 in this.filteredSources)
          {
            ALHelper.Efx.Filter(this.filterId, EfxFilterf.LowpassGainHF, MathHelper.Clamp(value, 0.0f, 1f));
            ALHelper.Efx.BindFilterToSource(item_0, this.filterId);
          }
        }
        ALHelper.Check();
        this.lowpassGainHf = value;
      }
    }

    static OpenALSoundController()
    {
    }

    private OpenALSoundController()
    {
      this.context = new AudioContext();
      this.filterId = ALHelper.Efx.GenFilter();
      ALHelper.Efx.Filter(this.filterId, EfxFilteri.FilterType, 1);
      ALHelper.Efx.Filter(this.filterId, EfxFilterf.LowpassGain, 1f);
      ALHelper.Efx.Filter(this.filterId, EfxFilterf.LowpassGainHF, 1f);
      ALHelper.Check();
      AL.DistanceModel(ALDistanceModel.InverseDistanceClamped);
      ALHelper.Check();
      this.freeBuffers = new Stack<int>(24);
      this.ExpandBuffers();
      this.allocatedBuffers = new Dictionary<SoundEffect, OpenALSoundController.BufferAllocation>(24);
      this.staleAllocations = new List<KeyValuePair<SoundEffect, OpenALSoundController.BufferAllocation>>();
      this.filteredSources = new HashSet<int>();
      this.activeSoundEffects = new List<SoundEffectInstance>();
      this.freeSources = new Stack<int>(16);
      this.ExpandSources();
    }

    public int RegisterSfxInstance(SoundEffectInstance instance, bool forceNoFilter = false)
    {
      this.activeSoundEffects.Add(instance);
      bool filter = !forceNoFilter && !instance.SoundEffect.Name.Contains("Ui") && !instance.SoundEffect.Name.Contains("Warp") && !instance.SoundEffect.Name.Contains("Zoom");
      return this.TakeSourceFor(instance.SoundEffect, filter);
    }

    public void Update(GameTime gameTime)
    {
      for (int index = this.activeSoundEffects.Count - 1; index >= 0; --index)
      {
        SoundEffectInstance soundEffectInstance = this.activeSoundEffects[index];
        if (soundEffectInstance.RefreshState())
        {
          if (!soundEffectInstance.IsDisposed)
            soundEffectInstance.Dispose();
          this.activeSoundEffects.RemoveAt(index);
        }
      }
      float num = (float) gameTime.ElapsedGameTime.TotalSeconds;
      foreach (KeyValuePair<SoundEffect, OpenALSoundController.BufferAllocation> keyValuePair in this.allocatedBuffers)
      {
        if (keyValuePair.Value.SourceCount == 0)
        {
          keyValuePair.Value.SinceUnused += num;
          if ((double) keyValuePair.Value.SinceUnused >= 10.0)
            this.staleAllocations.Add(keyValuePair);
        }
      }
      foreach (KeyValuePair<SoundEffect, OpenALSoundController.BufferAllocation> keyValuePair in this.staleAllocations)
      {
        this.allocatedBuffers.Remove(keyValuePair.Key);
        this.freeBuffers.Push(keyValuePair.Value.BufferId);
      }
      this.TidySources();
      this.TidyBuffers();
      this.staleAllocations.Clear();
    }

    public void RegisterSoundEffect(SoundEffect soundEffect)
    {
      if (this.allocatedBuffers.ContainsKey(soundEffect))
        return;
      if (this.freeBuffers.Count == 0)
        this.ExpandBuffers();
      Trace.WriteLine("[OpenAL] Pre-allocating buffer for " + soundEffect.Name);
      OpenALSoundController.BufferAllocation bufferAllocation;
      this.allocatedBuffers.Add(soundEffect, bufferAllocation = new OpenALSoundController.BufferAllocation()
      {
        BufferId = this.freeBuffers.Pop(),
        SinceUnused = -1f
      });
      AL.BufferData<byte>(bufferAllocation.BufferId, soundEffect.Format, soundEffect._data, soundEffect.Size, soundEffect.Rate);
      ALHelper.Check();
    }

    public void DestroySoundEffect(SoundEffect soundEffect)
    {
      OpenALSoundController.BufferAllocation bufferAllocation;
      if (!this.allocatedBuffers.TryGetValue(soundEffect, out bufferAllocation))
        return;
      bool flag = false;
      for (int index = this.activeSoundEffects.Count - 1; index >= 0; --index)
      {
        SoundEffectInstance soundEffectInstance = this.activeSoundEffects[index];
        if (soundEffectInstance.SoundEffect == soundEffect)
        {
          if (!soundEffectInstance.IsDisposed)
          {
            flag = true;
            soundEffectInstance.Stop(false);
            soundEffectInstance.Dispose();
          }
          this.activeSoundEffects.RemoveAt(index);
        }
      }
      if (flag)
        Trace.WriteLine("[OpenAL] Delete active sources & buffer for " + soundEffect.Name);
      this.allocatedBuffers.Remove(soundEffect);
      this.freeBuffers.Push(bufferAllocation.BufferId);
    }

    private int TakeSourceFor(SoundEffect soundEffect, bool filter = false)
    {
      if (this.freeSources.Count == 0)
        this.ExpandSources();
      int source = this.freeSources.Pop();
      if (filter && ALHelper.Efx.IsInitialized)
      {
        ALHelper.Efx.Filter(this.filterId, EfxFilterf.LowpassGainHF, MathHelper.Clamp(this.lowpassGainHf, 0.0f, 1f));
        ALHelper.Efx.BindFilterToSource(source, this.filterId);
        lock (this.filteredSources)
          this.filteredSources.Add(source);
      }
      OpenALSoundController.BufferAllocation bufferAllocation;
      if (!this.allocatedBuffers.TryGetValue(soundEffect, out bufferAllocation))
      {
        if (this.freeBuffers.Count == 0)
          this.ExpandBuffers();
        this.allocatedBuffers.Add(soundEffect, bufferAllocation = new OpenALSoundController.BufferAllocation()
        {
          BufferId = this.freeBuffers.Pop()
        });
        AL.BufferData<byte>(bufferAllocation.BufferId, soundEffect.Format, soundEffect._data, soundEffect.Size, soundEffect.Rate);
        ALHelper.Check();
      }
      ++bufferAllocation.SourceCount;
      AL.BindBufferToSource(source, bufferAllocation.BufferId);
      ALHelper.Check();
      return source;
    }

    public void ReturnSourceFor(SoundEffect soundEffect, int sourceId)
    {
      OpenALSoundController.BufferAllocation bufferAllocation;
      if (!this.allocatedBuffers.TryGetValue(soundEffect, out bufferAllocation))
        throw new InvalidOperationException(soundEffect.Name + " not found");
      --bufferAllocation.SourceCount;
      if (bufferAllocation.SourceCount == 0)
        bufferAllocation.SinceUnused = 0.0f;
      this.ReturnSource(sourceId);
    }

    public int[] TakeBuffers(int count)
    {
      if (this.freeBuffers.Count < count)
        this.ExpandBuffers();
      int[] numArray = new int[count];
      for (int index = 0; index < count; ++index)
        numArray[index] = this.freeBuffers.Pop();
      return numArray;
    }

    public int TakeSource()
    {
      if (this.freeSources.Count == 0)
        this.ExpandSources();
      int source = this.freeSources.Pop();
      if (ALHelper.Efx.IsInitialized)
      {
        lock (this.filteredSources)
          this.filteredSources.Add(source);
        ALHelper.Efx.Filter(this.filterId, EfxFilterf.LowpassGainHF, MathHelper.Clamp(this.lowpassGainHf, 0.0f, 1f));
        ALHelper.Efx.BindFilterToSource(source, this.filterId);
      }
      return source;
    }

    public void SetSourceFiltered(int sourceId, bool filtered)
    {
      if (!ALHelper.Efx.IsInitialized)
        return;
      lock (this.filteredSources)
      {
        if (!filtered && this.filteredSources.Remove(sourceId))
        {
          ALHelper.Efx.Filter(this.filterId, EfxFilterf.LowpassGainHF, 1f);
          ALHelper.Efx.BindFilterToSource(sourceId, 0);
        }
        else if (filtered && !this.filteredSources.Contains(sourceId))
        {
          this.filteredSources.Add(sourceId);
          ALHelper.Efx.Filter(this.filterId, EfxFilterf.LowpassGainHF, MathHelper.Clamp(this.lowpassGainHf, 0.0f, 1f));
          ALHelper.Efx.BindFilterToSource(sourceId, this.filterId);
        }
      }
    }

    public void ReturnBuffers(int[] bufferIds)
    {
      foreach (int num in bufferIds)
        this.freeBuffers.Push(num);
    }

    public void ReturnSource(int sourceId)
    {
      this.ResetSource(sourceId);
    }

    private void ResetSource(int sourceId)
    {
      AL.Source(sourceId, ALSourceb.Looping, false);
      AL.Source(sourceId, ALSource3f.Position, 0.0f, 0.0f, 0.1f);
      AL.Source(sourceId, ALSourcef.Pitch, 1f);
      AL.Source(sourceId, ALSourcef.Gain, 1f);
      AL.Source(sourceId, ALSourcei.Buffer, 0);
      lock (this.filteredSources)
      {
        if (ALHelper.Efx.IsInitialized && this.filteredSources.Remove(sourceId))
          ALHelper.Efx.BindFilterToSource(sourceId, 0);
      }
      ALHelper.Check();
      this.freeSources.Push(sourceId);
    }

    private void ExpandBuffers()
    {
      this.totalBuffers += 24;
      Trace.WriteLine("[OpenAL] Expanding buffers to " + (object) this.totalBuffers);
      int[] numArray = AL.GenBuffers(24);
      ALHelper.Check();
      if (ALHelper.XRam.IsInitialized)
      {
        ALHelper.XRam.SetBufferMode(numArray.Length, ref numArray[0], XRamExtension.XRamStorage.Hardware);
        ALHelper.Check();
      }
      foreach (int num in numArray)
        this.freeBuffers.Push(num);
    }

    private void ExpandSources()
    {
      this.totalSources += 16;
      Trace.WriteLine("[OpenAL] Expanding sources to " + (object) this.totalSources);
      int[] numArray = AL.GenSources(16);
      ALHelper.Check();
      foreach (int num in numArray)
        this.freeSources.Push(num);
    }

    private void TidySources()
    {
      bool flag = false;
      if (this.freeSources.Count <= 32)
        return;
      AL.DeleteSource(this.freeSources.Pop());
      ALHelper.Check();
      --this.totalSources;
      flag = true;
    }

    private void TidyBuffers()
    {
      bool flag = false;
      if (this.freeBuffers.Count <= 48)
        return;
      AL.DeleteBuffer(this.freeBuffers.Pop());
      ALHelper.Check();
      --this.totalBuffers;
      flag = true;
    }

    public void Dispose()
    {
      if (ALHelper.Efx.IsInitialized)
        ALHelper.Efx.DeleteFilter(this.filterId);
      while (this.freeSources.Count > 0)
        AL.DeleteSource(this.freeSources.Pop());
      while (this.freeBuffers.Count > 0)
        AL.DeleteBuffer(this.freeBuffers.Pop());
      this.context.Dispose();
      OpenALSoundController.instance = (OpenALSoundController) null;
    }

    private class BufferAllocation
    {
      public int BufferId;
      public int SourceCount;
      public float SinceUnused;
    }
  }
}
