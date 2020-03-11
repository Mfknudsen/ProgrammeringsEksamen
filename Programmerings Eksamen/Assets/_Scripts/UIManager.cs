/// Summary:
/// This script will handle all object 
/// or similar that is connected to UI 
/// that the user can activly see.

#region Systems:
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
#endregion

public class UIManager : MonoBehaviour
{
    #region Public Data
    public TextMeshProUGUI AI_Log;
    public TextMeshProUGUI Speech_Log;
    #endregion

    private void Start()
    {
        AI_Log.text = "Local Json datapath: \n"+ Application.persistentDataPath;
    }

    public void AddToLog(string Addition)
    {
        if (Addition != null)
        {
            AI_Log.text = Addition + "\n \n" + AI_Log.text;
        }
    }

    public void ReplaceSpeechText(string newText)
    {
        Speech_Log.text = newText;
    }
}
