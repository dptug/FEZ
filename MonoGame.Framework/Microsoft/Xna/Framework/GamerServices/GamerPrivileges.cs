// Type: Microsoft.Xna.Framework.GamerServices.GamerPrivileges
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

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
