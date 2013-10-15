// Type: FezGame.Services.Scripting.NpcService
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine.Components.Scripting;
using FezEngine.Services;
using FezEngine.Services.Scripting;
using FezEngine.Structure;
using FezEngine.Tools;
using FezGame.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;

namespace FezGame.Services.Scripting
{
  internal class NpcService : INpcService, IScriptingBase
  {
    [ServiceDependency]
    public ILevelManager LevelManager { private get; set; }

    [ServiceDependency]
    public IContentManagerProvider CMProvider { private get; set; }

    public LongRunningAction Say(int id, string line, string customSound, string customAnimation)
    {
      SpeechLine speechLine = new SpeechLine()
      {
        Text = line
      };
      NpcInstance npc = this.LevelManager.NonPlayerCharacters[id];
      if (!string.IsNullOrEmpty(customSound))
      {
        if (speechLine.OverrideContent == null)
          speechLine.OverrideContent = new NpcActionContent();
        speechLine.OverrideContent.Sound = this.LoadSound(customSound);
      }
      if (!string.IsNullOrEmpty(customAnimation))
      {
        if (speechLine.OverrideContent == null)
          speechLine.OverrideContent = new NpcActionContent();
        speechLine.OverrideContent.Animation = this.LoadAnimation(npc, customAnimation);
      }
      npc.CustomSpeechLine = speechLine;
      return new LongRunningAction((Func<float, float, bool>) ((_, __) => npc.CustomSpeechLine == null));
    }

    public void CarryGeezerLetter(int id)
    {
      ServiceHelper.AddComponent((IGameComponent) new GeezerLetterSender(ServiceHelper.Game, id));
    }

    private AnimatedTexture LoadAnimation(NpcInstance npc, string name)
    {
      return this.CMProvider.CurrentLevel.Load<AnimatedTexture>("Character Animations/" + npc.Name + "/" + name);
    }

    private SoundEffect LoadSound(string name)
    {
      return this.CMProvider.CurrentLevel.Load<SoundEffect>("Sounds/Npc/" + name);
    }

    public void ResetEvents()
    {
    }
  }
}
