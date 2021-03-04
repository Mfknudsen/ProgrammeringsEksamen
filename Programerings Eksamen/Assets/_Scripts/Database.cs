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
    [HideInInspector]
    public List<Dictionary<string, string[]>> CommandList = new List<Dictionary<string, string[]>>();
    #endregion

    #region Private Data
    Master Master;
    #endregion

    private void Start()
    {
        CommandList = new List<Dictionary<string, string[]>>();

        Debug.Log(Application.persistentDataPath);
        Master = GetComponent<Master>();
    }

    public void LoadOldFromJson(Database data, Commands COM)
    {
        bool check = false;

        if (data.CommandList.Count > 0)
        {
            CommandList = new List<Dictionary<string, string[]>>();
            CommandList = data.CommandList;

            Master.DatabaseIsLoaded = true;
            COM.CommandList = CommandList;
            Master.ReceiveNewLog("Data is loaded!");
            check = true;
        }
        else if (data.CommandList.Count <= 0)
        {
            Master.ReceiveNewLog("Failed to load!");
            check = true;
        }

        if (!check)
        {
            Master.ReceiveNewLog("Failed to load!");
        }
    }
}

