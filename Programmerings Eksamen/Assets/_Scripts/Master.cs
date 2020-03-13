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
    public ExternalAppController EAC;
    public UIManager UI;
    public VoiceRecognition VR;
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
    //AI resources:
    AudioSource AS;

    //AI information:
    string AIName;

    //User information:
    string UserName;
    #endregion

    private void Awake()
    {
        Application.targetFrameRate = 60;
    }

    private void Start()
    {
        //Getting all the required components
        AS = GetComponent<AudioSource>();
        if (AS == null)
        {
            AS = gameObject.AddComponent<AudioSource>();
        }

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
        if (EAC == null)
        {
            EAC = GetComponent<ExternalAppController>();

            if (EAC == null)
            {
                EAC = gameObject.AddComponent<ExternalAppController>();
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
            Debug.Log("VR is null");
            VR = GetComponent<VoiceRecognition>();

            if (VR == null)
            {
                VR = gameObject.AddComponent<VoiceRecognition>();
            }
        }

        /* UI.AddToLog("Loading Data to Database!");
         Database LoadValues = SaveSystem.LoadDatabase();

         if (LoadValues != null)
         {
             DATA.LoadOldFromJson(LoadValues);
             UI.AddToLog("Loading Complete!");
         }
         else
         {
             UI.AddToLog("Loading Failed!");
         }
         */

        VR.StartListening();

        //GetStartData();
        //COM.AI_Name = AIName;
    }

    private void FixedUpdate()
    {
        UpdateCurrentAction();
        UpdateAsyncActions();
    }

    private void GetStartData()
    {
        AIName = DATA.AI_Name;
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
            else
            {
                checkedCommand.Add(W);
            }
        }

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
        foreach (string W in checkedCommand)
        {
            textToShow += W + " ";
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

    /*
    private void OnApplicationQuit()
    {
        UpdateCurrentAction();
        UpdateAsyncActions();

        foreach (Action A in DATA.Continues_AI_Actions)
        {
            A.IsStarted = false;
        }

        foreach (Action A in DATA.Continues_Async_AI_Actions)
        {
            A.IsStarted = false;
        }

        UI.AddToLog("Saving Database to Json file");
        SaveSystem.SaveData(DATA);
        UI.AddToLog("Saving Complete");
    }
    */
}
