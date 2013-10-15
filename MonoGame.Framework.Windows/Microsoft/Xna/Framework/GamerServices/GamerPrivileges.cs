// Type: Microsoft.Xna.Framework.GamerServices.GamerPrivileges
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

namespace Microsoft.Xna.Framework.GamerServices
{
  public class GamerPrivileges
  {
    public GamerPrivilegeSetting AllowCommunication
    {
      get
      {
        return GamerPrivilegeSetting.Everyone;
      }
    }

    public bool AllowOnlineSessions
    {
      get
      {
        return true;
      }
    }

    public GamerPrivilegeSetting AllowProfileViewing
    {
      get
      {
        return GamerPrivilegeSetting.Blocked;
      }
    }

    public bool AllowPurchaseContent
    {
      get
      {
        return false;
      }
    }

    public bool AllowTradeContent
    {
      get
      {
        return false;
      }
    }

    public GamerPrivilegeSetting AllowUserCreatedContent
    {
      get
      {
        return GamerPrivilegeSetting.Blocked;
      }
    }
  }
}
