// Type: FezEngine.Structure.SpeechLine
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using ContentSerialization.Attributes;

namespace FezEngine.Structure
{
  public class SpeechLine
  {
    public string Text { get; set; }

    [Serialization(Optional = true)]
    public NpcActionContent OverrideContent { get; set; }

    public SpeechLine Clone()
    {
      return new SpeechLine()
      {
        Text = this.Text,
        OverrideContent = this.OverrideContent == null ? (NpcActionContent) null : this.OverrideContent.Clone()
      };
    }
  }
}
