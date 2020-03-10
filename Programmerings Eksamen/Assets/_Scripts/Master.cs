﻿#region Systems:
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
    #endregion

    #region Private Data
    //AI resources:
    AudioSource AS;

      //Actions for the AI to atempt to complete. One at a time.
    List<object> AI_Actions = new List<object>();
      //Actions for the AI to atempt to complete. All at one time.
    List<object> Async_AI_Actions = new List<object>();

    //AI information:
    string AIName;

    //User information:
    string UserName;
    #endregion

    private void Start()
    {
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

            if(EAC == null)
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

        Debug.Log(VR);
        VR.StartListening();

        //SaveSystem.LoadDatabase();
    }

    private void FixedUpdate()
    {
        UpdateCurrentAction();
    }

    private void UpdateCurrentAction()
    {
        object ObjectToRead = null;

        foreach (object o in AI_Actions)
        {
            if (ObjectToRead == null) {
                ObjectToRead = o;
                break;
            }
        }
    }

    public void ReceiveNewSpeechText(string newSpeech)
    {
        COM.FindActionBasedOnText(newSpeech);
    }

    private void OnApplicationQuit()
    {
        //SaveSystem.SaveDatabase();
    }
}
