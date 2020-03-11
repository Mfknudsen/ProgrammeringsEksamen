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
        UnityWebRequest www = new UnityWebRequest(url);
        StartCoroutine(WaitForrequest(www));
    }

    IEnumerator WaitForrequest(UnityWebRequest www)
    {
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
