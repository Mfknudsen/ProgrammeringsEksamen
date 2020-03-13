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
    public TextMeshProUGUI SpeechLog;
    #endregion

    private void Start()
    {
        AI_Log.text = "Local Json datapath: \n" + Application.persistentDataPath;
        SpeechLog.text = "Ready for Speech!";
    }

    public void AddToLog(string Addition)
    {
        AI_Log.text = Addition + "\n \n" + AI_Log.text;
    }

    public void ReplaceSpeechText(string newText)
    {
        if (newText.Length > 0)
        {
            string First = char.ToUpper(newText[0]) + newText.Substring(1);
            SpeechLog.text = First;
        }
    }
}
