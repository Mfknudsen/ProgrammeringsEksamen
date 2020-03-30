using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;


public static class TestSaveLoad
{
    public static void SaveData(TestData DATA)
    {
        BinaryFormatter Formatter = new BinaryFormatter();

        string path = Application.persistentDataPath + "/TestInformation.DATA";

        if (File.Exists(path) == false)
        {
            FileStream Stream = new FileStream(path, FileMode.Create);

            Formatter.Serialize(Stream, DATA);
            Stream.Close();
        }
        else
        {

        }
    }

    public static TestData LoadDatabase()
    {
        string path = Application.persistentDataPath + "/TestInformation.DATA";

        if (File.Exists(path))
        {
            BinaryFormatter Formatter = new BinaryFormatter();
            FileStream Stream = new FileStream(path, FileMode.Open);

            TestData DATA = Formatter.Deserialize(Stream) as TestData;
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
