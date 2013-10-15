// Type: Microsoft.Xna.Framework.GamerServices.Achievement
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

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
