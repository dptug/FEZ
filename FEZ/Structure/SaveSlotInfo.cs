// Type: FezGame.Structure.SaveSlotInfo
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using System;

namespace FezGame.Structure
{
  internal class SaveSlotInfo
  {
    public TimeSpan PlayTime;
    public float Percentage;
    public int Index;
    public bool Empty;
    public SaveData SaveData;
  }
}
