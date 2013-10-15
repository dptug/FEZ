// Type: CommunityExpressNS.InGamePurchasing
// Assembly: CommunityExpress, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3B3F745C-AA2A-4DDF-AA8A-F5898AF84B8D
// Assembly location: F:\Program Files (x86)\FEZ\CommunityExpress.dll

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;

namespace CommunityExpressNS
{
  public class InGamePurchasing
  {
    private string _language = "en";
    private List<InGamePurchase> _outstandingPurchases = new List<InGamePurchase>();
    private IntPtr _user;
    private IntPtr _apps;
    private SteamID _steamID;
    private RegionInfo _regionInfo;
    private OnTransactionAuthorizationReceivedFromSteam _internalOnTransactionAuthorizationReceived;

    public RegionInfo RegionInfo
    {
      get
      {
        return this._regionInfo;
      }
    }

    internal InGamePurchasing()
    {
      this._user = InGamePurchasing.SteamUnityAPI_SteamUser();
      this._apps = InGamePurchasing.SteamUnityAPI_SteamApps();
      if (this._internalOnTransactionAuthorizationReceived != null)
        return;
      this._internalOnTransactionAuthorizationReceived = new OnTransactionAuthorizationReceivedFromSteam(this.OnTransactionAuthorizationReceived);
      InGamePurchasing.SteamUnityAPI_SetTransactionAuthorizationCallback(Marshal.GetFunctionPointerForDelegate((Delegate) this._internalOnTransactionAuthorizationReceived));
    }

    [DllImport("CommunityExpressSW")]
    private static bool SteamUnityAPI_SetTransactionAuthorizationCallback(IntPtr OnTransactionAuthorizationReceivedCallback);

    [DllImport("CommunityExpressSW")]
    private static IntPtr SteamUnityAPI_SteamUser();

    [DllImport("CommunityExpressSW")]
    private static ulong SteamUnityAPI_SteamUser_GetSteamID(IntPtr user);

    [DllImport("CommunityExpressSW")]
    private static IntPtr SteamUnityAPI_SteamApps();

    [DllImport("CommunityExpressSW")]
    [return: MarshalAs(UnmanagedType.LPStr)]
    private static string SteamUnityAPI_SteamApps_GetCurrentGameLanguage(IntPtr apps);

    [DllImport("CommunityExpressSW")]
    private static uint SteamUnityAPI_SteamUtils_GetAppID();

    public InGamePurchase NewPurchase(bool useTestMode, string webAPIKey, ulong orderID)
    {
      if (this._regionInfo == null)
      {
        this._steamID = new SteamID(InGamePurchasing.SteamUnityAPI_SteamUser_GetSteamID(this._user));
        string currentGameLanguage = InGamePurchasing.SteamUnityAPI_SteamApps_GetCurrentGameLanguage(this._apps);
        foreach (CultureInfo cultureInfo in CultureInfo.GetCultures(CultureTypes.AllCultures))
        {
          if (cultureInfo.DisplayName.Equals(currentGameLanguage, StringComparison.OrdinalIgnoreCase) || cultureInfo.EnglishName.Equals(currentGameLanguage, StringComparison.OrdinalIgnoreCase) || cultureInfo.NativeName.Equals(currentGameLanguage, StringComparison.OrdinalIgnoreCase))
          {
            this._language = cultureInfo.TwoLetterISOLanguageName;
            break;
          }
        }
        this.FetchRegionInfo(useTestMode, webAPIKey);
      }
      return new InGamePurchase(useTestMode, webAPIKey, orderID);
    }

    public bool StartPurchase(InGamePurchase purchase, OnInGamePurchaseComplete callback)
    {
      if (this._regionInfo == null)
        return false;
      purchase.InGamePurchaseCompleteCallback = callback;
      SteamWebAPIRequest steamWebApiRequest = CommunityExpress.Instance.SteamWebAPI.NewRequest(this.GetWebInterface(purchase.UseTestMode), "InitTxn", "v0002");
      steamWebApiRequest.AddPostValue("key", purchase.WebAPIKey);
      steamWebApiRequest.AddPostValue("orderid", purchase.OrderID.ToString());
      steamWebApiRequest.AddPostValue("steamid", this._steamID.ToUInt64().ToString());
      steamWebApiRequest.AddPostValue("appid", InGamePurchasing.SteamUnityAPI_SteamUtils_GetAppID().ToString());
      steamWebApiRequest.AddPostValue("itemcount", purchase.ItemList.Count.ToString());
      steamWebApiRequest.AddPostValue("language", this._language);
      steamWebApiRequest.AddPostValue("currency", this._regionInfo.ISOCurrencySymbol);
      int num = 0;
      foreach (InGamePurchase.Item obj in (IEnumerable<InGamePurchase.Item>) purchase.ItemList)
      {
        steamWebApiRequest.AddPostValue("itemid%5B" + (object) num + "%5D", obj.ID.ToString());
        steamWebApiRequest.AddPostValue("qty%5B" + (object) num + "%5D", obj.Quantity.ToString());
        steamWebApiRequest.AddPostValue("amount%5B" + (object) num + "%5D", obj.Amount.ToString());
        steamWebApiRequest.AddPostValue("description%5B" + (object) num + "%5D", obj.Description);
        if (obj.Category != "")
          steamWebApiRequest.AddPostValue("category%5B" + (object) num + "%5D", obj.Category);
        ++num;
      }
      this._outstandingPurchases.Add(purchase);
      steamWebApiRequest.Execute(new OnRequestComplete(this.OnPurchaseStarted));
      return true;
    }

