// Type: MonoGame.Framework.MonoLive.MonoLive
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Web.Services;
using System.Web.Services.Description;
using System.Web.Services.Protocols;

namespace MonoGame.Framework.MonoLive
{
  [DebuggerStepThrough]
  [GeneratedCode("System.Web.Services", "4.0.30319.1")]
  [DesignerCategory("code")]
  [WebServiceBinding(Name = "MonoLiveSoap", Namespace = "http://monolive.com/")]
  public class MonoLive : SoapHttpClientProtocol
  {
    private SendOrPostCallback SignInOperationCompleted;
    private SendOrPostCallback RegisterOperationCompleted;
    private bool useDefaultCredentialsSetExplicitly;

    public new string Url
    {
      get
      {
        return base.Url;
      }
      set
      {
        if (this.IsLocalFileSystemWebService(base.Url) && !this.useDefaultCredentialsSetExplicitly && !this.IsLocalFileSystemWebService(value))
          base.UseDefaultCredentials = false;
        base.Url = value;
      }
    }

    public new bool UseDefaultCredentials
    {
      get
      {
        return base.UseDefaultCredentials;
      }
      set
      {
        base.UseDefaultCredentials = value;
        this.useDefaultCredentialsSetExplicitly = true;
      }
    }

    public event SignInCompletedEventHandler SignInCompleted;

    public event RegisterCompletedEventHandler RegisterCompleted;

    public MonoLive()
    {
      this.Url = "https://monolive.com";
      if (this.IsLocalFileSystemWebService(this.Url))
      {
        this.UseDefaultCredentials = true;
        this.useDefaultCredentialsSetExplicitly = false;
      }
      else
        this.useDefaultCredentialsSetExplicitly = true;
    }

    [SoapDocumentMethod("http://monolive.com/SignIn", ParameterStyle = SoapParameterStyle.Wrapped, RequestNamespace = "http://monolive.com/", ResponseNamespace = "http://monolive.com/", Use = SoapBindingUse.Literal)]
    public Result SignIn(string username, string password)
    {
      return (Result) this.Invoke("SignIn", new object[2]
      {
        (object) username,
        (object) password
      })[0];
    }

    public void SignInAsync(string username, string password)
    {
      this.SignInAsync(username, password, (object) null);
    }

    public void SignInAsync(string username, string password, object userState)
    {
      if (this.SignInOperationCompleted == null)
        this.SignInOperationCompleted = new SendOrPostCallback(this.OnSignInOperationCompleted);
      this.InvokeAsync("SignIn", new object[2]
      {
        (object) username,
        (object) password
      }, this.SignInOperationCompleted, userState);
    }

    private void OnSignInOperationCompleted(object arg)
    {
      if (this.SignInCompleted == null)
        return;
      InvokeCompletedEventArgs completedEventArgs = (InvokeCompletedEventArgs) arg;
      this.SignInCompleted((object) this, new SignInCompletedEventArgs(completedEventArgs.Results, completedEventArgs.Error, completedEventArgs.Cancelled, completedEventArgs.UserState));
    }

    [SoapDocumentMethod("http://monolive.com/Register", ParameterStyle = SoapParameterStyle.Wrapped, RequestNamespace = "http://monolive.com/", ResponseNamespace = "http://monolive.com/", Use = SoapBindingUse.Literal)]
    public Result Register(string username, string password, string gamertag)
    {
      return (Result) this.Invoke("Register", new object[3]
      {
        (object) username,
        (object) password,
        (object) gamertag
      })[0];
    }

    public void RegisterAsync(string username, string password, string gamertag)
    {
      this.RegisterAsync(username, password, gamertag, (object) null);
    }

    public void RegisterAsync(string username, string password, string gamertag, object userState)
    {
      if (this.RegisterOperationCompleted == null)
        this.RegisterOperationCompleted = new SendOrPostCallback(this.OnRegisterOperationCompleted);
      this.InvokeAsync("Register", new object[3]
      {
        (object) username,
        (object) password,
        (object) gamertag
      }, this.RegisterOperationCompleted, userState);
    }

    private void OnRegisterOperationCompleted(object arg)
    {
      if (this.RegisterCompleted == null)
        return;
      InvokeCompletedEventArgs completedEventArgs = (InvokeCompletedEventArgs) arg;
      this.RegisterCompleted((object) this, new RegisterCompletedEventArgs(completedEventArgs.Results, completedEventArgs.Error, completedEventArgs.Cancelled, completedEventArgs.UserState));
    }

    public new void CancelAsync(object userState)
    {
      base.CancelAsync(userState);
    }

    private bool IsLocalFileSystemWebService(string url)
    {
      if (url == null || url == string.Empty)
        return false;
      Uri uri = new Uri(url);
      return uri.Port >= 1024 && string.Compare(uri.Host, "localHost", StringComparison.OrdinalIgnoreCase) == 0;
    }
  }
}
