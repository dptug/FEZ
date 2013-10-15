// Type: CommunityExpressNS.InGamePurchase
// Assembly: CommunityExpress, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3B3F745C-AA2A-4DDF-AA8A-F5898AF84B8D
// Assembly location: F:\Program Files (x86)\FEZ\CommunityExpress.dll

using System.Collections.Generic;

namespace CommunityExpressNS
{
  public class InGamePurchase
  {
    private bool _useTestMode;
    private string _webAPIKey;
    private ulong _orderID;
    private List<InGamePurchase.Item> _itemList;
    private OnInGamePurchaseComplete _inGamePurchaseCompleteCallback;

    internal bool UseTestMode
    {
      get
      {
        return this._useTestMode;
      }
    }

    internal string WebAPIKey
    {
      get
      {
        return this._webAPIKey;
      }
    }

    public ulong OrderID
    {
      get
      {
        return this._orderID;
      }
      set
      {
        this._orderID = value;
      }
    }

    public IList<InGamePurchase.Item> ItemList
    {
      get
      {
        return (IList<InGamePurchase.Item>) this._itemList;
      }
    }

    internal OnInGamePurchaseComplete InGamePurchaseCompleteCallback
    {
      get
      {
        return this._inGamePurchaseCompleteCallback;
      }
      set
      {
        this._inGamePurchaseCompleteCallback = value;
      }
    }

    internal InGamePurchase(bool useTestMode, string webAPIKey, ulong orderID)
    {
      this._useTestMode = useTestMode;
      this._webAPIKey = webAPIKey;
      this._orderID = orderID;
      this._itemList = new List<InGamePurchase.Item>();
    }

    public void AddItem(InGamePurchase.Item item)
    {
      this._itemList.Add(item);
    }

    public void AddItem(int id, int quantity, double amountPerItem, string description, string category = "")
    {
      this._itemList.Add(new InGamePurchase.Item(id, quantity, amountPerItem, description, category));
    }

    public class Item
    {
      public int ID;
      public int Quantity;
      public double AmountPerItem;
      public string Description;
      public string Category;

      internal int Amount
      {
        get
        {
          return (int) ((double) this.Quantity * this.AmountPerItem * 100.0);
        }
      }

      public Item(int id, int quantity, double amountPerItem, string description, string category = "")
      {
        this.ID = id;
        this.Quantity = quantity;
        this.AmountPerItem = amountPerItem;
        this.Description = description;
        this.Category = category;
      }
    }
  }
}