    private void OnPurchaseStarted(SteamWebAPIRequest request, string response)
    {
      InGamePurchase purchase = (InGamePurchase) null;
      ulong num = ulong.Parse(request.GetPostValue("orderid"));
      foreach (InGamePurchase inGamePurchase in this._outstandingPurchases)
      {
        if ((long) inGamePurchase.OrderID == (long) num)
        {
          purchase = inGamePurchase;
          break;
        }
      }
      if (purchase == null)
        return;
      JsonReader jsonReader = (JsonReader) new JsonTextReader((TextReader) new StringReader(response));
      while (jsonReader.Read())
      {
        if (jsonReader.TokenType == JsonToken.PropertyName && jsonReader.Value.Equals((object) "result") && jsonReader.ReadAsString().Equals("OK", StringComparison.OrdinalIgnoreCase))
          return;
      }
      purchase.InGamePurchaseCompleteCallback(purchase, false);
      this._outstandingPurchases.Remove(purchase);
    }

    private void OnTransactionAuthorizationReceived(ref MicroTxnAuthorizationResponse_t callbackData)
    {
      foreach (InGamePurchase purchase in this._outstandingPurchases)
      {
        if ((long) purchase.OrderID == (long) callbackData.m_ulOrderID)
        {
          if ((int) callbackData.m_bAuthorized == 1)
          {
            SteamWebAPIRequest steamWebApiRequest = CommunityExpress.Instance.SteamWebAPI.NewRequest(this.GetWebInterface(purchase.UseTestMode), "FinalizeTxn", "v0001");
            steamWebApiRequest.AddPostValue("key", purchase.WebAPIKey);
            steamWebApiRequest.AddPostValue("orderid", callbackData.m_ulOrderID.ToString());
            steamWebApiRequest.AddPostValue("appid", InGamePurchasing.SteamUnityAPI_SteamUtils_GetAppID().ToString());
            steamWebApiRequest.Execute(new OnRequestComplete(this.OnTransactionFinalized));
            break;
          }
          else
          {
            purchase.InGamePurchaseCompleteCallback(purchase, false);
            this._outstandingPurchases.Remove(purchase);
            break;
          }
        }
      }
    }

    private void OnTransactionFinalized(SteamWebAPIRequest request, string response)
    {
      InGamePurchase purchase = (InGamePurchase) null;
      ulong num = ulong.Parse(request.GetPostValue("orderid"));
      foreach (InGamePurchase inGamePurchase in this._outstandingPurchases)
      {
        if ((long) inGamePurchase.OrderID == (long) num)
        {
          purchase = inGamePurchase;
          break;
        }
      }
      if (purchase == null)
        return;
      bool successful = false;
      JsonReader jsonReader = (JsonReader) new JsonTextReader((TextReader) new StringReader(response));
      while (jsonReader.Read())
      {
        if (jsonReader.TokenType == JsonToken.PropertyName && jsonReader.Value.Equals((object) "result") && jsonReader.ReadAsString().Equals("OK", StringComparison.OrdinalIgnoreCase))
        {
          successful = true;
          break;
        }
      }
      purchase.InGamePurchaseCompleteCallback(purchase, successful);
      this._outstandingPurchases.Remove(purchase);
    }

    private void FetchRegionInfo(bool useTestURL, string webAPIKey)
    {
      SteamWebAPIRequest steamWebApiRequest = CommunityExpress.Instance.SteamWebAPI.NewRequest(this.GetWebInterface(useTestURL), "GetUserInfo", "v0001");
      steamWebApiRequest.AddGetValue("key", webAPIKey);
      steamWebApiRequest.AddGetValue("steamid", this._steamID.ToUInt64().ToString());
      steamWebApiRequest.Execute(new OnRequestComplete(this.OnRegionInfoRetrieved));
    }

    private void OnRegionInfoRetrieved(SteamWebAPIRequest request, string response)
    {
      JsonReader jsonReader = (JsonReader) new JsonTextReader((TextReader) new StringReader(response));
      while (jsonReader.Read())
      {
        if (jsonReader.TokenType == JsonToken.PropertyName && jsonReader.Value.Equals((object) "country"))
        {
          this._regionInfo = new RegionInfo(jsonReader.ReadAsString());
          break;
        }
      }
    }

    private string GetWebInterface(bool useTestURL)
    {
      return useTestURL ? "ISteamMicroTxnSandbox" : "ISteamMicroTxn";
    }
  }
}
