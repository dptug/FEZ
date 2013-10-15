// Type: OpenTK.Audio.OpenAL.EffectsExtension
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using OpenTK;
using OpenTK.Audio;
using System;
using System.Runtime.InteropServices;

namespace OpenTK.Audio.OpenAL
{
  public class EffectsExtension
  {
    private EffectsExtension.Delegate_alGenEffects Imported_alGenEffects;
    private EffectsExtension.Delegate_alDeleteEffects Imported_alDeleteEffects;
    private EffectsExtension.Delegate_alIsEffect Imported_alIsEffect;
    private EffectsExtension.Delegate_alEffecti Imported_alEffecti;
    private EffectsExtension.Delegate_alEffectf Imported_alEffectf;
    private EffectsExtension.Delegate_alEffectfv Imported_alEffectfv;
    private EffectsExtension.Delegate_alGetEffecti Imported_alGetEffecti;
    private EffectsExtension.Delegate_alGetEffectf Imported_alGetEffectf;
    private EffectsExtension.Delegate_alGetEffectfv Imported_alGetEffectfv;
    private EffectsExtension.Delegate_alGenFilters Imported_alGenFilters;
    private EffectsExtension.Delegate_alDeleteFilters Imported_alDeleteFilters;
    private EffectsExtension.Delegate_alIsFilter Imported_alIsFilter;
    private EffectsExtension.Delegate_alFilteri Imported_alFilteri;
    private EffectsExtension.Delegate_alFilterf Imported_alFilterf;
    private EffectsExtension.Delegate_alGetFilteri Imported_alGetFilteri;
    private EffectsExtension.Delegate_alGetFilterf Imported_alGetFilterf;
    private EffectsExtension.Delegate_alGenAuxiliaryEffectSlots Imported_alGenAuxiliaryEffectSlots;
    private EffectsExtension.Delegate_alDeleteAuxiliaryEffectSlots Imported_alDeleteAuxiliaryEffectSlots;
    private EffectsExtension.Delegate_alIsAuxiliaryEffectSlot Imported_alIsAuxiliaryEffectSlot;
    private EffectsExtension.Delegate_alAuxiliaryEffectSloti Imported_alAuxiliaryEffectSloti;
    private EffectsExtension.Delegate_alAuxiliaryEffectSlotf Imported_alAuxiliaryEffectSlotf;
    private EffectsExtension.Delegate_alGetAuxiliaryEffectSloti Imported_alGetAuxiliaryEffectSloti;
    private EffectsExtension.Delegate_alGetAuxiliaryEffectSlotf Imported_alGetAuxiliaryEffectSlotf;
    private bool _valid;

    public bool IsInitialized
    {
      get
      {
        return this._valid;
      }
    }

    public EffectsExtension()
    {
      this._valid = false;
      if (AudioContext.CurrentContext == null)
        throw new InvalidOperationException("AL.LoadAll() needs a current AudioContext.");
      if (!AudioContext.CurrentContext.SupportsExtension("ALC_EXT_EFX"))
        return;
      try
      {
        this.Imported_alGenEffects = (EffectsExtension.Delegate_alGenEffects) Marshal.GetDelegateForFunctionPointer(AL.GetProcAddress("alGenEffects"), typeof (EffectsExtension.Delegate_alGenEffects));
        this.Imported_alDeleteEffects = (EffectsExtension.Delegate_alDeleteEffects) Marshal.GetDelegateForFunctionPointer(AL.GetProcAddress("alDeleteEffects"), typeof (EffectsExtension.Delegate_alDeleteEffects));
        this.Imported_alIsEffect = (EffectsExtension.Delegate_alIsEffect) Marshal.GetDelegateForFunctionPointer(AL.GetProcAddress("alIsEffect"), typeof (EffectsExtension.Delegate_alIsEffect));
        this.Imported_alEffecti = (EffectsExtension.Delegate_alEffecti) Marshal.GetDelegateForFunctionPointer(AL.GetProcAddress("alEffecti"), typeof (EffectsExtension.Delegate_alEffecti));
        this.Imported_alEffectf = (EffectsExtension.Delegate_alEffectf) Marshal.GetDelegateForFunctionPointer(AL.GetProcAddress("alEffectf"), typeof (EffectsExtension.Delegate_alEffectf));
        this.Imported_alEffectfv = (EffectsExtension.Delegate_alEffectfv) Marshal.GetDelegateForFunctionPointer(AL.GetProcAddress("alEffectfv"), typeof (EffectsExtension.Delegate_alEffectfv));
        this.Imported_alGetEffecti = (EffectsExtension.Delegate_alGetEffecti) Marshal.GetDelegateForFunctionPointer(AL.GetProcAddress("alGetEffecti"), typeof (EffectsExtension.Delegate_alGetEffecti));
        this.Imported_alGetEffectf = (EffectsExtension.Delegate_alGetEffectf) Marshal.GetDelegateForFunctionPointer(AL.GetProcAddress("alGetEffectf"), typeof (EffectsExtension.Delegate_alGetEffectf));
        this.Imported_alGetEffectfv = (EffectsExtension.Delegate_alGetEffectfv) Marshal.GetDelegateForFunctionPointer(AL.GetProcAddress("alGetEffectfv"), typeof (EffectsExtension.Delegate_alGetEffectfv));
      }
      catch (Exception ex)
      {
        return;
      }
      try
      {
        this.Imported_alGenFilters = (EffectsExtension.Delegate_alGenFilters) Marshal.GetDelegateForFunctionPointer(AL.GetProcAddress("alGenFilters"), typeof (EffectsExtension.Delegate_alGenFilters));
        this.Imported_alDeleteFilters = (EffectsExtension.Delegate_alDeleteFilters) Marshal.GetDelegateForFunctionPointer(AL.GetProcAddress("alDeleteFilters"), typeof (EffectsExtension.Delegate_alDeleteFilters));
        this.Imported_alIsFilter = (EffectsExtension.Delegate_alIsFilter) Marshal.GetDelegateForFunctionPointer(AL.GetProcAddress("alIsFilter"), typeof (EffectsExtension.Delegate_alIsFilter));
        this.Imported_alFilteri = (EffectsExtension.Delegate_alFilteri) Marshal.GetDelegateForFunctionPointer(AL.GetProcAddress("alFilteri"), typeof (EffectsExtension.Delegate_alFilteri));
        this.Imported_alFilterf = (EffectsExtension.Delegate_alFilterf) Marshal.GetDelegateForFunctionPointer(AL.GetProcAddress("alFilterf"), typeof (EffectsExtension.Delegate_alFilterf));
        this.Imported_alGetFilteri = (EffectsExtension.Delegate_alGetFilteri) Marshal.GetDelegateForFunctionPointer(AL.GetProcAddress("alGetFilteri"), typeof (EffectsExtension.Delegate_alGetFilteri));
        this.Imported_alGetFilterf = (EffectsExtension.Delegate_alGetFilterf) Marshal.GetDelegateForFunctionPointer(AL.GetProcAddress("alGetFilterf"), typeof (EffectsExtension.Delegate_alGetFilterf));
      }
      catch (Exception ex)
      {
        return;
      }
      try
      {
        this.Imported_alGenAuxiliaryEffectSlots = (EffectsExtension.Delegate_alGenAuxiliaryEffectSlots) Marshal.GetDelegateForFunctionPointer(AL.GetProcAddress("alGenAuxiliaryEffectSlots"), typeof (EffectsExtension.Delegate_alGenAuxiliaryEffectSlots));
        this.Imported_alDeleteAuxiliaryEffectSlots = (EffectsExtension.Delegate_alDeleteAuxiliaryEffectSlots) Marshal.GetDelegateForFunctionPointer(AL.GetProcAddress("alDeleteAuxiliaryEffectSlots"), typeof (EffectsExtension.Delegate_alDeleteAuxiliaryEffectSlots));
        this.Imported_alIsAuxiliaryEffectSlot = (EffectsExtension.Delegate_alIsAuxiliaryEffectSlot) Marshal.GetDelegateForFunctionPointer(AL.GetProcAddress("alIsAuxiliaryEffectSlot"), typeof (EffectsExtension.Delegate_alIsAuxiliaryEffectSlot));
        this.Imported_alAuxiliaryEffectSloti = (EffectsExtension.Delegate_alAuxiliaryEffectSloti) Marshal.GetDelegateForFunctionPointer(AL.GetProcAddress("alAuxiliaryEffectSloti"), typeof (EffectsExtension.Delegate_alAuxiliaryEffectSloti));
        this.Imported_alAuxiliaryEffectSlotf = (EffectsExtension.Delegate_alAuxiliaryEffectSlotf) Marshal.GetDelegateForFunctionPointer(AL.GetProcAddress("alAuxiliaryEffectSlotf"), typeof (EffectsExtension.Delegate_alAuxiliaryEffectSlotf));
        this.Imported_alGetAuxiliaryEffectSloti = (EffectsExtension.Delegate_alGetAuxiliaryEffectSloti) Marshal.GetDelegateForFunctionPointer(AL.GetProcAddress("alGetAuxiliaryEffectSloti"), typeof (EffectsExtension.Delegate_alGetAuxiliaryEffectSloti));
        this.Imported_alGetAuxiliaryEffectSlotf = (EffectsExtension.Delegate_alGetAuxiliaryEffectSlotf) Marshal.GetDelegateForFunctionPointer(AL.GetProcAddress("alGetAuxiliaryEffectSlotf"), typeof (EffectsExtension.Delegate_alGetAuxiliaryEffectSlotf));
      }
      catch (Exception ex)
      {
        return;
      }
      this._valid = true;
    }

    [CLSCompliant(false)]
    public void BindEffect(uint eid, EfxEffectType type)
    {
      this.Imported_alEffecti(eid, EfxEffecti.EffectType, (int) type);
    }

    public void BindEffect(int eid, EfxEffectType type)
    {
      this.Imported_alEffecti((uint) eid, EfxEffecti.EffectType, (int) type);
    }

    [CLSCompliant(false)]
    public void BindFilterToSource(uint source, uint filter)
    {
      AL.Source(source, ALSourcei.EfxDirectFilter, (int) filter);
    }

    public void BindFilterToSource(int source, int filter)
    {
      AL.Source((uint) source, ALSourcei.EfxDirectFilter, filter);
    }

    [CLSCompliant(false)]
    public void BindEffectToAuxiliarySlot(uint auxiliaryeffectslot, uint effect)
    {
      this.AuxiliaryEffectSlot(auxiliaryeffectslot, EfxAuxiliaryi.EffectslotEffect, (int) effect);
    }

