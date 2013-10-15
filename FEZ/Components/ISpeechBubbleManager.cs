// Type: FezGame.Components.ISpeechBubbleManager
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using Microsoft.Xna.Framework;

namespace FezGame.Components
{
  public interface ISpeechBubbleManager
  {
    Vector3 Origin { set; }

    SpeechFont Font { set; }

    bool Hidden { get; }

    void ChangeText(string toText);

    void Hide();

    void ForceDrawOrder(int drawOrder);

    void RevertDrawOrder();
  }
}
