/// Summary:
/// This script is a action the AI can take 
/// when giving the command to open a website.

#region Systems
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
#endregion

public class Action_OpenURL : MonoBehaviour
{
    #region Public Data
    public Action ActionHandler;
    public string url;
    #endregion

    private void Start()
    {
        ActionHandler = GetComponent<Action>();
        ActionHandler.Asyncronus = true;
    }

    private void StartNow()
    {
        ActionHandler.IsStarted = true;
        StartCoroutine(WaitForrequest());
    }

    private void UpdateNow()
    {
    }

    IEnumerator WaitForrequest()
    {
        Application.OpenURL(url);
        UnityWebRequest www = new UnityWebRequest(url);

        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.LogError(www.error);
        }
        else
        {
            ActionHandler.IsDone = true;
        }
    }
}