    public void BindEffectToAuxiliarySlot(int auxiliaryeffectslot, int effect)
    {
      this.AuxiliaryEffectSlot((uint) auxiliaryeffectslot, EfxAuxiliaryi.EffectslotEffect, effect);
    }

    [CLSCompliant(false)]
    public void BindSourceToAuxiliarySlot(uint source, uint slot, int slotnumber, uint filter)
    {
      AL.Source(source, ALSource3i.EfxAuxiliarySendFilter, (int) slot, slotnumber, (int) filter);
    }

    public void BindSourceToAuxiliarySlot(int source, int slot, int slotnumber, int filter)
    {
      AL.Source((uint) source, ALSource3i.EfxAuxiliarySendFilter, slot, slotnumber, filter);
    }

    [CLSCompliant(false)]
    public unsafe void GenEffects(int n, out uint effects)
    {
      fixed (uint* effects1 = &effects)
      {
        this.Imported_alGenEffects(n, effects1);
        effects = *effects1;
      }
    }

    public unsafe void GenEffects(int n, out int effects)
    {
      fixed (int* numPtr = &effects)
      {
        this.Imported_alGenEffects(n, (uint*) numPtr);
        effects = *numPtr;
      }
    }

    public int[] GenEffects(int n)
    {
      if (n <= 0)
        throw new ArgumentOutOfRangeException("n", "Must be higher than 0.");
      int[] numArray = new int[n];
      this.GenEffects(n, out numArray[0]);
      return numArray;
    }

    public int GenEffect()
    {
      int effects;
      this.GenEffects(1, out effects);
      return effects;
    }

    [CLSCompliant(false)]
    public unsafe void GenEffect(out uint effect)
    {
      fixed (uint* effects = &effect)
      {
        this.Imported_alGenEffects(1, effects);
        effect = *effects;
      }
    }

    [CLSCompliant(false)]
    public unsafe void DeleteEffects(int n, ref uint effects)
    {
      fixed (uint* effects1 = &effects)
        this.Imported_alDeleteEffects(n, effects1);
    }

    public unsafe void DeleteEffects(int n, ref int effects)
    {
      fixed (int* numPtr = &effects)
        this.Imported_alDeleteEffects(n, (uint*) numPtr);
    }

    public void DeleteEffects(int[] effects)
    {
      if (effects == null)
        throw new ArgumentNullException("effects");
      this.DeleteEffects(effects.Length, ref effects[0]);
    }

    [CLSCompliant(false)]
    public void DeleteEffects(uint[] effects)
    {
      if (effects == null)
        throw new ArgumentNullException("effects");
      this.DeleteEffects(effects.Length, ref effects[0]);
    }

    public void DeleteEffect(int effect)
    {
      this.DeleteEffects(1, ref effect);
    }

    [CLSCompliant(false)]
    public unsafe void DeleteEffect(ref uint effect)
    {
      fixed (uint* effects = &effect)
        this.Imported_alDeleteEffects(1, effects);
    }

    [CLSCompliant(false)]
    public bool IsEffect(uint eid)
    {
      return this.Imported_alIsEffect(eid);
    }

    public bool IsEffect(int eid)
    {
      return this.Imported_alIsEffect((uint) eid);
    }

    [CLSCompliant(false)]
    public void Effect(uint eid, EfxEffecti param, int value)
    {
      this.Imported_alEffecti(eid, param, value);
    }

    public void Effect(int eid, EfxEffecti param, int value)
    {
      this.Imported_alEffecti((uint) eid, param, value);
    }

    [CLSCompliant(false)]
    public void Effect(uint eid, EfxEffectf param, float value)
    {
      this.Imported_alEffectf(eid, param, value);
    }

    public void Effect(int eid, EfxEffectf param, float value)
    {
      this.Imported_alEffectf((uint) eid, param, value);
    }

    [CLSCompliant(false)]
    public unsafe void Effect(uint eid, EfxEffect3f param, ref Vector3 values)
    {
      fixed (float* values1 = &values.X)
        this.Imported_alEffectfv(eid, param, values1);
    }

    public void Effect(int eid, EfxEffect3f param, ref Vector3 values)
    {
      this.Effect((uint) eid, param, ref values);
    }

    [CLSCompliant(false)]
    public unsafe void GetEffect(uint eid, EfxEffecti pname, out int value)
    {
      fixed (int* numPtr = &value)
        this.Imported_alGetEffecti(eid, pname, numPtr);
    }

    public void GetEffect(int eid, EfxEffecti pname, out int value)
    {
      this.GetEffect((uint) eid, pname, out value);
    }

    [CLSCompliant(false)]
    public unsafe void GetEffect(uint eid, EfxEffectf pname, out float value)
    {
      fixed (float* numPtr = &value)
        this.Imported_alGetEffectf(eid, pname, numPtr);
    }

    public void GetEffect(int eid, EfxEffectf pname, out float value)
    {
      this.GetEffect((uint) eid, pname, out value);
    }

    [CLSCompliant(false)]
    public unsafe void GetEffect(uint eid, EfxEffect3f param, out Vector3 values)
    {
      fixed (float* values1 = &values.X)
      {
        this.Imported_alGetEffectfv(eid, param, values1);
        values.X = *values1;
        values.Y = values1[1];
        values.Z = values1[2];
      }
    }

    public void GetEffect(int eid, EfxEffect3f param, out Vector3 values)
    {
      this.GetEffect((uint) eid, param, out values);
    }

    [CLSCompliant(false)]
    public unsafe void GenFilters(int n, out uint filters)
    {
      fixed (uint* filters1 = &filters)
      {
        this.Imported_alGenFilters(n, filters1);
        filters = *filters1;
      }
    }

    public unsafe void GenFilters(int n, out int filters)
    {
      fixed (int* numPtr = &filters)
      {
        this.Imported_alGenFilters(n, (uint*) numPtr);
        filters = *numPtr;
      }
    }

    public int[] GenFilters(int n)
    {
      if (n <= 0)
        throw new ArgumentOutOfRangeException("n", "Must be higher than 0.");
      int[] numArray = new int[n];
      this.GenFilters(numArray.Length, out numArray[0]);
      return numArray;
    }

    public int GenFilter()
    {
      int filters;
      this.GenFilters(1, out filters);
      return filters;
    }

    [CLSCompliant(false)]
    public unsafe void GenFilter(out uint filter)
    {
      fixed (uint* filters = &filter)
      {
        this.Imported_alGenFilters(1, filters);
        filter = *filters;
      }
    }

    [CLSCompliant(false)]
    public unsafe void DeleteFilters(int n, ref uint filters)
    {
      fixed (uint* filters1 = &filters)
        this.Imported_alDeleteFilters(n, filters1);
    }

    public unsafe void DeleteFilters(int n, ref int filters)
    {
      fixed (int* numPtr = &filters)
        this.Imported_alDeleteFilters(n, (uint*) numPtr);
    }

    [CLSCompliant(false)]
    public void DeleteFilters(uint[] filters)
    {
      if (filters == null)
        throw new ArgumentNullException("filters");
      this.DeleteFilters(filters.Length, ref filters[0]);
    }

    public void DeleteFilters(int[] filters)
    {
      if (filters == null)
        throw new ArgumentNullException("filters");
      this.DeleteFilters(filters.Length, ref filters[0]);
    }

    public void DeleteFilter(int filter)
    {
      this.DeleteFilters(1, ref filter);
    }

    [CLSCompliant(false)]
    public unsafe void DeleteFilter(ref uint filter)
    {
      fixed (uint* filters = &filter)
        this.Imported_alDeleteFilters(1, filters);
    }

    [CLSCompliant(false)]
    public bool IsFilter(uint fid)
    {
      return this.Imported_alIsFilter(fid);
    }

    public bool IsFilter(int fid)
    {
      return this.Imported_alIsFilter((uint) fid);
    }

    [CLSCompliant(false)]
    public void Filter(uint fid, EfxFilteri param, int value)
    {
      this.Imported_alFilteri(fid, param, value);
    }

    public void Filter(int fid, EfxFilteri param, int value)
    {
      this.Imported_alFilteri((uint) fid, param, value);
    }

    [CLSCompliant(false)]
    public void Filter(uint fid, EfxFilterf param, float value)
    {
      this.Imported_alFilterf(fid, param, value);
    }

    public void Filter(int fid, EfxFilterf param, float value)
    {
      this.Imported_alFilterf((uint) fid, param, value);
    }

    [CLSCompliant(false)]
    public unsafe void GetFilter(uint fid, EfxFilteri pname, out int value)
    {
      fixed (int* numPtr = &value)
        this.Imported_alGetFilteri(fid, pname, numPtr);
    }

    public void GetFilter(int fid, EfxFilteri pname, out int value)
    {
      this.GetFilter((uint) fid, pname, out value);
    }

    [CLSCompliant(false)]
    public unsafe void GetFilter(uint fid, EfxFilterf pname, out float value)
    {
      fixed (float* numPtr = &value)
        this.Imported_alGetFilterf(fid, pname, numPtr);
    }

    public void GetFilter(int fid, EfxFilterf pname, out float value)
    {
      this.GetFilter((uint) fid, pname, out value);
    }

    [CLSCompliant(false)]
    public unsafe void GenAuxiliaryEffectSlots(int n, out uint slots)
    {
      fixed (uint* slots1 = &slots)
      {
        this.Imported_alGenAuxiliaryEffectSlots(n, slots1);
        slots = *slots1;
      }
    }

    public unsafe void GenAuxiliaryEffectSlots(int n, out int slots)
    {
      fixed (int* numPtr = &slots)
      {
        this.Imported_alGenAuxiliaryEffectSlots(n, (uint*) numPtr);
        slots = *numPtr;
      }
    }

    public int[] GenAuxiliaryEffectSlots(int n)
    {
      if (n <= 0)
        throw new ArgumentOutOfRangeException("n", "Must be higher than 0.");
      int[] numArray = new int[n];
      this.GenAuxiliaryEffectSlots(numArray.Length, out numArray[0]);
      return numArray;
    }

    public int GenAuxiliaryEffectSlot()
    {
      int slots;
      this.GenAuxiliaryEffectSlots(1, out slots);
      return slots;
    }

    [CLSCompliant(false)]
    public unsafe void GenAuxiliaryEffectSlot(out uint slot)
    {
      fixed (uint* slots = &slot)
      {
        this.Imported_alGenAuxiliaryEffectSlots(1, slots);
        slot = *slots;
      }
    }

