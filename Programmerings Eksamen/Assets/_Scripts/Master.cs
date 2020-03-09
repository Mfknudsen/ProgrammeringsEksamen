#region Systems:
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#endregion

#region Custom Functions:
using AI_Commands;
using AI_Database;
using AI_External;
#endregion

public class Master : MonoBehaviour
{
    #region Public Data
    Database DATA;
    Commands COM;
    ExternalAppController EAC;
    #endregion

    #region Private Data
    //AI resources:
    AudioSource AS;
      //Actions for the AI to atempt to complete. One at a time.
    List<Dictionary<string, object>> AI_Actions = new List<Dictionary<string, object>>();
      //Actions for the AI to atempt to complete. All at one time.
    List<Dictionary<string, object>> Async_AI_Actions = new List<Dictionary<string, object>>();

    //AI information:
    string AIName;

    //User information:
    string UserName;
    #endregion

    void Start()
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

        EAC.Launch();

        AIName = DATA.GET_AI_Name();

        Debug.Log(AIName);
    }

    void FixedUpdate()
    {
        UpdateCurrentAction();
    }

    private void UpdateCurrentAction(Object ActionInformation)
    {

    }
}
