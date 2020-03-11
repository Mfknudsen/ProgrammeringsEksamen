/// Summary:
/// This script will handle the interpatations of the text made from 
/// the speech and will try to find a matching command with an action 
/// to perform and will then create that action.
/// 
/// The actions will be created as empty objects with only the script for the giving command and an Action script to control it. 

#region Systems
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#endregion

public class Commands : MonoBehaviour
{
    #region Public Data
    public string AI_Name;
    public GameObject ActionObject;
    public Transform ObjectStorage;
    #endregion

    public void FindActionBasedOnText(string text)
    {
        char[] SeperateBy = { ' ', '/', '_' };
        string[] Wish = text.Split(SeperateBy);
        List<string> Command = new List<string>();

        foreach (string W in Wish)
        {
            string Word = W;

            if (Word == AI_Name || Command[0] == AI_Name)
            {
                Command.Add(Word);
            } 
        }

        if (Command.Count > 0)
        {
            string GivingCommand = "";
            foreach (string W in Command)
            {
                GivingCommand = GivingCommand + "" + W;
            }
            Debug.Log(GivingCommand);

            if (Command[1].ToLower() == "open")
            {
                if (Command[1].ToLower() == "youtube")
                {
                    Command_OpenURL("Youtube.com");
                    Debug.Log("Opening YouTube.com in standard browser.");
                }
            }
        }
        else
        {
            Debug.Log("There was no command giving it the last recording");
        }
    }


    #region Actions
    private void Command_OpenURL(string url)
    {
        GameObject newObject = Instantiate(ActionObject,Vector3.zero, Quaternion.identity, ObjectStorage);

        newObject.AddComponent<Action_OpenURL>();
        Action_OpenURL sAction = newObject.GetComponent<Action_OpenURL>();
        sAction.url = url;

        Action Action = newObject.GetComponent<Action>();
        Action.Asyncronus = true;
    }
    #endregion
}

