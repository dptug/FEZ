// Type: Microsoft.Xna.Framework.Audio.OpenALSoundController
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

using Microsoft.Xna.Framework;
using OpenTK.Audio;
using OpenTK.Audio.OpenAL;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace Microsoft.Xna.Framework.Audio
{
  internal sealed class OpenALSoundController : IDisposable
  {
    private static OpenALSoundController instance = new OpenALSoundController();
    private static readonly ReaderWriterLockSlim ActiveLock = new ReaderWriterLockSlim();
    private static readonly ReaderWriterLockSlim FilteringLock = new ReaderWriterLockSlim();
    private static readonly ReaderWriterLockSlim AllocationsLock = new ReaderWriterLockSlim();
    private float lowpassGainHf = 1f;
    private const int PreallocatedBuffers = 256;
    private const int PreallocatedSources = 64;
    private const int ExpandSize = 32;
    private const float BufferTimeout = 10f;
    private readonly AudioContext context;
    private readonly ConcurrentStack<int> freeSources;
    private readonly ConcurrentStack<int> freeBuffers;
    private readonly Dictionary<SoundEffect, OpenALSoundController.BufferAllocation> allocatedBuffers;
    private readonly HashSet<int> filteredSources;
    private readonly List<SoundEffectInstance> activeSoundEffects;
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
        OpenALSoundController.FilteringLock.EnterReadLock();
        foreach (int source in this.filteredSources)
        {
          ALHelper.Efx.Filter(this.filterId, EfxFilterf.LowpassGainHF, MathHelper.Clamp(value, 0.0f, 1f));
          ALHelper.Efx.BindFilterToSource(source, this.filterId);
        }
        OpenALSoundController.FilteringLock.ExitReadLock();
        this.lowpassGainHf = value;
      }
    }

    static OpenALSoundController()
    {
    }

    private OpenALSoundController()
    {
      try
      {
        this.context = new AudioContext();
      }
      catch (Exception ex)
      {
        OpenALSoundController.Log("Last error in enumerator is " + AudioDeviceEnumerator.LastError);
        int num = (int) MessageBox.Show("Error initializing audio subsystem. Game will now exit.\n(see debug log for more details)", "OpenAL Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        throw;
      }
      OpenALSoundController.Log("Sound manager initialized!");
      int[] data1 = new int[1];
      IntPtr contextsDevice = Alc.GetContextsDevice(Alc.GetCurrentContext());
      Alc.GetInteger(contextsDevice, AlcGetInteger.AttributesSize, 1, data1);
      int[] data2 = new int[data1[0]];
      Alc.GetInteger(contextsDevice, AlcGetInteger.AllAttributes, data1[0], data2);
      for (int index = 0; index < data2.Length; ++index)
      {
        if (data2[index] == 4112)
        {
          OpenALSoundController.Log("Available mono sources : " + (object) data2[index + 1]);
          break;
        }
      }
      this.filterId = ALHelper.Efx.GenFilter();
      ALHelper.Efx.Filter(this.filterId, EfxFilteri.FilterType, 1);
      ALHelper.Efx.Filter(this.filterId, EfxFilterf.LowpassGain, 1f);
      ALHelper.Efx.Filter(this.filterId, EfxFilterf.LowpassGainHF, 1f);
      AL.DistanceModel(ALDistanceModel.InverseDistanceClamped);
      this.freeBuffers = new ConcurrentStack<int>();
      this.ExpandBuffers(256);
      this.allocatedBuffers = new Dictionary<SoundEffect, OpenALSoundController.BufferAllocation>();
      this.staleAllocations = new List<KeyValuePair<SoundEffect, OpenALSoundController.BufferAllocation>>();
      this.filteredSources = new HashSet<int>();
      this.activeSoundEffects = new List<SoundEffectInstance>();
      this.freeSources = new ConcurrentStack<int>();
      this.ExpandSources(64);
    }

    private static void Log(string message)
    {
      try
      {
        Console.WriteLine("({0}) [{1}] {2}", (object) DateTime.Now.ToString("HH:mm:ss.fff"), (object) "OpenAL", (object) message);
        using (FileStream fileStream = File.Open(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\FEZ\\Debug Log.txt", FileMode.Append))
        {
          using (StreamWriter streamWriter = new StreamWriter((Stream) fileStream))
            streamWriter.WriteLine("({0}) [{1}] {2}", (object) DateTime.Now.ToString("HH:mm:ss.fff"), (object) "OpenAL", (object) message);
        }
      }
      catch (Exception ex)
      {
      }
    }

    public int RegisterSfxInstance(SoundEffectInstance instance, bool forceNoFilter = false)
    {
      OpenALSoundController.ActiveLock.EnterWriteLock();
      this.activeSoundEffects.Add(instance);
      OpenALSoundController.ActiveLock.ExitWriteLock();
      bool filter = !forceNoFilter && !instance.SoundEffect.Name.Contains("Ui") && (!instance.SoundEffect.Name.Contains("Warp") && !instance.SoundEffect.Name.Contains("Zoom")) && !instance.SoundEffect.Name.Contains("Trixel");
      return this.TakeSourceFor(instance.SoundEffect, filter);
    }

    public void Update(GameTime gameTime)
    {
      OpenALSoundController.ActiveLock.EnterUpgradeableReadLock();
      for (int index = this.activeSoundEffects.Count - 1; index >= 0; --index)
      {
        SoundEffectInstance soundEffectInstance = this.activeSoundEffects[index];
        if (soundEffectInstance.RefreshState() || soundEffectInstance.IsDisposed)
        {
          OpenALSoundController.ActiveLock.EnterWriteLock();
          if (!soundEffectInstance.IsDisposed)
            soundEffectInstance.Dispose();
          this.activeSoundEffects.RemoveAt(index);
          OpenALSoundController.ActiveLock.ExitWriteLock();
        }
      }
      OpenALSoundController.ActiveLock.ExitUpgradeableReadLock();
      float num = (float) gameTime.ElapsedGameTime.TotalSeconds;
      OpenALSoundController.AllocationsLock.EnterUpgradeableReadLock();
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
        OpenALSoundController.AllocationsLock.EnterWriteLock();
        this.allocatedBuffers.Remove(keyValuePair.Key);
        OpenALSoundController.AllocationsLock.ExitWriteLock();
        this.freeBuffers.Push(keyValuePair.Value.BufferId);
      }
      OpenALSoundController.AllocationsLock.ExitUpgradeableReadLock();
      this.TidySources();
      this.TidyBuffers();
      this.staleAllocations.Clear();
    }

    public void DestroySoundEffect(SoundEffect soundEffect)
    {
      OpenALSoundController.AllocationsLock.EnterUpgradeableReadLock();
      OpenALSoundController.BufferAllocation bufferAllocation;
      if (!this.allocatedBuffers.TryGetValue(soundEffect, out bufferAllocation))
      {
        OpenALSoundController.AllocationsLock.ExitUpgradeableReadLock();
      }
      else
      {
        bool flag = false;
        OpenALSoundController.ActiveLock.EnterUpgradeableReadLock();
        for (int index = this.activeSoundEffects.Count - 1; index >= 0; --index)
        {
          SoundEffectInstance soundEffectInstance = this.activeSoundEffects[index];
          if (soundEffectInstance.SoundEffect == soundEffect)
          {
            OpenALSoundController.ActiveLock.EnterWriteLock();
            if (!soundEffectInstance.IsDisposed)
            {
              flag = true;
              soundEffectInstance.Stop(false);
              soundEffectInstance.Dispose();
            }
            this.activeSoundEffects.RemoveAt(index);
            OpenALSoundController.ActiveLock.ExitWriteLock();
          }
        }
        OpenALSoundController.ActiveLock.ExitUpgradeableReadLock();
        if (flag)
          Trace.WriteLine("[OpenAL] Delete active sources & buffer for " + soundEffect.Name);
        OpenALSoundController.AllocationsLock.EnterWriteLock();
        this.allocatedBuffers.Remove(soundEffect);
        OpenALSoundController.AllocationsLock.ExitWriteLock();
        this.freeBuffers.Push(bufferAllocation.BufferId);
        OpenALSoundController.AllocationsLock.ExitUpgradeableReadLock();
      }
    }

    private int TakeSourceFor(SoundEffect soundEffect, bool filter = false)
    {
      int result;
      while (!this.freeSources.TryPop(out result))
        this.ExpandSources(32);
      if (filter && ALHelper.Efx.IsInitialized)
      {
        ALHelper.Efx.Filter(this.filterId, EfxFilterf.LowpassGainHF, MathHelper.Clamp(this.lowpassGainHf, 0.0f, 1f));
        ALHelper.Efx.BindFilterToSource(result, this.filterId);
        OpenALSoundController.FilteringLock.EnterWriteLock();
        this.filteredSources.Add(result);
        OpenALSoundController.FilteringLock.ExitWriteLock();
      }
      OpenALSoundController.AllocationsLock.EnterUpgradeableReadLock();
      OpenALSoundController.BufferAllocation bufferAllocation;
      if (!this.allocatedBuffers.TryGetValue(soundEffect, out bufferAllocation))
      {
        bufferAllocation = new OpenALSoundController.BufferAllocation();
        while (!this.freeBuffers.TryPop(out bufferAllocation.BufferId))
          this.ExpandBuffers(32);
        OpenALSoundController.AllocationsLock.EnterWriteLock();
        this.allocatedBuffers.Add(soundEffect, bufferAllocation);
        OpenALSoundController.AllocationsLock.ExitWriteLock();
        AL.BufferData<byte>(bufferAllocation.BufferId, soundEffect.Format, soundEffect._data, soundEffect.Size, soundEffect.Rate);
      }
      ++bufferAllocation.SourceCount;
      AL.BindBufferToSource(result, bufferAllocation.BufferId);
      OpenALSoundController.AllocationsLock.ExitUpgradeableReadLock();
      return result;
    }

    public void ReturnSourceFor(SoundEffect soundEffect, int sourceId)
    {
      OpenALSoundController.AllocationsLock.EnterReadLock();
      OpenALSoundController.BufferAllocation bufferAllocation;
      if (!this.allocatedBuffers.TryGetValue(soundEffect, out bufferAllocation))
        throw new InvalidOperationException(soundEffect.Name + " not found");
      --bufferAllocation.SourceCount;
      if (bufferAllocation.SourceCount == 0)
        bufferAllocation.SinceUnused = 0.0f;
      OpenALSoundController.AllocationsLock.ExitReadLock();
      this.ReturnSource(sourceId);
    }

    public int[] TakeBuffers(int count)
    {
      int[] items = new int[count];
      int startIndex = 0;
      while ((startIndex += this.freeBuffers.TryPopRange(items, startIndex, count - startIndex)) < count)
        this.ExpandBuffers(32);
      return items;
    }

    public int TakeSource()
    {
      int result;
      while (!this.freeSources.TryPop(out result))
        this.ExpandSources(32);
      if (ALHelper.Efx.IsInitialized)
      {
        OpenALSoundController.FilteringLock.EnterWriteLock();
        this.filteredSources.Add(result);
        ALHelper.Efx.Filter(this.filterId, EfxFilterf.LowpassGainHF, MathHelper.Clamp(this.lowpassGainHf, 0.0f, 1f));
        ALHelper.Efx.BindFilterToSource(result, this.filterId);
        OpenALSoundController.FilteringLock.ExitWriteLock();
      }
      return result;
    }

    public void SetSourceFiltered(int sourceId, bool filtered)
    {
      if (!ALHelper.Efx.IsInitialized)
        return;
      OpenALSoundController.FilteringLock.EnterWriteLock();
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
      OpenALSoundController.FilteringLock.ExitWriteLock();
    }

    public void ReturnBuffers(int[] bufferIds)
    {
      this.freeBuffers.PushRange(bufferIds);
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
      AL.SourceStop(sourceId);
      AL.Source(sourceId, ALSourcei.Buffer, 0);
      if (ALHelper.Efx.IsInitialized)
      {
        OpenALSoundController.FilteringLock.EnterWriteLock();
        if (this.filteredSources.Remove(sourceId))
          ALHelper.Efx.BindFilterToSource(sourceId, 0);
        OpenALSoundController.FilteringLock.ExitWriteLock();
      }
      this.freeSources.Push(sourceId);
    }

    private void ExpandBuffers(int expandSize = 32)
    {
      this.totalBuffers += expandSize;
      Trace.WriteLine("[OpenAL] Expanding buffers to " + (object) this.totalBuffers);
      int[] items = AL.GenBuffers(expandSize);
      if (ALHelper.XRam.IsInitialized)
        ALHelper.XRam.SetBufferMode(items.Length, ref items[0], XRamExtension.XRamStorage.Hardware);
      Array.Reverse((Array) items);
      this.freeBuffers.PushRange(items);
    }

    private void ExpandSources(int expandSize = 32)
    {
      this.totalSources += expandSize;
      Trace.WriteLine("[OpenAL] Expanding sources to " + (object) this.totalSources);
      int[] items = AL.GenSources(expandSize);
      Array.Reverse((Array) items);
      this.freeSources.PushRange(items);
    }

    private void TidySources()
    {
      bool flag = false;
      int result;
      if (this.freeSources.Count > 128 && this.freeSources.TryPop(out result))
      {
        AL.DeleteSource(result);
        --this.totalSources;
        flag = true;
      }
      if (!flag)
        return;
      Trace.WriteLine("[OpenAL] Tidied sources down to " + (object) this.totalSources);
    }

    private void TidyBuffers()
    {
      bool flag = false;
      int result;
      if (this.freeBuffers.Count > 512 && this.freeBuffers.TryPop(out result))
      {
        AL.DeleteBuffer(result);
        --this.totalBuffers;
        flag = true;
      }
      if (!flag)
        return;
      Trace.WriteLine("[OpenAL] Tidied buffers down to " + (object) this.totalBuffers);
    }

    public void Dispose()
    {
      if (ALHelper.Efx.IsInitialized)
        ALHelper.Efx.DeleteFilter(this.filterId);
      int result;
      while (this.freeSources.TryPop(out result))
        AL.DeleteSource(result);
      while (this.freeBuffers.TryPop(out result))
        AL.DeleteBuffer(result);
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
