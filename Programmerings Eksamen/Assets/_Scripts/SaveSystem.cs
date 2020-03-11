#region Systems
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
#endregion

public static class SaveSystem
{
    public static void SaveData(Database DATA)
    {
        BinaryFormatter Formatter = new BinaryFormatter();

        string path = Application.persistentDataPath + "/Information.DATA";

        FileStream Stream = new FileStream(path, FileMode.Create);

        Formatter.Serialize(Stream, DATA);
        Stream.Close();
    }

    public static Database LoadDatabase()
    {
        string path = Application.persistentDataPath + "/Information.DATA";

        if (File.Exists(path))
        {
            BinaryFormatter Formatter = new BinaryFormatter();
            FileStream Stream = new FileStream(path, FileMode.Open);

            Database DATA = Formatter.Deserialize(Stream) as Database;
            Stream.Close();

            return DATA;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }
}