    [CLSCompliant(false)]
    public unsafe void DeleteAuxiliaryEffectSlots(int n, ref uint slots)
    {
      fixed (uint* slots1 = &slots)
        this.Imported_alDeleteAuxiliaryEffectSlots(n, slots1);
    }

    public unsafe void DeleteAuxiliaryEffectSlots(int n, ref int slots)
    {
      fixed (int* numPtr = &slots)
        this.Imported_alDeleteAuxiliaryEffectSlots(n, (uint*) numPtr);
    }

    public void DeleteAuxiliaryEffectSlots(int[] slots)
    {
      if (slots == null)
        throw new ArgumentNullException("slots");
      this.DeleteAuxiliaryEffectSlots(slots.Length, ref slots[0]);
    }

    [CLSCompliant(false)]
    public void DeleteAuxiliaryEffectSlots(uint[] slots)
    {
      if (slots == null)
        throw new ArgumentNullException("slots");
      this.DeleteAuxiliaryEffectSlots(slots.Length, ref slots[0]);
    }

    public void DeleteAuxiliaryEffectSlot(int slot)
    {
      this.DeleteAuxiliaryEffectSlots(1, ref slot);
    }

    [CLSCompliant(false)]
    public unsafe void DeleteAuxiliaryEffectSlot(ref uint slot)
    {
      fixed (uint* slots = &slot)
        this.Imported_alDeleteAuxiliaryEffectSlots(1, slots);
    }

    [CLSCompliant(false)]
    public bool IsAuxiliaryEffectSlot(uint slot)
    {
      return this.Imported_alIsAuxiliaryEffectSlot(slot);
    }

    public bool IsAuxiliaryEffectSlot(int slot)
    {
      return this.Imported_alIsAuxiliaryEffectSlot((uint) slot);
    }

    [CLSCompliant(false)]
    public void AuxiliaryEffectSlot(uint asid, EfxAuxiliaryi param, int value)
    {
      this.Imported_alAuxiliaryEffectSloti(asid, param, value);
    }

    public void AuxiliaryEffectSlot(int asid, EfxAuxiliaryi param, int value)
    {
      this.Imported_alAuxiliaryEffectSloti((uint) asid, param, value);
    }

    [CLSCompliant(false)]
    public void AuxiliaryEffectSlot(uint asid, EfxAuxiliaryf param, float value)
    {
      this.Imported_alAuxiliaryEffectSlotf(asid, param, value);
    }

    public void AuxiliaryEffectSlot(int asid, EfxAuxiliaryf param, float value)
    {
      this.Imported_alAuxiliaryEffectSlotf((uint) asid, param, value);
    }

    [CLSCompliant(false)]
    public unsafe void GetAuxiliaryEffectSlot(uint asid, EfxAuxiliaryi pname, out int value)
    {
      fixed (int* numPtr = &value)
        this.Imported_alGetAuxiliaryEffectSloti(asid, pname, numPtr);
    }

    public void GetAuxiliaryEffectSlot(int asid, EfxAuxiliaryi pname, out int value)
    {
      this.GetAuxiliaryEffectSlot((uint) asid, pname, out value);
    }

    [CLSCompliant(false)]
    public unsafe void GetAuxiliaryEffectSlot(uint asid, EfxAuxiliaryf pname, out float value)
    {
      fixed (float* numPtr = &value)
        this.Imported_alGetAuxiliaryEffectSlotf(asid, pname, numPtr);
    }

    public void GetAuxiliaryEffectSlot(int asid, EfxAuxiliaryf pname, out float value)
    {
      this.GetAuxiliaryEffectSlot((uint) asid, pname, out value);
    }

    [CLSCompliant(false)]
    public static void GetEaxFromEfxEax(ref EffectsExtension.EaxReverb input, out EffectsExtension.EfxEaxReverb output)
    {
      output.AirAbsorptionGainHF = 0.995f;
      output.RoomRolloffFactor = input.RoomRolloffFactor;
      output.Density = 1f;
      output.Diffusion = 1f;
      output.DecayTime = input.DecayTime;
      output.DecayHFLimit = 1;
      output.DecayHFRatio = input.DecayHFRatio;
      output.DecayLFRatio = input.DecayLFRatio;
      output.EchoDepth = input.EchoDepth;
      output.EchoTime = input.EchoTime;
      output.Gain = 0.32f;
      output.GainHF = 0.89f;
      output.GainLF = 1f;
      output.LFReference = input.LFReference;
      output.HFReference = input.HFReference;
      output.LateReverbDelay = input.ReverbDelay;
      output.LateReverbGain = 1.26f;
      output.LateReverbPan = input.ReverbPan;
      output.ModulationDepth = input.ModulationDepth;
      output.ModulationTime = input.ModulationTime;
      output.ReflectionsDelay = input.ReflectionsDelay;
      output.ReflectionsGain = 0.05f;
      output.ReflectionsPan = input.ReflectionsPan;
    }

    private delegate void Delegate_alGenEffects(int n, [Out] uint* effects);

    private delegate void Delegate_alDeleteEffects(int n, [In] uint* effects);

    private delegate bool Delegate_alIsEffect(uint eid);

    private delegate void Delegate_alEffecti(uint eid, EfxEffecti param, int value);

    private delegate void Delegate_alEffectf(uint eid, EfxEffectf param, float value);

    private delegate void Delegate_alEffectfv(uint eid, EfxEffect3f param, [In] float* values);

    private delegate void Delegate_alGetEffecti(uint eid, EfxEffecti pname, [Out] int* value);

    private delegate void Delegate_alGetEffectf(uint eid, EfxEffectf pname, [Out] float* value);

    private delegate void Delegate_alGetEffectfv(uint eid, EfxEffect3f param, [Out] float* values);

    private delegate void Delegate_alGenFilters(int n, [Out] uint* filters);

    private delegate void Delegate_alDeleteFilters(int n, [In] uint* filters);

    private delegate bool Delegate_alIsFilter(uint fid);

    private delegate void Delegate_alFilteri(uint fid, EfxFilteri param, int value);

    private delegate void Delegate_alFilterf(uint fid, EfxFilterf param, float value);

    private delegate void Delegate_alGetFilteri(uint fid, EfxFilteri pname, [Out] int* value);

    private delegate void Delegate_alGetFilterf(uint fid, EfxFilterf pname, [Out] float* value);

    private delegate void Delegate_alGenAuxiliaryEffectSlots(int n, [Out] uint* slots);

    private delegate void Delegate_alDeleteAuxiliaryEffectSlots(int n, [In] uint* slots);

    private delegate bool Delegate_alIsAuxiliaryEffectSlot(uint slot);

    private delegate void Delegate_alAuxiliaryEffectSloti(uint asid, EfxAuxiliaryi param, int value);

    private delegate void Delegate_alAuxiliaryEffectSlotf(uint asid, EfxAuxiliaryf param, float value);

    private delegate void Delegate_alGetAuxiliaryEffectSloti(uint asid, EfxAuxiliaryi pname, [Out] int* value);

    private delegate void Delegate_alGetAuxiliaryEffectSlotf(uint asid, EfxAuxiliaryf pname, [Out] float* value);

    [CLSCompliant(false)]
    public struct EaxReverb
    {
      public uint Environment;
      public float EnvironmentSize;
      public float EnvironmentDiffusion;
      public int Room;
      public int RoomHF;
      public int RoomLF;
      public float DecayTime;
      public float DecayHFRatio;
      public float DecayLFRatio;
      public int Reflections;
      public float ReflectionsDelay;
      public Vector3 ReflectionsPan;
      public int Reverb;
      public float ReverbDelay;
      public Vector3 ReverbPan;
      public float EchoTime;
      public float EchoDepth;
      public float ModulationTime;
      public float ModulationDepth;
      public float AirAbsorptionHF;
      public float HFReference;
      public float LFReference;
      public float RoomRolloffFactor;
      public uint Flags;

      public EaxReverb(uint environment, float environmentSize, float environmentDiffusion, int room, int roomHF, int roomLF, float decayTime, float decayHFRatio, float decayLFRatio, int reflections, float reflectionsDelay, float reflectionsPanX, float reflectionsPanY, float reflectionsPanZ, int reverb, float reverbDelay, float reverbPanX, float reverbPanY, float reverbPanZ, float echoTime, float echoDepth, float modulationTime, float modulationDepth, float airAbsorptionHF, float hfReference, float lfReference, float roomRolloffFactor, uint flags)
      {
        this.Environment = environment;
        this.EnvironmentSize = environmentSize;
        this.EnvironmentDiffusion = environmentDiffusion;
        this.Room = room;
        this.RoomHF = roomHF;
        this.RoomLF = roomLF;
        this.DecayTime = decayTime;
        this.DecayHFRatio = decayHFRatio;
        this.DecayLFRatio = decayLFRatio;
        this.Reflections = reflections;
        this.ReflectionsDelay = reflectionsDelay;
        this.ReflectionsPan = new Vector3(reflectionsPanX, reflectionsPanY, reflectionsPanZ);
        this.Reverb = reverb;
        this.ReverbDelay = reverbDelay;
        this.ReverbPan = new Vector3(reverbPanX, reverbPanY, reverbPanZ);
        this.EchoTime = echoTime;
        this.EchoDepth = echoDepth;
        this.ModulationTime = modulationTime;
        this.ModulationDepth = modulationDepth;
        this.AirAbsorptionHF = airAbsorptionHF;
        this.HFReference = hfReference;
        this.LFReference = lfReference;
        this.RoomRolloffFactor = roomRolloffFactor;
        this.Flags = flags;
      }
    }

    public struct EfxEaxReverb
    {
      public float Density;
      public float Diffusion;
      public float Gain;
      public float GainHF;
      public float GainLF;
      public float DecayTime;
      public float DecayHFRatio;
      public float DecayLFRatio;
      public float ReflectionsGain;
      public float ReflectionsDelay;
      public Vector3 ReflectionsPan;
      public float LateReverbGain;
      public float LateReverbDelay;
      public Vector3 LateReverbPan;
      public float EchoTime;
      public float EchoDepth;
      public float ModulationTime;
      public float ModulationDepth;
      public float AirAbsorptionGainHF;
      public float HFReference;
      public float LFReference;
      public float RoomRolloffFactor;
      public int DecayHFLimit;
    }

