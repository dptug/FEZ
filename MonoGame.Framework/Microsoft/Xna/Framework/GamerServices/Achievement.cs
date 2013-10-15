// Type: Microsoft.Xna.Framework.GamerServices.Achievement
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

using System;
using System.IO;

namespace Microsoft.Xna.Framework.GamerServices
{
  public class Achievement
  {
    public string Description { get; internal set; }

    public bool DisplayBeforeEarned { get; internal set; }

    public DateTime EarnedDateTime { get; internal set; }

    public bool EarnedOnline { get; internal set; }

    public int GamerScore { get; internal set; }

    public string HowToEarn { get; internal set; }

    public bool IsEarned { get; internal set; }

    public string Key { get; internal set; }

    public string Name { get; internal set; }

    public Stream GetPicture()
    {
      throw new NotImplementedException();
    }
  }
}
