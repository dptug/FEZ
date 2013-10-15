// Type: FezEngine.Structure.Scripting.ScriptTrigger
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

namespace FezEngine.Structure.Scripting
{
  public class ScriptTrigger : ScriptPart
  {
    public string Event { get; set; }

    public override string ToString()
    {
      return (this.Object == null ? "(none)" : this.Object.ToString()) + "." + (this.Event ?? "(none)");
    }

    public ScriptTrigger Clone()
    {
      ScriptTrigger scriptTrigger = new ScriptTrigger();
      scriptTrigger.Event = this.Event;
      scriptTrigger.Object = this.Object.Clone();
      return scriptTrigger;
    }
  }
}
