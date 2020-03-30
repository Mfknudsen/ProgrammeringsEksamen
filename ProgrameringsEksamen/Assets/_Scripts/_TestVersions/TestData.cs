using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestData : MonoBehaviour
{
    public string Name;

    void Start()
    {
        Debug.Log(Application.persistentDataPath);
    }

    void Update()
    {
        Debug.Log(Name);
    }

    public void ChangeName(string newName)
    {
        Name = newName;
    }

    public void Load()
    {
        TestData TD = TestSaveLoad.LoadDatabase();


    }

    public void Save()
    {
        TestSaveLoad.SaveData(this);
    }
}
