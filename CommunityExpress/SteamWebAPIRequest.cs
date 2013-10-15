// Type: CommunityExpressNS.SteamWebAPIRequest
// Assembly: CommunityExpress, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3B3F745C-AA2A-4DDF-AA8A-F5898AF84B8D
// Assembly location: F:\Program Files (x86)\FEZ\CommunityExpress.dll

using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;

namespace CommunityExpressNS
{
  public class SteamWebAPIRequest
  {
    private string _postValues = "";
    private Uri _url;
    private OnRequestComplete _onRequestComplete;
    private HttpWebRequest _request;

    internal SteamWebAPIRequest(string url)
    {
      this._url = new Uri(url);
    }

    public void AddGetValue(string key, string value)
    {
      if (this._url.Query == "")
        this._url = new Uri(this._url.ToString() + "?" + key + "=" + value);
      else
        this._url = new Uri(this._url.ToString() + "&" + key + "=" + value);
    }

    public void AddPostValue(string key, string value)
    {
      if (this._postValues == "")
      {
        this._postValues = key + "=" + value;
      }
      else
      {
        SteamWebAPIRequest steamWebApiRequest = this;
        string str = steamWebApiRequest._postValues + "&" + key + "=" + value;
        steamWebApiRequest._postValues = str;
      }
    }

    public string GetPostValue(string key)
    {
      int num1 = this._postValues.IndexOf(key, 0, StringComparison.OrdinalIgnoreCase);
      if (num1 == -1)
        return "";
      int startIndex = num1 + (key.Length + 1);
      int num2 = this._postValues.IndexOf("&", startIndex);
      if (num2 == -1)
        return this._postValues.Substring(startIndex, this._postValues.Length);
      else
        return this._postValues.Substring(startIndex, num2 - startIndex);
    }

    public void Execute(OnRequestComplete onRequestComplete)
    {
      this._onRequestComplete = onRequestComplete;
      new Thread(new ThreadStart(this.internalExecute))
      {
        IsBackground = true
      }.Start();
    }

    private void internalExecute()
    {
      this._request = (HttpWebRequest) WebRequest.Create(this._url);
      if (this._postValues != "")
      {
        this._request.ContentType = "application/x-www-form-urlencoded";
        this._request.Method = "POST";
        this._request.AllowWriteStreamBuffering = true;
        this._request.ContentLength = (long) this._postValues.Length;
        StreamWriter streamWriter = new StreamWriter(((WebRequest) this._request).GetRequestStream());
        streamWriter.Write(this._postValues);
        streamWriter.Close();
      }
      ThreadPool.RegisterWaitForSingleObject(this._request.BeginGetResponse(new AsyncCallback(this.OnResponseReceived), (object) null).AsyncWaitHandle, new WaitOrTimerCallback(SteamWebAPIRequest.ScanTimeoutCallback), (object) this, 10000, true);
    }

    private void OnResponseReceived(IAsyncResult asyncResult)
    {
      HttpWebResponse httpWebResponse = (HttpWebResponse) this._request.EndGetResponse(asyncResult);
      Encoding encoding = Encoding.GetEncoding(1252);
      StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream(), encoding);
      string response = streamReader.ReadToEnd();
      streamReader.Close();
      httpWebResponse.Close();
      this._onRequestComplete(this, response);
    }

    private static void ScanTimeoutCallback(object steamWebAPIRequest, bool timedOut)
    {
      if (!timedOut || steamWebAPIRequest == null)
        return;
      ((WebRequest) ((SteamWebAPIRequest) steamWebAPIRequest)._request).Abort();
      ((SteamWebAPIRequest) steamWebAPIRequest)._onRequestComplete((SteamWebAPIRequest) steamWebAPIRequest, "");
    }
  }
}