    [CLSCompliant(false)]
    public static class ReverbPresets
    {
      public static EffectsExtension.EaxReverb CastleSmallRoom = new EffectsExtension.EaxReverb(26U, 8.3f, 0.89f, -1000, -800, -2000, 1.22f, 0.83f, 0.31f, -100, 0.022f, 0.0f, 0.0f, 0.0f, 600, 11.0 / 1000.0, 0.0f, 0.0f, 0.0f, 0.138f, 0.08f, 0.25f, 0.0f, -5f, 5168.6f, 139.5f, 0.0f, 32U);
      public static EffectsExtension.EaxReverb CastleShortPassage = new EffectsExtension.EaxReverb(26U, 8.3f, 0.89f, -1000, -1000, -2000, 2.32f, 0.83f, 0.31f, -100, 0.007f, 0.0f, 0.0f, 0.0f, 200, 23.0 / 1000.0, 0.0f, 0.0f, 0.0f, 0.138f, 0.08f, 0.25f, 0.0f, -5f, 5168.6f, 139.5f, 0.0f, 32U);
      public static EffectsExtension.EaxReverb CastleMediumroom = new EffectsExtension.EaxReverb(26U, 8.3f, 0.93f, -1000, -1100, -2000, 2.04f, 0.83f, 0.46f, -400, 0.022f, 0.0f, 0.0f, 0.0f, 400, 11.0 / 1000.0, 0.0f, 0.0f, 0.0f, 0.155f, 0.03f, 0.25f, 0.0f, -5f, 5168.6f, 139.5f, 0.0f, 32U);
      public static EffectsExtension.EaxReverb CastleLongpassage = new EffectsExtension.EaxReverb(26U, 8.3f, 0.89f, -1000, -800, -2000, 3.42f, 0.83f, 0.31f, -100, 0.007f, 0.0f, 0.0f, 0.0f, 300, 23.0 / 1000.0, 0.0f, 0.0f, 0.0f, 0.138f, 0.08f, 0.25f, 0.0f, -5f, 5168.6f, 139.5f, 0.0f, 32U);
      public static EffectsExtension.EaxReverb CastleLargeroom = new EffectsExtension.EaxReverb(26U, 8.3f, 0.82f, -1000, -1100, -1800, 2.53f, 0.83f, 0.5f, -700, 0.034f, 0.0f, 0.0f, 0.0f, 200, 0.016f, 0.0f, 0.0f, 0.0f, 0.185f, 0.07f, 0.25f, 0.0f, -5f, 5168.6f, 139.5f, 0.0f, 32U);
      public static EffectsExtension.EaxReverb CastleHall = new EffectsExtension.EaxReverb(26U, 8.3f, 0.81f, -1000, -1100, -1500, 3.14f, 0.79f, 0.62f, -1500, 0.056f, 0.0f, 0.0f, 0.0f, 100, 0.024f, 0.0f, 0.0f, 0.0f, 0.25f, 0.0f, 0.25f, 0.0f, -5f, 5168.6f, 139.5f, 0.0f, 32U);
      public static EffectsExtension.EaxReverb CastleCupboard = new EffectsExtension.EaxReverb(26U, 8.3f, 0.89f, -1000, -1100, -2000, 0.67f, 0.87f, 0.31f, 300, 0.01f, 0.0f, 0.0f, 0.0f, 1100, 0.007f, 0.0f, 0.0f, 0.0f, 0.138f, 0.08f, 0.25f, 0.0f, -5f, 5168.6f, 139.5f, 0.0f, 32U);
      public static EffectsExtension.EaxReverb CastleCourtyard = new EffectsExtension.EaxReverb(26U, 8.3f, 0.42f, -1000, -700, -1400, 2.13f, 0.61f, 0.23f, -1300, 0.16f, 0.0f, 0.0f, 0.0f, -300, 0.036f, 0.0f, 0.0f, 0.0f, 0.25f, 0.37f, 0.25f, 0.0f, -5f, 5000f, 250f, 0.0f, 31U);
      public static EffectsExtension.EaxReverb CastleAlcove = new EffectsExtension.EaxReverb(26U, 8.3f, 0.89f, -1000, -600, -2000, 1.64f, 0.87f, 0.31f, 0, 0.007f, 0.0f, 0.0f, 0.0f, 300, 0.034f, 0.0f, 0.0f, 0.0f, 0.138f, 0.08f, 0.25f, 0.0f, -5f, 5168.6f, 139.5f, 0.0f, 32U);
      public static EffectsExtension.EaxReverb FactoryAlcove = new EffectsExtension.EaxReverb(26U, 1.8f, 0.59f, -1200, -200, -600, 3.14f, 0.65f, 1.31f, 300, 0.01f, 0.0f, 0.0f, 0.0f, 0, 0.038f, 0.0f, 0.0f, 0.0f, 57.0 / 500.0, 0.1f, 0.25f, 0.0f, -5f, 3762.6f, 362.5f, 0.0f, 32U);
      public static EffectsExtension.EaxReverb FactoryShortpassage = new EffectsExtension.EaxReverb(26U, 1.8f, 0.64f, -1200, -200, -600, 2.53f, 0.65f, 1.31f, 0, 0.01f, 0.0f, 0.0f, 0.0f, 200, 0.038f, 0.0f, 0.0f, 0.0f, 0.135f, 0.23f, 0.25f, 0.0f, -5f, 3762.6f, 362.5f, 0.0f, 32U);
      public static EffectsExtension.EaxReverb FactoryMediumroom = new EffectsExtension.EaxReverb(26U, 1.9f, 0.82f, -1200, -200, -600, 2.76f, 0.65f, 1.31f, -1100, 0.022f, 0.0f, 0.0f, 0.0f, 300, 23.0 / 1000.0, 0.0f, 0.0f, 0.0f, 0.174f, 0.07f, 0.25f, 0.0f, -5f, 3762.6f, 362.5f, 0.0f, 32U);
      public static EffectsExtension.EaxReverb FactoryLongpassage = new EffectsExtension.EaxReverb(26U, 1.8f, 0.64f, -1200, -200, -600, 4.06f, 0.65f, 1.31f, 0, 0.02f, 0.0f, 0.0f, 0.0f, 200, 0.037f, 0.0f, 0.0f, 0.0f, 0.135f, 0.23f, 0.25f, 0.0f, -5f, 3762.6f, 362.5f, 0.0f, 32U);
      public static EffectsExtension.EaxReverb FactoryLargeroom = new EffectsExtension.EaxReverb(26U, 1.9f, 0.75f, -1200, -300, -400, 4.24f, 0.51f, 1.31f, -1500, 0.039f, 0.0f, 0.0f, 0.0f, 100, 23.0 / 1000.0, 0.0f, 0.0f, 0.0f, 0.231f, 0.07f, 0.25f, 0.0f, -5f, 3762.6f, 362.5f, 0.0f, 32U);
      public static EffectsExtension.EaxReverb FactoryHall = new EffectsExtension.EaxReverb(26U, 1.9f, 0.75f, -1000, -300, -400, 7.43f, 0.51f, 1.31f, -2400, 0.073f, 0.0f, 0.0f, 0.0f, -100, 0.027f, 0.0f, 0.0f, 0.0f, 0.25f, 0.07f, 0.25f, 0.0f, -5f, 3762.6f, 362.5f, 0.0f, 32U);
      public static EffectsExtension.EaxReverb FactoryCupboard = new EffectsExtension.EaxReverb(26U, 1.7f, 0.63f, -1200, -200, -600, 0.49f, 0.65f, 1.31f, 200, 0.01f, 0.0f, 0.0f, 0.0f, 600, 0.032f, 0.0f, 0.0f, 0.0f, 0.107f, 0.07f, 0.25f, 0.0f, -5f, 3762.6f, 362.5f, 0.0f, 32U);
      public static EffectsExtension.EaxReverb FactoryCourtyard = new EffectsExtension.EaxReverb(26U, 1.7f, 0.57f, -1000, -1000, -400, 2.32f, 0.29f, 0.56f, -1300, 0.14f, 0.0f, 0.0f, 0.0f, -800, 0.039f, 0.0f, 0.0f, 0.0f, 0.25f, 0.29f, 0.25f, 0.0f, -5f, 3762.6f, 362.5f, 0.0f, 32U);
      public static EffectsExtension.EaxReverb FactorySmallroom = new EffectsExtension.EaxReverb(26U, 1.8f, 0.82f, -1000, -200, -600, 1.72f, 0.65f, 1.31f, -300, 0.01f, 0.0f, 0.0f, 0.0f, 500, 0.024f, 0.0f, 0.0f, 0.0f, 0.119f, 0.07f, 0.25f, 0.0f, -5f, 3762.6f, 362.5f, 0.0f, 32U);
      public static EffectsExtension.EaxReverb IcepalaceAlcove = new EffectsExtension.EaxReverb(26U, 2.7f, 0.84f, -1000, -500, -1100, 2.76f, 1.46f, 0.28f, 100, 0.01f, 0.0f, 0.0f, 0.0f, -100, 0.03f, 0.0f, 0.0f, 0.0f, 0.161f, 0.09f, 0.25f, 0.0f, -5f, 12428.5f, 99.6f, 0.0f, 32U);
      public static EffectsExtension.EaxReverb IcepalaceShortpassage = new EffectsExtension.EaxReverb(26U, 2.7f, 0.75f, -1000, -500, -1100, 1.79f, 1.46f, 0.28f, -600, 0.01f, 0.0f, 0.0f, 0.0f, 100, 0.019f, 0.0f, 0.0f, 0.0f, 0.177f, 0.09f, 0.25f, 0.0f, -5f, 12428.5f, 99.6f, 0.0f, 32U);
      public static EffectsExtension.EaxReverb IcepalaceMediumroom = new EffectsExtension.EaxReverb(26U, 2.7f, 0.87f, -1000, -500, -700, 2.22f, 1.53f, 0.32f, -800, 0.039f, 0.0f, 0.0f, 0.0f, 100, 0.027f, 0.0f, 0.0f, 0.0f, 0.186f, 0.12f, 0.25f, 0.0f, -5f, 12428.5f, 99.6f, 0.0f, 32U);
      public static EffectsExtension.EaxReverb IcepalaceLongpassage = new EffectsExtension.EaxReverb(26U, 2.7f, 0.77f, -1000, -500, -800, 3.01f, 1.46f, 0.28f, -200, 0.012f, 0.0f, 0.0f, 0.0f, 200, 0.025f, 0.0f, 0.0f, 0.0f, 0.186f, 0.04f, 0.25f, 0.0f, -5f, 12428.5f, 99.6f, 0.0f, 32U);
      public static EffectsExtension.EaxReverb IcepalaceLargeroom = new EffectsExtension.EaxReverb(26U, 2.9f, 0.81f, -1000, -500, -700, 3.14f, 1.53f, 0.32f, -1200, 0.039f, 0.0f, 0.0f, 0.0f, 0, 0.027f, 0.0f, 0.0f, 0.0f, 0.214f, 0.11f, 0.25f, 0.0f, -5f, 12428.5f, 99.6f, 0.0f, 32U);
      public static EffectsExtension.EaxReverb IcepalaceHall = new EffectsExtension.EaxReverb(26U, 2.9f, 0.76f, -1000, -700, -500, 5.49f, 1.53f, 0.38f, -1900, 0.054f, 0.0f, 0.0f, 0.0f, -400, 0.052f, 0.0f, 0.0f, 0.0f, 0.226f, 0.11f, 0.25f, 0.0f, -5f, 12428.5f, 99.6f, 0.0f, 32U);
      public static EffectsExtension.EaxReverb IcepalaceCupboard = new EffectsExtension.EaxReverb(26U, 2.7f, 0.83f, -1000, -600, -1300, 0.76f, 1.53f, 0.26f, 100, 0.012f, 0.0f, 0.0f, 0.0f, 600, 0.016f, 0.0f, 0.0f, 0.0f, 0.143f, 0.08f, 0.25f, 0.0f, -5f, 12428.5f, 99.6f, 0.0f, 32U);
      public static EffectsExtension.EaxReverb IcepalaceCourtyard = new EffectsExtension.EaxReverb(26U, 2.9f, 0.59f, -1000, -1100, -1000, 2.04f, 1.2f, 0.38f, -1000, 0.173f, 0.0f, 0.0f, 0.0f, -1000, 0.043f, 0.0f, 0.0f, 0.0f, 0.235f, 0.48f, 0.25f, 0.0f, -5f, 12428.5f, 99.6f, 0.0f, 32U);
      public static EffectsExtension.EaxReverb IcepalaceSmallroom = new EffectsExtension.EaxReverb(26U, 2.7f, 0.84f, -1000, -500, -1100, 1.51f, 1.53f, 0.27f, -100, 0.01f, 0.0f, 0.0f, 0.0f, 300, 11.0 / 1000.0, 0.0f, 0.0f, 0.0f, 0.164f, 0.14f, 0.25f, 0.0f, -5f, 12428.5f, 99.6f, 0.0f, 32U);
      public static EffectsExtension.EaxReverb SpacestationAlcove = new EffectsExtension.EaxReverb(26U, 1.5f, 0.78f, -1000, -300, -100, 1.16f, 0.81f, 0.55f, 300, 0.007f, 0.0f, 0.0f, 0.0f, 0, 0.018f, 0.0f, 0.0f, 0.0f, 0.192f, 0.21f, 0.25f, 0.0f, -5f, 3316.1f, 458.2f, 0.0f, 32U);
      public static EffectsExtension.EaxReverb SpacestationMediumroom = new EffectsExtension.EaxReverb(26U, 1.5f, 0.75f, -1000, -400, -100, 3.01f, 0.5f, 0.55f, -800, 0.034f, 0.0f, 0.0f, 0.0f, 100, 0.035f, 0.0f, 0.0f, 0.0f, 0.209f, 0.31f, 0.25f, 0.0f, -5f, 3316.1f, 458.2f, 0.0f, 32U);
      public static EffectsExtension.EaxReverb SpacestationShortpassage = new EffectsExtension.EaxReverb(26U, 1.5f, 0.87f, -1000, -400, -100, 3.57f, 0.5f, 0.55f, 0, 0.012f, 0.0f, 0.0f, 0.0f, 100, 0.016f, 0.0f, 0.0f, 0.0f, 0.172f, 0.2f, 0.25f, 0.0f, -5f, 3316.1f, 458.2f, 0.0f, 32U);
      public static EffectsExtension.EaxReverb SpacestationLongpassage = new EffectsExtension.EaxReverb(26U, 1.9f, 0.82f, -1000, -400, -100, 4.62f, 0.62f, 0.55f, 0, 0.012f, 0.0f, 0.0f, 0.0f, 200, 0.031f, 0.0f, 0.0f, 0.0f, 0.25f, 0.23f, 0.25f, 0.0f, -5f, 3316.1f, 458.2f, 0.0f, 32U);
      public static EffectsExtension.EaxReverb SpacestationLargeroom = new EffectsExtension.EaxReverb(26U, 1.8f, 0.81f, -1000, -400, -100, 3.89f, 0.38f, 0.61f, -1000, 0.056f, 0.0f, 0.0f, 0.0f, -100, 0.035f, 0.0f, 0.0f, 0.0f, 0.233f, 0.28f, 0.25f, 0.0f, -5f, 3316.1f, 458.2f, 0.0f, 32U);
      public static EffectsExtension.EaxReverb SpacestationHall = new EffectsExtension.EaxReverb(26U, 1.9f, 0.87f, -1000, -400, -100, 7.11f, 0.38f, 0.61f, -1500, 0.1f, 0.0f, 0.0f, 0.0f, -400, 0.047f, 0.0f, 0.0f, 0.0f, 0.25f, 0.25f, 0.25f, 0.0f, -5f, 3316.1f, 458.2f, 0.0f, 32U);
      public static EffectsExtension.EaxReverb SpacestationCupboard = new EffectsExtension.EaxReverb(26U, 1.4f, 0.56f, -1000, -300, -100, 0.79f, 0.81f, 0.55f, 300, 0.007f, 0.0f, 0.0f, 0.0f, 500, 0.018f, 0.0f, 0.0f, 0.0f, 0.181f, 0.31f, 0.25f, 0.0f, -5f, 3316.1f, 458.2f, 0.0f, 32U);
      public static EffectsExtension.EaxReverb SpacestationSmallroom = new EffectsExtension.EaxReverb(26U, 1.5f, 0.7f, -1000, -300, -100, 1.72f, 0.82f, 0.55f, -200, 0.007f, 0.0f, 0.0f, 0.0f, 300, 0.013f, 0.0f, 0.0f, 0.0f, 0.188f, 0.26f, 0.25f, 0.0f, -5f, 3316.1f, 458.2f, 0.0f, 32U);
      public static EffectsExtension.EaxReverb WoodenAlcove = new EffectsExtension.EaxReverb(26U, 7.5f, 1f, -1000, -1800, -1000, 1.22f, 0.62f, 0.91f, 100, 0.012f, 0.0f, 0.0f, 0.0f, -300, 0.024f, 0.0f, 0.0f, 0.0f, 0.25f, 0.0f, 0.25f, 0.0f, -5f, 4705f, 99.6f, 0.0f, 63U);
      public static EffectsExtension.EaxReverb WoodenShortpassage = new EffectsExtension.EaxReverb(26U, 7.5f, 1f, -1000, -1800, -1000, 1.75f, 0.5f, 0.87f, -100, 0.012f, 0.0f, 0.0f, 0.0f, -400, 0.024f, 0.0f, 0.0f, 0.0f, 0.25f, 0.0f, 0.25f, 0.0f, -5f, 4705f, 99.6f, 0.0f, 63U);
      public static EffectsExtension.EaxReverb WoodenMediumroom = new EffectsExtension.EaxReverb(26U, 7.5f, 1f, -1000, -2000, -1100, 1.47f, 0.42f, 0.82f, -100, 0.049f, 0.0f, 0.0f, 0.0f, -100, 0.029f, 0.0f, 0.0f, 0.0f, 0.25f, 0.0f, 0.25f, 0.0f, -5f, 4705f, 99.6f, 0.0f, 63U);
      public static EffectsExtension.EaxReverb WoodenLongpassage = new EffectsExtension.EaxReverb(26U, 7.5f, 1f, -1000, -2000, -1000, 1.99f, 0.4f, 0.79f, 0, 0.02f, 0.0f, 0.0f, 0.0f, -700, 0.036f, 0.0f, 0.0f, 0.0f, 0.25f, 0.0f, 0.25f, 0.0f, -5f, 4705f, 99.6f, 0.0f, 63U);
      public static EffectsExtension.EaxReverb WoodenLargeroom = new EffectsExtension.EaxReverb(26U, 7.5f, 1f, -1000, -2100, -1100, 2.65f, 0.33f, 0.82f, -100, 0.066f, 0.0f, 0.0f, 0.0f, -200, 0.049f, 0.0f, 0.0f, 0.0f, 0.25f, 0.0f, 0.25f, 0.0f, -5f, 4705f, 99.6f, 0.0f, 63U);
      public static EffectsExtension.EaxReverb WoodenHall = new EffectsExtension.EaxReverb(26U, 7.5f, 1f, -1000, -2200, -1100, 3.45f, 0.3f, 0.82f, -100, 0.088f, 0.0f, 0.0f, 0.0f, -200, 0.063f, 0.0f, 0.0f, 0.0f, 0.25f, 0.0f, 0.25f, 0.0f, -5f, 4705f, 99.6f, 0.0f, 63U);
      public static EffectsExtension.EaxReverb WoodenCupboard = new EffectsExtension.EaxReverb(26U, 7.5f, 1f, -1000, -1700, -1000, 0.56f, 0.46f, 0.91f, 100, 0.012f, 0.0f, 0.0f, 0.0f, 100, 0.028f, 0.0f, 0.0f, 0.0f, 0.25f, 0.0f, 0.25f, 0.0f, -5f, 4705f, 99.6f, 0.0f, 63U);
      public static EffectsExtension.EaxReverb WoodenSmallroom = new EffectsExtension.EaxReverb(26U, 7.5f, 1f, -1000, -1900, -1000, 0.79f, 0.32f, 0.87f, 0, 0.032f, 0.0f, 0.0f, 0.0f, -100, 0.029f, 0.0f, 0.0f, 0.0f, 0.25f, 0.0f, 0.25f, 0.0f, -5f, 4705f, 99.6f, 0.0f, 63U);
      public static EffectsExtension.EaxReverb WoodenCourtyard = new EffectsExtension.EaxReverb(26U, 7.5f, 0.65f, -1000, -2200, -1000, 1.79f, 0.35f, 0.79f, -500, 0.123f, 0.0f, 0.0f, 0.0f, -2000, 0.032f, 0.0f, 0.0f, 0.0f, 0.25f, 0.0f, 0.25f, 0.0f, -5f, 4705f, 99.6f, 0.0f, 63U);
      public static EffectsExtension.EaxReverb SportEmptystadium = new EffectsExtension.EaxReverb(26U, 7.2f, 1f, -1000, -700, -200, 6.26f, 0.51f, 1.1f, -2400, 0.183f, 0.0f, 0.0f, 0.0f, -800, 0.038f, 0.0f, 0.0f, 0.0f, 0.25f, 0.0f, 0.25f, 0.0f, -5f, 5000f, 250f, 0.0f, 32U);
      public static EffectsExtension.EaxReverb SportSquashcourt = new EffectsExtension.EaxReverb(26U, 7.5f, 0.75f, -1000, -1000, -200, 2.22f, 0.91f, 1.16f, -700, 0.007f, 0.0f, 0.0f, 0.0f, -200, 11.0 / 1000.0, 0.0f, 0.0f, 0.0f, 0.126f, 0.19f, 0.25f, 0.0f, -5f, 7176.9f, 211.2f, 0.0f, 32U);
      public static EffectsExtension.EaxReverb SportSmallswimmingpool = new EffectsExtension.EaxReverb(26U, 36.2f, 0.7f, -1000, -200, -100, 2.76f, 1.25f, 1.14f, -400, 0.02f, 0.0f, 0.0f, 0.0f, -200, 0.03f, 0.0f, 0.0f, 0.0f, 0.179f, 0.15f, 0.895f, 0.19f, -5f, 5000f, 250f, 0.0f, 0U);
      public static EffectsExtension.EaxReverb SportLargeswimmingpool = new EffectsExtension.EaxReverb(26U, 36.2f, 0.82f, -1000, -200, 0, 5.49f, 1.31f, 1.14f, -700, 0.039f, 0.0f, 0.0f, 0.0f, -600, 0.049f, 0.0f, 0.0f, 0.0f, 0.222f, 0.55f, 1.159f, 0.21f, -5f, 5000f, 250f, 0.0f, 0U);
      public static EffectsExtension.EaxReverb SportGymnasium = new EffectsExtension.EaxReverb(26U, 7.5f, 0.81f, -1000, -700, -100, 3.14f, 1.06f, 1.35f, -800, 0.029f, 0.0f, 0.0f, 0.0f, -500, 0.045f, 0.0f, 0.0f, 0.0f, 0.146f, 0.14f, 0.25f, 0.0f, -5f, 7176.9f, 211.2f, 0.0f, 32U);
      public static EffectsExtension.EaxReverb SportFullstadium = new EffectsExtension.EaxReverb(26U, 7.2f, 1f, -1000, -2300, -200, 5.25f, 0.17f, 0.8f, -2000, 0.188f, 0.0f, 0.0f, 0.0f, -1100, 0.038f, 0.0f, 0.0f, 0.0f, 0.25f, 0.0f, 0.25f, 0.0f, -5f, 5000f, 250f, 0.0f, 32U);
      public static EffectsExtension.EaxReverb SportStadimtannoy = new EffectsExtension.EaxReverb(26U, 3f, 0.78f, -1000, -500, -600, 2.53f, 0.88f, 0.68f, -1100, 0.23f, 0.0f, 0.0f, 0.0f, -600, 0.063f, 0.0f, 0.0f, 0.0f, 0.25f, 0.2f, 0.25f, 0.0f, -5f, 5000f, 250f, 0.0f, 32U);
      public static EffectsExtension.EaxReverb PrefabWorkshop = new EffectsExtension.EaxReverb(26U, 1.9f, 1f, -1000, -1700, -800, 0.76f, 1f, 1f, 0, 0.012f, 0.0f, 0.0f, 0.0f, 100, 0.012f, 0.0f, 0.0f, 0.0f, 0.25f, 0.0f, 0.25f, 0.0f, -5f, 5000f, 250f, 0.0f, 0U);
      public static EffectsExtension.EaxReverb PrefabSchoolroom = new EffectsExtension.EaxReverb(26U, 1.86f, 0.69f, -1000, -400, -600, 0.98f, 0.45f, 0.18f, 300, 0.017f, 0.0f, 0.0f, 0.0f, 300, 0.015f, 0.0f, 0.0f, 0.0f, 0.095f, 0.14f, 0.25f, 0.0f, -5f, 7176.9f, 211.2f, 0.0f, 32U);
      public static EffectsExtension.EaxReverb PrefabPractiseroom = new EffectsExtension.EaxReverb(26U, 1.86f, 0.87f, -1000, -800, -600, 1.12f, 0.56f, 0.18f, 200, 0.01f, 0.0f, 0.0f, 0.0f, 300, 11.0 / 1000.0, 0.0f, 0.0f, 0.0f, 0.095f, 0.14f, 0.25f, 0.0f, -5f, 7176.9f, 211.2f, 0.0f, 32U);
      public static EffectsExtension.EaxReverb PrefabOuthouse = new EffectsExtension.EaxReverb(26U, 80.3f, 0.82f, -1000, -1900, -1600, 1.38f, 0.38f, 0.35f, -100, 0.024f, 0.0f, 0.0f, -0.0f, -400, 0.044f, 0.0f, 0.0f, 0.0f, 0.121f, 0.17f, 0.25f, 0.0f, -5f, 2854.4f, 107.5f, 0.0f, 0U);
      public static EffectsExtension.EaxReverb PrefabCaravan = new EffectsExtension.EaxReverb(26U, 8.3f, 1f, -1000, -2100, -1800, 0.43f, 1.5f, 1f, 0, 0.012f, 0.0f, 0.0f, 0.0f, 600, 0.012f, 0.0f, 0.0f, 0.0f, 0.25f, 0.0f, 0.25f, 0.0f, -5f, 5000f, 250f, 0.0f, 31U);
      public static EffectsExtension.EaxReverb DomeTomb = new EffectsExtension.EaxReverb(26U, 51.8f, 0.79f, -1000, -900, -1300, 4.18f, 0.21f, 0.1f, -825, 0.03f, 0.0f, 0.0f, 0.0f, 450, 0.022f, 0.0f, 0.0f, 0.0f, 0.177f, 0.19f, 0.25f, 0.0f, -5f, 2854.4f, 20f, 0.0f, 0U);
      public static EffectsExtension.EaxReverb DomeSaintPauls = new EffectsExtension.EaxReverb(26U, 50.3f, 0.87f, -1000, -900, -1300, 10.48f, 0.19f, 0.1f, -1500, 0.09f, 0.0f, 0.0f, 0.0f, 200, 0.042f, 0.0f, 0.0f, 0.0f, 0.25f, 0.12f, 0.25f, 0.0f, -5f, 2854.4f, 20f, 0.0f, 63U);
      public static EffectsExtension.EaxReverb PipeSmall = new EffectsExtension.EaxReverb(26U, 50.3f, 1f, -1000, -900, -1300, 5.04f, 0.1f, 0.1f, -600, 0.032f, 0.0f, 0.0f, 0.0f, 800, 0.015f, 0.0f, 0.0f, 0.0f, 0.25f, 0.0f, 0.25f, 0.0f, -5f, 2854.4f, 20f, 0.0f, 63U);
      public static EffectsExtension.EaxReverb PipeLongthin = new EffectsExtension.EaxReverb(26U, 1.6f, 0.91f, -1000, -700, -1100, 9.21f, 0.18f, 0.1f, -300, 0.01f, 0.0f, 0.0f, 0.0f, -300, 0.022f, 0.0f, 0.0f, 0.0f, 0.25f, 0.0f, 0.25f, 0.0f, -5f, 2854.4f, 20f, 0.0f, 0U);
      public static EffectsExtension.EaxReverb PipeLarge = new EffectsExtension.EaxReverb(26U, 50.3f, 1f, -1000, -900, -1300, 8.45f, 0.1f, 0.1f, -800, 23.0 / 500.0, 0.0f, 0.0f, 0.0f, 400, 0.032f, 0.0f, 0.0f, 0.0f, 0.25f, 0.0f, 0.25f, 0.0f, -5f, 2854.4f, 20f, 0.0f, 63U);
      public static EffectsExtension.EaxReverb PipeResonant = new EffectsExtension.EaxReverb(26U, 1.3f, 0.91f, -1000, -700, -1100, 6.81f, 0.18f, 0.1f, -300, 0.01f, 0.0f, 0.0f, 0.0f, 0, 0.022f, 0.0f, 0.0f, 0.0f, 0.25f, 0.0f, 0.25f, 0.0f, -5f, 2854.4f, 20f, 0.0f, 0U);
      public static EffectsExtension.EaxReverb OutdoorsBackyard = new EffectsExtension.EaxReverb(26U, 80.3f, 0.45f, -1000, -1200, -600, 1.12f, 0.34f, 0.46f, -700, 0.069f, 0.0f, 0.0f, -0.0f, -300, 23.0 / 1000.0, 0.0f, 0.0f, 0.0f, 0.218f, 0.34f, 0.25f, 0.0f, -5f, 4399.1f, 242.9f, 0.0f, 0U);
      public static EffectsExtension.EaxReverb OutdoorsRollingplains = new EffectsExtension.EaxReverb(26U, 80.3f, 0.0f, -1000, -3900, -400, 2.13f, 0.21f, 0.46f, -1500, 0.3f, 0.0f, 0.0f, -0.0f, -700, 0.019f, 0.0f, 0.0f, 0.0f, 0.25f, 1f, 0.25f, 0.0f, -5f, 4399.1f, 242.9f, 0.0f, 0U);
      public static EffectsExtension.EaxReverb OutdoorsDeepcanyon = new EffectsExtension.EaxReverb(26U, 80.3f, 0.74f, -1000, -1500, -400, 3.89f, 0.21f, 0.46f, -1000, 0.223f, 0.0f, 0.0f, -0.0f, -900, 0.019f, 0.0f, 0.0f, 0.0f, 0.25f, 1f, 0.25f, 0.0f, -5f, 4399.1f, 242.9f, 0.0f, 0U);
      public static EffectsExtension.EaxReverb OutdoorsCreek = new EffectsExtension.EaxReverb(26U, 80.3f, 0.35f, -1000, -1500, -600, 2.13f, 0.21f, 0.46f, -800, 0.115f, 0.0f, 0.0f, -0.0f, -1400, 0.031f, 0.0f, 0.0f, 0.0f, 0.218f, 0.34f, 0.25f, 0.0f, -5f, 4399.1f, 242.9f, 0.0f, 0U);
      public static EffectsExtension.EaxReverb OutdoorsValley = new EffectsExtension.EaxReverb(26U, 80.3f, 0.28f, -1000, -3100, -1600, 2.88f, 0.26f, 0.35f, -1700, 0.263f, 0.0f, 0.0f, -0.0f, -800, 0.1f, 0.0f, 0.0f, 0.0f, 0.25f, 0.34f, 0.25f, 0.0f, -5f, 2854.4f, 107.5f, 0.0f, 0U);
      public static EffectsExtension.EaxReverb MoodHeaven = new EffectsExtension.EaxReverb(26U, 19.6f, 0.94f, -1000, -200, -700, 5.04f, 1.12f, 0.56f, -1230, 0.02f, 0.0f, 0.0f, 0.0f, 200, 0.029f, 0.0f, 0.0f, 0.0f, 0.25f, 0.08f, 2.742f, 0.05f, -2f, 5000f, 250f, 0.0f, 63U);
      public static EffectsExtension.EaxReverb MoodHell = new EffectsExtension.EaxReverb(26U, 100f, 0.57f, -1000, -900, -700, 3.57f, 0.49f, 2f, -10000, 0.02f, 0.0f, 0.0f, 0.0f, 300, 0.03f, 0.0f, 0.0f, 0.0f, 0.11f, 0.04f, 2.109f, 0.52f, -5f, 5000f, 139.5f, 0.0f, 64U);
      public static EffectsExtension.EaxReverb MoodMemory = new EffectsExtension.EaxReverb(26U, 8f, 0.85f, -1000, -400, -900, 4.06f, 0.82f, 0.56f, -2800, 0.0f, 0.0f, 0.0f, 0.0f, 100, 0.0f, 0.0f, 0.0f, 0.0f, 0.25f, 0.0f, 0.474f, 0.45f, -10f, 5000f, 250f, 0.0f, 0U);
      public static EffectsExtension.EaxReverb DrivingCommentator = new EffectsExtension.EaxReverb(26U, 3f, 0.0f, 1000, -500, -600, 2.42f, 0.88f, 0.68f, -1400, 0.093f, 0.0f, 0.0f, 0.0f, -1200, 0.017f, 0.0f, 0.0f, 0.0f, 0.25f, 1f, 0.25f, 0.0f, -10f, 5000f, 250f, 0.0f, 32U);
      public static EffectsExtension.EaxReverb DrivingPitgarage = new EffectsExtension.EaxReverb(26U, 1.9f, 0.59f, -1000, -300, -500, 1.72f, 0.93f, 0.87f, -500, 0.0f, 0.0f, 0.0f, 0.0f, 200, 0.016f, 0.0f, 0.0f, 0.0f, 0.25f, 0.11f, 0.25f, 0.0f, -5f, 5000f, 250f, 0.0f, 0U);
      public static EffectsExtension.EaxReverb DrivingIncarRacer = new EffectsExtension.EaxReverb(26U, 1.1f, 0.8f, -1000, 0, -200, 0.17f, 2f, 0.41f, 500, 0.007f, 0.0f, 0.0f, 0.0f, -300, 0.015f, 0.0f, 0.0f, 0.0f, 0.25f, 0.0f, 0.25f, 0.0f, -5f, 10268.2f, 251f, 0.0f, 32U);
      public static EffectsExtension.EaxReverb DrivingIncarSports = new EffectsExtension.EaxReverb(26U, 1.1f, 0.8f, -1000, -400, 0, 0.17f, 0.75f, 0.41f, 0, 0.01f, 0.0f, 0.0f, 0.0f, -500, 0.0f, 0.0f, 0.0f, 0.0f, 0.25f, 0.0f, 0.25f, 0.0f, -5f, 10268.2f, 251f, 0.0f, 32U);
      public static EffectsExtension.EaxReverb DrivingIncarLuxury = new EffectsExtension.EaxReverb(26U, 1.6f, 1f, -1000, -2000, -600, 0.13f, 0.41f, 0.46f, -200, 0.01f, 0.0f, 0.0f, 0.0f, 400, 0.01f, 0.0f, 0.0f, 0.0f, 0.25f, 0.0f, 0.25f, 0.0f, -5f, 10268.2f, 251f, 0.0f, 32U);
      public static EffectsExtension.EaxReverb DrivingFullgrandstand = new EffectsExtension.EaxReverb(26U, 8.3f, 1f, -1000, -1100, -400, 3.01f, 1.37f, 1.28f, -900, 0.09f, 0.0f, 0.0f, 0.0f, -1500, 0.049f, 0.0f, 0.0f, 0.0f, 0.25f, 0.0f, 0.25f, 0.0f, -5f, 10420.2f, 250f, 0.0f, 31U);
      public static EffectsExtension.EaxReverb DrivingEmptygrandstand = new EffectsExtension.EaxReverb(26U, 8.3f, 1f, -1000, 0, -200, 4.62f, 1.75f, 1.4f, -1363, 0.09f, 0.0f, 0.0f, 0.0f, -1200, 0.049f, 0.0f, 0.0f, 0.0f, 0.25f, 0.0f, 0.25f, 0.0f, -5f, 10420.2f, 250f, 0.0f, 31U);
      public static EffectsExtension.EaxReverb DrivingTunnel = new EffectsExtension.EaxReverb(26U, 3.1f, 0.81f, -1000, -800, -100, 3.42f, 0.94f, 1.31f, -300, 0.051f, 0.0f, 0.0f, 0.0f, -300, 0.047f, 0.0f, 0.0f, 0.0f, 0.214f, 0.05f, 0.25f, 0.0f, -5f, 5000f, 155.3f, 0.0f, 32U);
      public static EffectsExtension.EaxReverb CityStreets = new EffectsExtension.EaxReverb(26U, 3f, 0.78f, -1000, -300, -100, 1.79f, 1.12f, 0.91f, -1100, 23.0 / 500.0, 0.0f, 0.0f, 0.0f, -1400, 0.028f, 0.0f, 0.0f, 0.0f, 0.25f, 0.2f, 0.25f, 0.0f, -5f, 5000f, 250f, 0.0f, 32U);
      public static EffectsExtension.EaxReverb CitySubway = new EffectsExtension.EaxReverb(26U, 3f, 0.74f, -1000, -300, -100, 3.01f, 1.23f, 0.91f, -300, 23.0 / 500.0, 0.0f, 0.0f, 0.0f, 200, 0.028f, 0.0f, 0.0f, 0.0f, 0.125f, 0.21f, 0.25f, 0.0f, -5f, 5000f, 250f, 0.0f, 32U);
      public static EffectsExtension.EaxReverb CityMuseum = new EffectsExtension.EaxReverb(26U, 80.3f, 0.82f, -1000, -1500, -1500, 3.28f, 1.4f, 0.57f, -1200, 0.039f, 0.0f, 0.0f, -0.0f, -100, 0.034f, 0.0f, 0.0f, 0.0f, 0.13f, 0.17f, 0.25f, 0.0f, -5f, 2854.4f, 107.5f, 0.0f, 0U);
      public static EffectsExtension.EaxReverb CityLibrary = new EffectsExtension.EaxReverb(26U, 80.3f, 0.82f, -1000, -1100, -2100, 2.76f, 0.89f, 0.41f, -900, 0.029f, 0.0f, 0.0f, -0.0f, -100, 0.02f, 0.0f, 0.0f, 0.0f, 0.13f, 0.17f, 0.25f, 0.0f, -5f, 2854.4f, 107.5f, 0.0f, 0U);
      public static EffectsExtension.EaxReverb CityUnderpass = new EffectsExtension.EaxReverb(26U, 3f, 0.82f, -1000, -700, -100, 3.57f, 1.12f, 0.91f, -800, 0.059f, 0.0f, 0.0f, 0.0f, -100, 0.037f, 0.0f, 0.0f, 0.0f, 0.25f, 0.14f, 0.25f, 0.0f, -7f, 5000f, 250f, 0.0f, 32U);
      public static EffectsExtension.EaxReverb CityAbandoned = new EffectsExtension.EaxReverb(26U, 3f, 0.69f, -1000, -200, -100, 3.28f, 1.17f, 0.91f, -700, 0.044f, 0.0f, 0.0f, 0.0f, -1100, 0.024f, 0.0f, 0.0f, 0.0f, 0.25f, 0.2f, 0.25f, 0.0f, -3f, 5000f, 250f, 0.0f, 32U);
      public static EffectsExtension.EaxReverb Generic = new EffectsExtension.EaxReverb(0U, 7.5f, 1f, -1000, -100, 0, 1.49f, 0.83f, 1f, -2602, 0.007f, 0.0f, 0.0f, 0.0f, 200, 11.0 / 1000.0, 0.0f, 0.0f, 0.0f, 0.25f, 0.0f, 0.25f, 0.0f, -5f, 5000f, 250f, 0.0f, 63U);
      public static EffectsExtension.EaxReverb Paddedcell = new EffectsExtension.EaxReverb(1U, 1.4f, 1f, -1000, -6000, 0, 0.17f, 0.1f, 1f, -1204, 1.0 / 1000.0, 0.0f, 0.0f, 0.0f, 207, 1.0 / 500.0, 0.0f, 0.0f, 0.0f, 0.25f, 0.0f, 0.25f, 0.0f, -5f, 5000f, 250f, 0.0f, 63U);
      public static EffectsExtension.EaxReverb Room = new EffectsExtension.EaxReverb(2U, 1.9f, 1f, -1000, -454, 0, 0.4f, 0.83f, 1f, -1646, 1.0 / 500.0, 0.0f, 0.0f, 0.0f, 53, 3.0 / 1000.0, 0.0f, 0.0f, 0.0f, 0.25f, 0.0f, 0.25f, 0.0f, -5f, 5000f, 250f, 0.0f, 63U);
      public static EffectsExtension.EaxReverb Bathroom = new EffectsExtension.EaxReverb(3U, 1.4f, 1f, -1000, -1200, 0, 1.49f, 0.54f, 1f, -370, 0.007f, 0.0f, 0.0f, 0.0f, 1030, 11.0 / 1000.0, 0.0f, 0.0f, 0.0f, 0.25f, 0.0f, 0.25f, 0.0f, -5f, 5000f, 250f, 0.0f, 63U);
      public static EffectsExtension.EaxReverb Livingroom = new EffectsExtension.EaxReverb(4U, 2.5f, 1f, -1000, -6000, 0, 0.5f, 0.1f, 1f, -1376, 3.0 / 1000.0, 0.0f, 0.0f, 0.0f, -1104, 0.004f, 0.0f, 0.0f, 0.0f, 0.25f, 0.0f, 0.25f, 0.0f, -5f, 5000f, 250f, 0.0f, 63U);
      public static EffectsExtension.EaxReverb Stoneroom = new EffectsExtension.EaxReverb(5U, 11.6f, 1f, -1000, -300, 0, 2.31f, 0.64f, 1f, -711, 0.012f, 0.0f, 0.0f, 0.0f, 83, 0.017f, 0.0f, 0.0f, 0.0f, 0.25f, 0.0f, 0.25f, 0.0f, -5f, 5000f, 250f, 0.0f, 63U);
      public static EffectsExtension.EaxReverb Auditorium = new EffectsExtension.EaxReverb(6U, 21.6f, 1f, -1000, -476, 0, 4.32f, 0.59f, 1f, -789, 0.02f, 0.0f, 0.0f, 0.0f, -289, 0.03f, 0.0f, 0.0f, 0.0f, 0.25f, 0.0f, 0.25f, 0.0f, -5f, 5000f, 250f, 0.0f, 63U);
      public static EffectsExtension.EaxReverb Concerthall = new EffectsExtension.EaxReverb(7U, 19.6f, 1f, -1000, -500, 0, 3.92f, 0.7f, 1f, -1230, 0.02f, 0.0f, 0.0f, 0.0f, -2, 0.029f, 0.0f, 0.0f, 0.0f, 0.25f, 0.0f, 0.25f, 0.0f, -5f, 5000f, 250f, 0.0f, 63U);
      public static EffectsExtension.EaxReverb Cave = new EffectsExtension.EaxReverb(8U, 14.6f, 1f, -1000, 0, 0, 2.91f, 1.3f, 1f, -602, 0.015f, 0.0f, 0.0f, 0.0f, -302, 0.022f, 0.0f, 0.0f, 0.0f, 0.25f, 0.0f, 0.25f, 0.0f, -5f, 5000f, 250f, 0.0f, 31U);
      public static EffectsExtension.EaxReverb Arena = new EffectsExtension.EaxReverb(9U, 36.2f, 1f, -1000, -698, 0, 7.24f, 0.33f, 1f, -1166, 0.02f, 0.0f, 0.0f, 0.0f, 16, 0.03f, 0.0f, 0.0f, 0.0f, 0.25f, 0.0f, 0.25f, 0.0f, -5f, 5000f, 250f, 0.0f, 63U);
      public static EffectsExtension.EaxReverb Hangar = new EffectsExtension.EaxReverb(10U, 50.3f, 1f, -1000, -1000, 0, 10.05f, 0.23f, 1f, -602, 0.02f, 0.0f, 0.0f, 0.0f, 198, 0.03f, 0.0f, 0.0f, 0.0f, 0.25f, 0.0f, 0.25f, 0.0f, -5f, 5000f, 250f, 0.0f, 63U);
      public static EffectsExtension.EaxReverb Carpettedhallway = new EffectsExtension.EaxReverb(11U, 1.9f, 1f, -1000, -4000, 0, 0.3f, 0.1f, 1f, -1831, 1.0 / 500.0, 0.0f, 0.0f, 0.0f, -1630, 0.03f, 0.0f, 0.0f, 0.0f, 0.25f, 0.0f, 0.25f, 0.0f, -5f, 5000f, 250f, 0.0f, 63U);
      public static EffectsExtension.EaxReverb Hallway = new EffectsExtension.EaxReverb(12U, 1.8f, 1f, -1000, -300, 0, 1.49f, 0.59f, 1f, -1219, 0.007f, 0.0f, 0.0f, 0.0f, 441, 11.0 / 1000.0, 0.0f, 0.0f, 0.0f, 0.25f, 0.0f, 0.25f, 0.0f, -5f, 5000f, 250f, 0.0f, 63U);
      public static EffectsExtension.EaxReverb Stonecorridor = new EffectsExtension.EaxReverb(13U, 13.5f, 1f, -1000, -237, 0, 2.7f, 0.79f, 1f, -1214, 0.013f, 0.0f, 0.0f, 0.0f, 395, 0.02f, 0.0f, 0.0f, 0.0f, 0.25f, 0.0f, 0.25f, 0.0f, -5f, 5000f, 250f, 0.0f, 63U);
      public static EffectsExtension.EaxReverb Alley = new EffectsExtension.EaxReverb(14U, 7.5f, 0.3f, -1000, -270, 0, 1.49f, 0.86f, 1f, -1204, 0.007f, 0.0f, 0.0f, 0.0f, -4, 11.0 / 1000.0, 0.0f, 0.0f, 0.0f, 0.125f, 0.95f, 0.25f, 0.0f, -5f, 5000f, 250f, 0.0f, 63U);
      public static EffectsExtension.EaxReverb Forest = new EffectsExtension.EaxReverb(15U, 38f, 0.3f, -1000, -3300, 0, 1.49f, 0.54f, 1f, -2560, 0.162f, 0.0f, 0.0f, 0.0f, -229, 0.088f, 0.0f, 0.0f, 0.0f, 0.125f, 1f, 0.25f, 0.0f, -5f, 5000f, 250f, 0.0f, 63U);
      public static EffectsExtension.EaxReverb City = new EffectsExtension.EaxReverb(16U, 7.5f, 0.5f, -1000, -800, 0, 1.49f, 0.67f, 1f, -2273, 0.007f, 0.0f, 0.0f, 0.0f, -1691, 11.0 / 1000.0, 0.0f, 0.0f, 0.0f, 0.25f, 0.0f, 0.25f, 0.0f, -5f, 5000f, 250f, 0.0f, 63U);
      public static EffectsExtension.EaxReverb Mountains = new EffectsExtension.EaxReverb(17U, 100f, 0.27f, -1000, -2500, 0, 1.49f, 0.21f, 1f, -2780, 0.3f, 0.0f, 0.0f, 0.0f, -1434, 0.1f, 0.0f, 0.0f, 0.0f, 0.25f, 1f, 0.25f, 0.0f, -5f, 5000f, 250f, 0.0f, 31U);
      public static EffectsExtension.EaxReverb Quarry = new EffectsExtension.EaxReverb(18U, 17.5f, 1f, -1000, -1000, 0, 1.49f, 0.83f, 1f, -10000, 0.061f, 0.0f, 0.0f, 0.0f, 500, 0.025f, 0.0f, 0.0f, 0.0f, 0.125f, 0.7f, 0.25f, 0.0f, -5f, 5000f, 250f, 0.0f, 63U);
      public static EffectsExtension.EaxReverb Plain = new EffectsExtension.EaxReverb(19U, 42.5f, 0.21f, -1000, -2000, 0, 1.49f, 0.5f, 1f, -2466, 0.179f, 0.0f, 0.0f, 0.0f, -1926, 0.1f, 0.0f, 0.0f, 0.0f, 0.25f, 1f, 0.25f, 0.0f, -5f, 5000f, 250f, 0.0f, 63U);
      public static EffectsExtension.EaxReverb Parkinglot = new EffectsExtension.EaxReverb(20U, 8.3f, 1f, -1000, 0, 0, 1.65f, 1.5f, 1f, -1363, 0.008f, 0.0f, 0.0f, 0.0f, -1153, 0.012f, 0.0f, 0.0f, 0.0f, 0.25f, 0.0f, 0.25f, 0.0f, -5f, 5000f, 250f, 0.0f, 31U);
      public static EffectsExtension.EaxReverb Sewerpipe = new EffectsExtension.EaxReverb(21U, 1.7f, 0.8f, -1000, -1000, 0, 2.81f, 0.14f, 1f, 429, 0.014f, 0.0f, 0.0f, 0.0f, 1023, 0.021f, 0.0f, 0.0f, 0.0f, 0.25f, 0.0f, 0.25f, 0.0f, -5f, 5000f, 250f, 0.0f, 63U);
      public static EffectsExtension.EaxReverb Underwater = new EffectsExtension.EaxReverb(22U, 1.8f, 1f, -1000, -4000, 0, 1.49f, 0.1f, 1f, -449, 0.007f, 0.0f, 0.0f, 0.0f, 1700, 11.0 / 1000.0, 0.0f, 0.0f, 0.0f, 0.25f, 0.0f, 1.18f, 0.348f, -5f, 5000f, 250f, 0.0f, 63U);
      public static EffectsExtension.EaxReverb Drugged = new EffectsExtension.EaxReverb(23U, 1.9f, 0.5f, -1000, 0, 0, 8.39f, 1.39f, 1f, -115, 1.0 / 500.0, 0.0f, 0.0f, 0.0f, 985, 0.03f, 0.0f, 0.0f, 0.0f, 0.25f, 0.0f, 0.25f, 1f, -5f, 5000f, 250f, 0.0f, 31U);
      public static EffectsExtension.EaxReverb Dizzy = new EffectsExtension.EaxReverb(24U, 1.8f, 0.6f, -1000, -400, 0, 17.23f, 0.56f, 1f, -1713, 0.02f, 0.0f, 0.0f, 0.0f, -613, 0.03f, 0.0f, 0.0f, 0.0f, 0.25f, 1f, 0.81f, 0.31f, -5f, 5000f, 250f, 0.0f, 31U);
      public static EffectsExtension.EaxReverb Psychotic = new EffectsExtension.EaxReverb(25U, 1f, 0.5f, -1000, -151, 0, 7.56f, 0.91f, 1f, -626, 0.02f, 0.0f, 0.0f, 0.0f, 774, 0.03f, 0.0f, 0.0f, 0.0f, 0.25f, 0.0f, 4f, 1f, -5f, 5000f, 250f, 0.0f, 31U);
      public static EffectsExtension.EaxReverb Dustyroom = new EffectsExtension.EaxReverb(26U, 1.8f, 0.56f, -1000, -200, -300, 1.79f, 0.38f, 0.21f, -600, 1.0 / 500.0, 0.0f, 0.0f, 0.0f, 200, 3.0 / 500.0, 0.0f, 0.0f, 0.0f, 0.202f, 0.05f, 0.25f, 0.0f, -10f, 13046f, 163.3f, 0.0f, 32U);
      public static EffectsExtension.EaxReverb Chapel = new EffectsExtension.EaxReverb(26U, 19.6f, 0.84f, -1000, -500, 0, 4.62f, 0.64f, 1.23f, -700, 0.032f, 0.0f, 0.0f, 0.0f, -200, 0.049f, 0.0f, 0.0f, 0.0f, 0.25f, 0.0f, 0.25f, 0.11f, -5f, 5000f, 250f, 0.0f, 63U);
      public static EffectsExtension.EaxReverb Smallwaterroom = new EffectsExtension.EaxReverb(26U, 36.2f, 0.7f, -1000, -698, 0, 1.51f, 1.25f, 1.14f, -100, 0.02f, 0.0f, 0.0f, 0.0f, 300, 0.03f, 0.0f, 0.0f, 0.0f, 0.179f, 0.15f, 0.895f, 0.19f, -7f, 5000f, 250f, 0.0f, 0U);

      static ReverbPresets()
      {
      }
    }
  }
}
