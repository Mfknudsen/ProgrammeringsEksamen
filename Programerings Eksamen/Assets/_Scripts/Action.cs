/// Summary:
/// This script will be the handler for all actions 
/// the AI can perform based on a giving command.

#region Systems:
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#endregion

public class Action : MonoBehaviour
{
    public Dictionary<string, string[]> ActionCommand;
    public bool Asyncronus = false;
    public bool Contenius = false;
    public bool IsStarted = false;
    public bool IsDone = false;
    public string ActionName = "";

    public void StartAction()
    {
        if (!IsStarted)
        {
            SendMessage("StartNow");
        }
    }

    public virtual void UpdateAction()
    {
        if (IsStarted && !IsDone)
        {
            SendMessage("UpdateNow");
        }
    }

    public void DestroyObject()
    {
        Destroy(gameObject);
    }
}
