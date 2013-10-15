// Type: MonoGame.Framework.MonoLive.Result
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace MonoGame.Framework.MonoLive
{
  [XmlType(Namespace = "http://monolive.com/")]
  [DesignerCategory("code")]
  [GeneratedCode("System.Xml", "4.0.30319.225")]
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
