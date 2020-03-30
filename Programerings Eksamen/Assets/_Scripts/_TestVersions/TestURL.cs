using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class TestURL : MonoBehaviour
{
    private string URL = "";

    public void changeURL(string url)
    {
        URL = "https://www."+url+".com/";
    }

    public void OpenURL()
    {
        StartCoroutine(WaitForRequest());
    }

    IEnumerator WaitForRequest()
    {
        Application.OpenURL(URL);

        UnityWebRequest www = new UnityWebRequest(URL);

        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.LogError(www.error);
        }
        else
        {
            Debug.Log("Url has been opened!");
        }
    }
}
