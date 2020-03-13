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
    public string AI_Name = "computer";
    public GameObject ActionObject;
    public Transform ObjectStorage;
    #endregion

    #region Private Data
    Master Master;
    private bool IsMuted = false;
    #endregion

    private void Start()
    {
        if (Master == null)
            Master = GetComponent<Master>();
    }

    public void FindActionBasedOnText(List<string> text)
    {
        WasCommandFound = false;
        List<string> Command = text;

        if (Command.Count > 0)
        {
            if (Command[0] == AI_Name)
            {
                if (Command.Count > 1)
                {
                    if (Command[1] == "open" && !IsMuted)
                    {
                        if (Command.Count > 2)
                        {
                            if (Command[2] == "youtube")
                            {
                                Master.ReceiveNewLog("Opening Youtube.");
                                WasCommandFound = true;
                                Command_OpenURL("https://www.youtube.com/");
                            }
                            else if (Command[2] == "facebook")
                            {
                                if (Command.Count > 3)
                                {
                                    if (Command[3] == "groups")
                                    {
                                        Master.ReceiveNewLog("Opening Facebook Groups.");
                                        WasCommandFound = true;
                                        Command_OpenURL("https://www.facebook.com/groups/");
                                    }
                                    else
                                    {
                                        Master.ReceiveNewLog("Opening Facebook.");
                                        WasCommandFound = true;
                                        Command_OpenURL("https://www.facebook.com/");
                                    }
                                }
                                else
                                {
                                    Master.ReceiveNewLog("Opening Facebook.");
                                    WasCommandFound = true;
                                    Command_OpenURL("https://www.facebook.com/");
                                }
                            }
                            else if (Command[2] == "messenger")
                            {
                                Master.ReceiveNewLog("Opening Messenger.");
                                WasCommandFound = true;
                                Command_OpenURL("https://www.facebook.com/messages");
                            }
                        }
                    }
                    else if (Command[1] == "mute" && !IsMuted)
                    {
                        IsMuted = true;
                        Master.ReceiveNewLog("Muted has been activated. \nThe computer will only react to the command unmute.");
                    }
                    else if (Command[1] == "unmute" && IsMuted)
                    {
                        IsMuted = false;
                        Master.ReceiveNewLog("Muted has been deactivated. \nThe computer will now react to all commands.");
                    }
                }
            }
        }
    }


    #region Actions
    private void Command_OpenURL(string url)
    {
        GameObject newObject = Instantiate(ActionObject, Vector3.zero, Quaternion.identity, ObjectStorage);

        newObject.AddComponent<Action_OpenURL>();
        Action_OpenURL sAction = newObject.GetComponent<Action_OpenURL>();
        sAction.url = url;

        Action Action = newObject.GetComponent<Action>();
        Action.Asyncronus = true;
        Master.ReceiveNewAction(Action);
    }
    #endregion
}

