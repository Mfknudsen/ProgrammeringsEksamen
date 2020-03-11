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
    #endregion

    #region Private Data
    //AI resources:
    AudioSource AS;

    //Actions for the AI to atempt to complete. One at a time.
    List<Action> AI_Actions = new List<Action>();
    //Actions for the AI to atempt to complete. All at one time.
    List<Action> Async_AI_Actions = new List<Action>();

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

        UI.AddToLog("Loading Data to Database!");
        Database LoadValues = SaveSystem.LoadDatabase();
        DATA.LoadOldFromJson(LoadValues);
        UI.AddToLog("Loading Complete!");

        VR.StartListening();

        GetStartData();
        COM.AI_Name = AIName;
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
        if (AI_Actions.Count > 0) {
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

        foreach (Action A in Async_AI_Actions)
        {
            if (A.Contenius)
            {
                DATA.Continues_Async_AI_Actions.Add(A);
            }
        }
    }

    public void ReceiveNewSpeechText(string newSpeech)
    {
        COM.FindActionBasedOnText(newSpeech);
    }

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
}
