/// Summary:
/// This script will hold all data that the Master dont activly need 
/// and that will be saved upon closing of the application.

#region Systems:
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#endregion

[System.Serializable]
public class Database : MonoBehaviour
{
    #region Public Data
    public string AI_Name;
    [HideInInspector]
    public List<Action> Continues_AI_Actions = new List<Action>();
    [HideInInspector]
    public List<Action> Continues_Async_AI_Actions = new List<Action>();
    #endregion

    #region Private Data
    Master Master;
    #endregion

    public void Start()
    {
        Debug.Log(Application.persistentDataPath);
        if (Master == null)
        {
            Master = GetComponent<Master>();
        }
    }

    public void LoadOldFromJson(Database data)
    {
        AI_Name = data.AI_Name;

        Continues_AI_Actions = data.Continues_AI_Actions;
        Continues_Async_AI_Actions = data.Continues_Async_AI_Actions;

        Master.DatabaseIsLoaded = true;
    }
}

