// Type: MonoGame.Framework.MonoLive.Result
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
  public class Result
  {
    private bool okField;
    private string reasonField;
    private Gamer gamerField;

    public bool ok
    {
      get
      {
        return this.okField;
      }
      set
      {
        this.okField = value;
      }
    }

    public string reason
    {
      get
      {
        return this.reasonField;
      }
      set
      {
        this.reasonField = value;
      }
    }

    public Gamer Gamer
    {
      get
      {
        return this.gamerField;
      }
      set
      {
        this.gamerField = value;
      }
    }
  }
}
