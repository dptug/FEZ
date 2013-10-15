// Type: MonoGame.Framework.MonoLive.Gamer
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace MonoGame.Framework.MonoLive
{
  [GeneratedCode("System.Xml", "4.0.30319.225")]
  [DesignerCategory("code")]
  [XmlType(Namespace = "http://monolive.com/")]
  [DebuggerStepThrough]
  [Serializable]
  public class Gamer
  {
    private long idField;
    private string gamerTagField;
    private bool enabledField;
    private string gravatarField;
    private string emailField;
    private string passwordField;

    public long Id
    {
      get
      {
        return this.idField;
      }
      set
      {
        this.idField = value;
      }
    }

    public string GamerTag
    {
      get
      {
        return this.gamerTagField;
      }
      set
      {
        this.gamerTagField = value;
      }
    }

    public bool Enabled
    {
      get
      {
        return this.enabledField;
      }
      set
      {
        this.enabledField = value;
      }
    }

    public string Gravatar
    {
      get
      {
        return this.gravatarField;
      }
      set
      {
        this.gravatarField = value;
      }
    }

    public string Email
    {
      get
      {
        return this.emailField;
      }
      set
      {
        this.emailField = value;
      }
    }

    public string Password
    {
      get
      {
        return this.passwordField;
      }
      set
      {
        this.passwordField = value;
      }
    }
  }
}
