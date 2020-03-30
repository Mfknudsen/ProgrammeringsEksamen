/// Summary:
/// This script will handle the saving and loading to 
/// and from a local Json document.
/// 
/// The document will hold all files that are needed 
/// to required when the AI starts up again.

#region Systems
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using System;
using System.Runtime.Serialization.Formatters.Binary;
#endregion

public static class SaveSystem
{
    public static void SaveData(Database DATA)
    {
        string FileName = "/Info.txt";
        string json = "";

        int n = 0;
        List<FromDictionaryToJson> DJ = new List<FromDictionaryToJson>();
        foreach (Dictionary<string, string[]> D in DATA.CommandList)
        {
            FromDictionaryToJson dj = new FromDictionaryToJson();

            dj.listNumber = n;
            dj.keys = new List<string>();
            dj.valuesOfKey = new List<string>();

            foreach (string key in D.Keys)
            {
                dj.keys.Add(key);
                string tempS = "";

                foreach (string s in D[key])
                {
                    if (s != "" && s != " ")
                    {
                        tempS += s + "_";
                    }
                }
                dj.valuesOfKey.Add(tempS);
            }

            json += "-" + JsonUtility.ToJson(dj);
            n++;
        }

        File.WriteAllText(Application.persistentDataPath + FileName, json);
    }

    public static Database LoadDatabase()
    {
        Database DB = GameObject.FindGameObjectWithTag("Player").GetComponent<Database>();
        DB.AI_Name = "computer";
        string FileName = "/Info.txt";
        Dictionary<string, string[]> tempCommands = new Dictionary<string, string[]>();
        List<FromDictionaryToJson> JS = new List<FromDictionaryToJson>();

        if (File.Exists(Application.persistentDataPath + FileName))
        {
            string response = "";
            StreamReader reader = new StreamReader(Application.persistentDataPath + "/Info.txt");

            while (!reader.EndOfStream)
            {
                response += reader.ReadLine();
            }
            reader.Close();

            string[] responses = response.Split('-');

            for (int i = 0; i < responses.Length; i++)
            {
                if (responses[i] != "")
                {
                    JS.Add(JsonUtility.FromJson<FromDictionaryToJson>(responses[i]));
                }
            }

            /*JsonCollection tempCollection = JsonUtility.FromJson<JsonCollection>(response);
            foreach (FromDictionaryToJson DJ in tempCollection.JsonInfo)
            {
                JS.Add(DJ);
            }*/


            foreach (FromDictionaryToJson t in JS)
            {
                for (int i = 0; i < t.keys.Count; i++)
                {
                    List<string> tempList = new List<string>();
                    foreach (string s in t.valuesOfKey[i].Split('_'))
                    {
                        tempList.Add(s.Replace("_", ""));
                    }

                    tempCommands.Add(t.keys[i], tempList.ToArray());
                }
                DB.CommandList.Add(tempCommands);
                tempCommands = new Dictionary<string, string[]>();
            }

            return DB;
        }
        else
        {
            Debug.Log("No save file found");
            return null;
        }
    }
}

[Serializable]
public struct FromDictionaryToJson
{
    public int listNumber;
    public List<string> keys;
    public List<string> valuesOfKey;
}

[Serializable]
public struct JsonCollection
{
    public List<FromDictionaryToJson> JsonInfo;
}
