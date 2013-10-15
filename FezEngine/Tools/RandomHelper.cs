// Type: FezEngine.Tools.RandomHelper
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using Common;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace FezEngine.Tools
{
  public static class RandomHelper
  {
    public static Random Random
    {
      get
      {
        LocalDataStoreSlot namedDataSlot = Thread.GetNamedDataSlot("Random");
        object obj;
        if ((obj = Thread.GetData(namedDataSlot)) == null)
          Thread.SetData(namedDataSlot, (object) (Random) (obj = (object) new Random()));
        return obj as Random;
      }
    }

    public static bool Probability(double p)
    {
      return p > RandomHelper.Random.NextDouble();
    }

    public static int Sign()
    {
      return !RandomHelper.Probability(0.5) ? 1 : -1;
    }

    public static float Centered(double distance)
    {
      return (float) ((RandomHelper.Random.NextDouble() - 0.5) * distance * 2.0);
    }

    public static float Centered(double distance, double around)
    {
      return (float) ((RandomHelper.Random.NextDouble() - 0.5) * distance * 2.0 + around);
    }

    public static float Between(double min, double max)
    {
      return (float) (RandomHelper.Random.NextDouble() * (max - min) + min);
    }

    public static float Unit()
    {
      return (float) RandomHelper.Random.NextDouble();
    }

    public static T EnumField<T>(bool excludeFirst) where T : struct
    {
      IEnumerable<T> values = Util.GetValues<T>();
      return Enumerable.ElementAt<T>(values, RandomHelper.Random.Next(excludeFirst ? 1 : 0, Enumerable.Count<T>(values)));
    }

    public static T EnumField<T>() where T : struct
    {
      return RandomHelper.EnumField<T>(false);
    }

    public static T InList<T>(T[] list)
    {
      return list[RandomHelper.Random.Next(0, list.Length)];
    }

    public static T InList<T>(List<T> list)
    {
      return list[RandomHelper.Random.Next(0, list.Count)];
    }

    public static T InList<T>(IEnumerable<T> list)
    {
      return Enumerable.ElementAt<T>(list, RandomHelper.Random.Next(0, Enumerable.Count<T>(list)));
    }

    public static Vector3 NormalizedVector()
    {
      return Vector3.Normalize(new Vector3((float) ((double) RandomHelper.Unit() * 2.0 - 1.0), (float) ((double) RandomHelper.Unit() * 2.0 - 1.0), (float) ((double) RandomHelper.Unit() * 2.0 - 1.0)));
    }
  }
}
