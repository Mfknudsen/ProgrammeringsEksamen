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
    public List<Dictionary<string, string[]>> CommandList = new List<Dictionary<string, string[]>>();
    public GameObject ActionObject;
    public Transform ObjectStorage;
    //All input needed for the actions.
    [Header("ACtion Input:")]
    GameObject AI_Log;
    #endregion

    #region Private Data
    Master Master;
    Database Data;
    UIManager UI;
    private bool IsMuted = false;
    #endregion

    public void StartNow(Master M, Database D, UIManager U)
    {
        Master = M;
        Data = D;
        UI = U;

        if (AI_Log == null)
            AI_Log = GameObject.FindGameObjectWithTag("AI_LOG");

        SetStandardCommands();
    }

    private void SetStandardCommands()
    {
        bool show = true, hide = true, mute = true, unmute = true, com = true, ai = true;

        foreach (Dictionary<string, string[]> L in CommandList)
        {
            string t = L["command"][0];

            if (t == "computer hide AI log")
                hide = false;
            else if (t == "computer show AI log")
                show = false;
            else if (t == "computer mute")
                mute = false;
            else if (t == "computer unmute")
                unmute = false;
            else if (t == "computer switch to main screen")
                ai = false;
            else if (t == "computer switch to command list")
                com = false;
        }

        Dictionary<string, string[]> temp = new Dictionary<string, string[]>();
        if (hide)
        {
            temp = new Dictionary<string, string[]>();
            temp.Add("command", new string[] { "computer hide AI log" });
            temp.Add("commandType", new string[] { "Hide/Show AI Log", "false" });
            temp.Add("commandInput", new string[] { "false" });
            CommandList.Add(temp);
        }
        if (show)
        {
            temp = new Dictionary<string, string[]>();
            temp.Add("command", new string[] { "computer show AI log" });
            temp.Add("commandType", new string[] { "Hide/Show AI Log", "false" });
            temp.Add("commandInput", new string[] { "true" });
            CommandList.Add(temp);
        }
        if (mute)
        {
            temp = new Dictionary<string, string[]>();
            temp.Add("command", new string[] { "computer mute" });
            temp.Add("commandType", new string[] { "Mute / Unmute", "false" });
            temp.Add("commandInput", new string[] { "true" });
            CommandList.Add(temp);
        }
        if (unmute)
        {
            temp = new Dictionary<string, string[]>();
            temp.Add("command", new string[] { "computer unmute" });
            temp.Add("commandType", new string[] { "Mute / Unmute", "false" });
            temp.Add("commandInput", new string[] { "false" });
            CommandList.Add(temp);
        }
        if (com)
        {
            temp = new Dictionary<string, string[]>();
            temp.Add("command", new string[] { "computer switch to command list" });
            temp.Add("commandType", new string[] { "Show Command List", "false" });
            temp.Add("commandInput", new string[] { "true" });
            CommandList.Add(temp);
        }
        if (ai)
        {
            temp = new Dictionary<string, string[]>();
            temp.Add("command", new string[] { "computer switch to main screen" });
            temp.Add("commandType", new string[] { "Show Command List", "false" });
            temp.Add("commandInput", new string[] { "false" });
            CommandList.Add(temp);
        }

        Data.CommandList = CommandList;
    }

    public void FindActionBasedOnText(List<string> text)
    {
        foreach (Dictionary<string, string[]> AL in CommandList)
        {
            bool hasFoundCommand = true;

            string[] CommandFromList = AL["command"][0].Split(' ');

            if (text.Count >= CommandFromList.Length)
            {
                for (int i = 0; i < CommandFromList.Length; i++)
                {
                    if (CommandFromList[i] != text[i])
                    {
                        hasFoundCommand = false;
                    }
                }

                if (hasFoundCommand)
                {
                    string CommandType = AL["commandType"][0];

                    if (CommandType == "Open Url" && !IsMuted)
                    {
                        Master.ReceiveNewLog("Opening URL: \n" + AL["commandInput"][0]);
                        Command_OpenURL(AL, AL["commandInput"]);
                    }
                    else if (CommandType == "Hide/Show AI Log")
                    {
                        if (AL["commandInput"][0] == "true" && !IsMuted)
                            Master.ReceiveNewLog("Showing the AI Log.");
                        else
                            Master.ReceiveNewLog("Hiden the AI Log.");

                        HideShowAILog(AL, AL["commandInput"][0]);
                    }
                    else if (CommandType == "Set Timer" && !IsMuted)
                    {
                        Master.ReceiveNewLog("Setting timer.");
                        SetTimer(AL);
                    }
                    else if (CommandType == "Open External App" && !IsMuted)
                    {
                        Master.ReceiveNewLog("Opening App named: \n" + AL["commandInput"][3]);
                        OpenExternalApplication(AL, AL["commandInput"]);
                    }
                    else if (CommandType == "Mute / Unmute")
                    {
                        if (AL["commandInput"][0] == "true")
                            Master.ReceiveNewLog("Muting the AI \nThe AI will now only react to the unmute command!");
                        else
                            Master.ReceiveNewLog("Unmuting the AI \nThe AI will now react to all commands!");

                        MuteUnmute(AL["commandInput"][0]);
                    }
                    else if (CommandType == "Show Command List")
                    {
                        if (AL["commandInput"][0] == "true")
                        {
                            Master.ReceiveNewLog("Switching to Command List Screen.");
                            MainCommandScreen(true);
                        }
                        else
                        {
                            Master.ReceiveNewLog("Switching to Main Screen.");
                            MainCommandScreen(false);
                        }
                    }
                    break;
                }
            }
        }


        ///Note:
        ///This is different commands that are hardcoded into the ai.
        ///I later changed it to a more open form that would allow for the user to later add or remove commands.
        ///Note:
        #region TEMP_HardcodeCommands
        /*List<string> Command = text;

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
                                Command_OpenURL("https://www.youtube.com/", "Open URL: Youtube.");
                            }
                            else if (Command[2] == "facebook")
                            {
                                if (Command.Count > 3)
                                {
                                    if (Command[3] == "groups")
                                    {
                                        Master.ReceiveNewLog("Opening Facebook Groups.");
                                        Command_OpenURL("https://www.facebook.com/groups/", "Open URL: Facebook.");
                                    }
                                    else
                                    {
                                        Master.ReceiveNewLog("Opening Facebook.");
                                        Command_OpenURL("https://www.facebook.com/", "Open URL: Facebook.");
                                    }
                                }
                                else
                                {
                                    Master.ReceiveNewLog("Opening Facebook.");
                                    Command_OpenURL("https://www.facebook.com/", "Open URL: Facebook.");
                                }
                            }
                            else if (Command[2] == "messenger")
                            {
                                Master.ReceiveNewLog("Opening Messenger.");
                                Command_OpenURL("https://www.facebook.com/messages", "Open URL: Messenger.");
                            }
                            else if (Command[2] == "reddit")
                            {
                                Master.ReceiveNewLog("Opening Reddit.");
                                Command_OpenURL("https://www.reddit.com/", "Open URL: Reddit.");
                            }
                            else if (Command[2] == "school")
                            {
                                if (Command.Count > 3)
                                {
                                    if (Command[3] == "website")
                                    {
                                        Master.ReceiveNewLog("Opening Lectio.");
                                        Command_OpenURL("https://www.lectio.dk/lectio/557/login.aspx", "Open URL: Lectio.");
                                    }
                                    else if (Command[3] == "documents")
                                    {
                                        Master.ReceiveNewLog("Opening Google Docs.");
                                        Command_OpenURL("https://docs.google.com/document/u/0/", "Open URL: Google Docs.");
                                    }
                                }
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
                    else if (Command[1] == "hide")
                    {
                        if (Command.Count > 2)
                        {
                            if (Command[2] == "AI")
                            {
                                if (Command.Count > 3)
                                {
                                    if (Command[3] == "log")
                                    {
                                        HideShowAILog(false);
                                        Master.ReceiveNewLog("Hiding the AI log.");
                                    }
                                }
                            }
                        }
                    }
                    else if (Command[1] == "show")
                    {
                        if (Command.Count > 2)
                        {
                            if (Command[2] == "AI")
                            {
                                if (Command.Count > 3)
                                {
                                    if (Command[3] == "log")
                                    {
                                        HideShowAILog(true);
                                        Master.ReceiveNewLog("Showing the AI log.");
                                    }
                                }
                            }
                        }
                    }
                    else if (Command[1] == "exit")
                    {
                        if (Command.Count > 2)
                        {
                            if (Command[2] == "app" || Command[2] == "application")
                            {
                                Application.Quit();
                                Master.ReceiveNewLog("Exiting Application.");
                            }
                        }
                    }
                    else if (Command[1] == "start")
                    {
                        if (Command.Count > 2)
                        {
                            if (Command[2] == "warframe")
                            {
                                Master.ReceiveNewLog("Starting Warframe.");
                                OpenExternalApplication("steam://rungameid/230410", "Starting: Warframe.");
                            }
                            else if (Command[2] == "team")
                            {
                                if (Command.Count > 4)
                                {
                                    if (Command[3] == "fortress" && Command[4] == "2")
                                    {
                                        Master.ReceiveNewLog("Starting Team Fortress 2.");
                                        OpenExternalApplication("steam://rungameid/440", "Starting: Team Fortress 2.");
                                    }
                                }
                            }
                            else if (Command[2] == "discord")
                            {
                                Master.ReceiveNewLog("Starting Discord");
                                OpenExternalApplication("C:/Users/Mads/AppData/Local/Discord/app-0.0.306/Discord.exe", "Starting: Discord");
                            }
                        }
                    }
                    else if (Command[1] == "set")
                    {
                        if (Command.Count > 2)
                        {
                            if (Command[2] == "timer")
                            {
                                SetTimer();
                            }
                        }
                    }
                }
            }
        }*/
        #endregion
    }

    #region Actions
    private void Command_OpenURL(Dictionary<string, string[]> ActionCommand, string[] AL)
    {
        string url = AL[0];
        string actionName = AL[1];

        GameObject newObject = Instantiate(ActionObject, Vector3.zero, Quaternion.identity, ObjectStorage);

        newObject.AddComponent<Action_OpenURL>();
        Action_OpenURL sAction = newObject.GetComponent<Action_OpenURL>();
        sAction.url = url;

        Action Action = newObject.GetComponent<Action>();
        Action.ActionCommand = ActionCommand;
        Action.Asyncronus = true;
        Action.ActionName = actionName;
        Master.ReceiveNewAction(Action);
    }

    private void HideShowAILog(Dictionary<string, string[]> ActionCommand, string AL)
    {
        GameObject newObject = Instantiate(ActionObject, Vector3.zero, Quaternion.identity, ObjectStorage);

        Action_HideShowAILog sAction = newObject.AddComponent<Action_HideShowAILog>();

        if (AL == "true")
        {
            sAction.ToShow = true;
        }
        else
        {
            sAction.ToShow = false;
        }

        sAction.Log = AI_Log;

        Action Action = newObject.GetComponent<Action>();
        Action.ActionCommand = ActionCommand;
        Master.ReceiveNewAction(Action);
    }

    private void OpenExternalApplication(Dictionary<string, string[]> ActionCommand, string[] AL)
    {
        string fileName = AL[0];
        string actionName = AL[1];

        GameObject newObject = Instantiate(ActionObject, Vector3.zero, Quaternion.identity, ObjectStorage);

        Action_OpenExternalApp sAction = newObject.AddComponent<Action_OpenExternalApp>();
        sAction.AppName = fileName;
        sAction.M = Master;

        Action Action = newObject.GetComponent<Action>();
        Action.ActionCommand = ActionCommand;
        Action.ActionName = actionName;
        Action.Asyncronus = true;
        Master.ReceiveNewAction(Action);
    }

    private void SetTimer(Dictionary<string, string[]> ActionCommand)
    {
        GameObject newObject = Instantiate(ActionObject, Vector3.zero, Quaternion.identity, ObjectStorage);

        Action_Timer sAction = newObject.AddComponent<Action_Timer>();

        Action Action = newObject.GetComponent<Action>();
        Action.ActionCommand = ActionCommand;
    }

    private void MuteUnmute(string AL)
    {
        bool toMute;

        if (AL == "true")
        {
            toMute = true;
        }
        else
        {
            toMute = false;
        }

        IsMuted = toMute;
    }

    private void MainCommandScreen(bool b)
    {
        UI.ChangeToCommandMenu(b);
    }
    #endregion
}

