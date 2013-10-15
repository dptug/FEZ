// Type: EasyStorage.PCSaveDevice
// Assembly: EasyStorage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F34DFF18-A3CB-48F6-8800-AFF45A305BF1
// Assembly location: F:\Program Files (x86)\FEZ\EasyStorage.dll

using CommunityExpressNS;
using System;
using System.IO;

namespace EasyStorage
{
  public class PCSaveDevice : ISaveDevice
  {
    public const int MaxSize = 40960;
    private static bool CloudSavesSynced;

    public string RootDirectory { get; set; }

    public PCSaveDevice(string gameName)
    {
      this.RootDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), gameName);
      if (PCSaveDevice.CloudSavesSynced)
        return;
      RemoteStorage remoteStorage = CommunityExpress.Instance.RemoteStorage;
      for (int index = 0; index < 3; ++index)
      {
        string str = "SaveSlot" + (object) index;
        PCSaveDevice.CloudEntry cloudEntry = new PCSaveDevice.CloudEntry()
        {
          Exists = remoteStorage.FileExists(str)
        };
        try
        {
          if (cloudEntry.Exists)
          {
            CommunityExpressNS.File file = remoteStorage.GetFile(str);
            cloudEntry.Data = file.ReadBytes();
            using (MemoryStream memoryStream = new MemoryStream(cloudEntry.Data))
            {
              using (BinaryReader binaryReader = new BinaryReader((Stream) memoryStream))
                cloudEntry.LastModifiedTimestamp = new long?(binaryReader.ReadInt64());
            }
          }
        }
        catch (Exception ex)
        {
          PCSaveDevice.Log(string.Concat(new object[4]
          {
            (object) "Error getting cloud save #",
            (object) index,
            (object) " : ",
            (object) ex
          }));
          cloudEntry.Corrupted = true;
        }
        try
        {
          if (remoteStorage.FileExists(str + "_LastDelete"))
          {
            using (MemoryStream memoryStream = new MemoryStream(remoteStorage.GetFile(str + "_LastDelete").ReadBytes()))
            {
              using (BinaryReader binaryReader = new BinaryReader((Stream) memoryStream))
                cloudEntry.LastDeletedTimestamp = new long?(binaryReader.ReadInt64());
            }
          }
        }
        catch (Exception ex)
        {
          PCSaveDevice.Log(string.Concat(new object[4]
          {
            (object) "Error getting last delete time for cloud save #",
            (object) index,
            (object) " : ",
            (object) ex
          }));
        }
        string path = Path.Combine(this.RootDirectory, str);
        if (!System.IO.File.Exists(path))
        {
          if (cloudEntry.Exists && !cloudEntry.Corrupted && (!cloudEntry.LastDeletedTimestamp.HasValue || cloudEntry.LastDeletedTimestamp.Value < cloudEntry.LastModifiedTimestamp.Value))
          {
            PCSaveDevice.Log("Copying back cloud save #" + (object) index + " to local because it did not exist locally");
            try
            {
              using (FileStream fileStream = new FileStream(path, FileMode.Create))
              {
                using (BinaryWriter binaryWriter = new BinaryWriter((Stream) fileStream))
                  binaryWriter.Write(cloudEntry.Data);
              }
            }
            catch (Exception ex)
            {
              PCSaveDevice.Log(string.Concat(new object[4]
              {
                (object) "Error copying cloud entry data to local for cloud save #",
                (object) index,
                (object) " : ",
                (object) ex
              }));
            }
          }
        }
        else
        {
          long num = long.MinValue;
          try
          {
            using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
              using (BinaryReader binaryReader = new BinaryReader((Stream) fileStream))
                num = binaryReader.ReadInt64();
            }
          }
          catch (Exception ex)
          {
            PCSaveDevice.Log("Error while loading local file for timestamp compare : " + (object) ex);
          }
          if (cloudEntry.LastDeletedTimestamp.HasValue && cloudEntry.LastDeletedTimestamp.Value > num)
          {
            PCSaveDevice.Log("Deleting local save #" + (object) index + " because of pending cloud deletion");
            System.IO.File.Delete(path);
            num = long.MinValue;
          }
          if (cloudEntry.Exists && !cloudEntry.Corrupted && (!cloudEntry.LastDeletedTimestamp.HasValue || cloudEntry.LastDeletedTimestamp.Value < cloudEntry.LastModifiedTimestamp.Value) && cloudEntry.LastModifiedTimestamp.Value > num)
          {
            PCSaveDevice.Log("Copying back cloud save #" + (object) index + " to local because the local copy is older");
            try
            {
              using (FileStream fileStream = new FileStream(path, FileMode.Create))
              {
                using (BinaryWriter binaryWriter = new BinaryWriter((Stream) fileStream))
                  binaryWriter.Write(cloudEntry.Data);
              }
            }
            catch (Exception ex)
            {
              PCSaveDevice.Log(string.Concat(new object[4]
              {
                (object) "Error copying cloud entry data to local for cloud save #",
                (object) index,
                (object) " : ",
                (object) ex
              }));
            }
          }
        }
        PCSaveDevice.CloudSavesSynced = true;
      }
    }

    private static void Log(string message)
    {
      try
      {
        Console.WriteLine("({0}) [{1}] {2}", (object) DateTime.Now.ToString("HH:mm:ss.fff"), (object) "SaveDevice", (object) message);
        using (FileStream fileStream = System.IO.File.Open(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "FEZ") + "\\Debug Log.txt", FileMode.Append))
        {
          using (StreamWriter streamWriter = new StreamWriter((Stream) fileStream))
            streamWriter.WriteLine("({0}) [{1}] {2}", (object) DateTime.Now.ToString("HH:mm:ss.fff"), (object) "SaveDevice", (object) message);
        }
      }
      catch (Exception ex)
      {
      }
    }

    public virtual bool Save(string fileName, SaveAction saveAction)
    {
      if (!Directory.Exists(this.RootDirectory))
        Directory.CreateDirectory(this.RootDirectory);
      string str = Path.Combine(this.RootDirectory, fileName);
      if (System.IO.File.Exists(str))
        System.IO.File.Copy(str, str + "_Backup", true);
      try
      {
        byte[] buffer = new byte[40960];
        using (MemoryStream memoryStream = new MemoryStream(buffer))
        {
          using (BinaryWriter writer = new BinaryWriter((Stream) memoryStream))
          {
            writer.Write(DateTime.Now.ToFileTime());
            saveAction(writer);
            if (memoryStream.Length < 40960L)
            {
              long length = 40960L - memoryStream.Length;
              writer.Write(new byte[length]);
            }
            else if (memoryStream.Length > 40960L)
              throw new InvalidOperationException("Save file greater than the imposed limit!");
          }
        }
        using (FileStream fileStream = new FileStream(str, FileMode.Create, FileAccess.Write, FileShare.Read))
        {
          using (BinaryWriter binaryWriter = new BinaryWriter((Stream) fileStream))
            binaryWriter.Write(buffer);
        }
        return true;
      }
      catch (Exception ex)
      {
        PCSaveDevice.Log("Error while saving : " + (object) ex);
      }
      return false;
    }

    public virtual bool Load(string fileName, LoadAction loadAction)
    {
      if (!Directory.Exists(this.RootDirectory))
        Directory.CreateDirectory(this.RootDirectory);
      bool flag = false;
      string path = Path.Combine(this.RootDirectory, fileName);
      try
      {
        using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
        {
          using (BinaryReader reader = new BinaryReader((Stream) fileStream))
          {
            reader.ReadInt64();
            loadAction(reader);
            flag = true;
          }
        }
      }
      catch (Exception ex)
      {
        if (!fileName.EndsWith("_Backup"))
        {
          if (System.IO.File.Exists(path + "_Backup"))
          {
            PCSaveDevice.Log("Loading error, will try with backup : " + (object) ex);
            return this.Load(fileName + "_Backup", loadAction);
          }
          else
            PCSaveDevice.Log("Loading error, no backup found : " + (object) ex);
        }
        else
          PCSaveDevice.Log("Error loading backup : " + (object) ex);
      }
      return flag;
    }

    public virtual bool Delete(string fileName)
    {
      if (!Directory.Exists(this.RootDirectory))
        Directory.CreateDirectory(this.RootDirectory);
      RemoteStorage remoteStorage = CommunityExpress.Instance.RemoteStorage;
      if (remoteStorage.FileExists(fileName))
        remoteStorage.DeleteFile(fileName);
      if (remoteStorage.FileExists(fileName + "_LastDelete"))
        remoteStorage.DeleteFile(fileName + "_LastDelete");
      remoteStorage.WriteFile(fileName + "_LastDelete", BitConverter.GetBytes(DateTime.Now.ToFileTime()));
      string path = Path.Combine(this.RootDirectory, fileName);
      if (!System.IO.File.Exists(path))
        return true;
      System.IO.File.Delete(path);
      return !System.IO.File.Exists(path);
    }

    public virtual bool FileExists(string fileName)
    {
      if (!Directory.Exists(this.RootDirectory))
        Directory.CreateDirectory(this.RootDirectory);
      return System.IO.File.Exists(Path.Combine(this.RootDirectory, fileName));
    }

    private struct CloudEntry
    {
      public bool Corrupted;
      public bool Exists;
      public long? LastDeletedTimestamp;
      public long? LastModifiedTimestamp;
      public byte[] Data;
    }
  }
}
