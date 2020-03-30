/// Summary:
/// This script will be the main part of the AI and will hold and direct all other parts.
/// It will tell the other scripts when to sertain actions and when it will receive input 
/// or returns from the script that is meant for other scripts then it will pass it on to 
/// right place.

#region Systems:
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#endregion

public class Master : MonoBehaviour
{
    #region Public Data
    [Header("Secondary Tool Scripts:")]
    public Database DATA;
    public Commands COM;
    public UIManager UI;
    public VoiceRecognition VR;
    [Space]
    [Header("Communications Values:")]
    public bool DatabaseIsLoaded = false;

    //Actions for the AI to atempt to complete. One at a time.
    [HideInInspector]
    public List<Action> AI_Actions = new List<Action>();
    //Actions for the AI to atempt to complete. All at one time.
    [HideInInspector]
    public List<Action> Async_AI_Actions = new List<Action>();
    #endregion

    #region Private Data
    //AI information:
    string AIName;

    //User information:
    string UserName;
    #endregion

    private void Start()
    {
        Application.targetFrameRate = 60;

        //Getting all the required components
        if (DATA == null)
        {
            DATA = GetComponent<Database>();

            if (DATA == null)
            {
                DATA = gameObject.AddComponent<Database>();
            }
        }
        if (COM == null)
        {
            COM = GetComponent<Commands>();

            if (COM == null)
            {
                COM = gameObject.AddComponent<Commands>();
            }
        }
        if (UI == null)
        {
            UI = GetComponent<UIManager>();

            if (UI == null)
            {
                UI = gameObject.AddComponent<UIManager>();
            }
        }
        if (VR == null)
        {
            VR = GetComponent<VoiceRecognition>();

            if (VR == null)
            {
                VR = gameObject.AddComponent<VoiceRecognition>();
            }
        }

        ReceiveNewLog("Attempting to load from Json!");
        Database tempLoad = SaveSystem.LoadDatabase();
        DATA.LoadOldFromJson(tempLoad, COM);
        COM.StartNow(this, DATA, UI);
        VR.StartListening();
    }

    private void FixedUpdate()
    {
        UpdateCurrentAction();
        UpdateAsyncActions();

        UI.UpdateActionLog(AI_Actions, Async_AI_Actions);
    }

    private void UpdateCurrentAction()
    {
        if (AI_Actions.Count > 0)
        {
            Action ActionToRead = AI_Actions[0];

            if (ActionToRead.IsStarted == false)
            {
                ActionToRead.StartAction();
            }
            else if (ActionToRead.IsDone == false)
            {
                ActionToRead.UpdateAction();
            }
            else
            {
                AI_Actions.Remove(ActionToRead);
                ActionToRead.DestroyObject();
            }

            foreach (Action A in AI_Actions)
            {
                if (A.Contenius)
                {
                    DATA.Continues_AI_Actions.Add(A);
                }
            }
        }
    }

    private void UpdateAsyncActions()
    {
        foreach (Action o in Async_AI_Actions)
        {
            if (o.IsStarted == false)
            {
                o.StartAction();
            }
            else if (o.IsDone == false)
            {
                o.UpdateAction();
            }
            else
            {
                Async_AI_Actions.Remove(o);
                o.DestroyObject();
            }
        }
    }

    public void ReceiveNewSpeechText(string newSpeech)
    {
        string[] Wish = newSpeech.Split(' ');
        List<string> Command = new List<string>();
        List<string> checkedCommand = new List<string>();

        checkedCommand = CheckCommand(Wish);

        for (int i = 0; i < checkedCommand.Count; i++)
        {
            string Word = checkedCommand[i];
            if (Command.Count > 0)
            {
                Command.Add(Word);
            }
            else
            {
                if (Word == "computer")
                {
                    Command.Add(Word);
                }
            }
        }

        string textToShow = "";
        List<string> lines = new List<string>();
        for (int i = 0; i < checkedCommand.Count; i++)
        {
            string W = checkedCommand[i];
            string w = "";
            if (W == "warframe")
            {
                w = "Warframe";
            }
            else if (W == "2")
            {
                if (lines[i - 2] == "team" && lines[i - 1] == "fortress")
                {
                    lines[i - 2] = "Team";
                    lines[i - 1] = "Fortress";
                }
            }
            else
            {
                w = W;
            }

            if (w != "")
            {
                lines.Add(w);
            }
        }
        foreach (string w in lines)
        {
            textToShow += w + " ";
        }

        UI.ReplaceSpeechText(textToShow);
        COM.FindActionBasedOnText(Command);
    }

    public void ReceiveNewLog(string newLog)
    {
        UI.AddToLog(newLog);
    }

    public void ReceiveNewAction(Action A)
    {
        if (A.Asyncronus)
        {
            Async_AI_Actions.Add(A);
        }
        else
        {
            AI_Actions.Add(A);
        }
    }

    private List<string> CheckCommand(string[] Wish)
    {
        List<string> checkedCommand = new List<string>();

        /// Note:
        /// The Speech to Text is not perfect and so will sometimes have troulbe understanding certain words or lines.
        /// This for loop will go through the text from the Recognizer and look for words or lines that I know
        /// is meant to mean something else and will it change it.
        /// Example: Speech = "Computer unmute" -> text = 'computer on mute' -> correction = 'computer unmute'
        for (int i = 0; i < Wish.Length; i++)
        {
            string W = Wish[i];

            if (W == "mute")
            {
                if (Wish[i - 1] == "on")
                {
                    checkedCommand[i - 1] = "unmute";
                }
                else
                {
                    checkedCommand.Add(W);
                }
            }
            else if (W == "lock" || W == "locked" || W == "look" || W == "doc")
            {
                if (Wish[i - 1] == "AI")
                {
                    if (Wish[i - 2] == "show" || Wish[i - 2] == "hide")
                    {
                        checkedCommand.Add("log");
                    }
                }
            }
            else if (W == "it")
            {
                if (Wish[i - 1] == "read")
                {
                    checkedCommand[i - 1] = "reddit";
                }
            }
            else if (W == "frame")
            {
                if (Wish[i - 1] == "war")
                {
                    Wish[i - 1] = "warframe";
                }
            }
            else if (W == "")
            {

            }
            else
            {
                checkedCommand.Add(W);
            }
        }

        return checkedCommand;
    }

    private IEnumerator GetDataFromJson()
    {
        if (!DatabaseIsLoaded)
        {
            yield return null;
        }


    }

    private void OnApplicationQuit()
    {
        foreach (Action A in DATA.Continues_AI_Actions)
        {
        }

        foreach (Action A in DATA.Continues_Async_AI_Actions)
        {
        }

        UI.AddToLog("Saving Database to Json file");
        SaveSystem.SaveData(DATA);
        UI.AddToLog("Saving Complete");
    }
}
