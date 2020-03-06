#region Systems
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManager;
#endregion

public class LoadGame : MonoBehaviour
{
    public void LoadOldGame()
    {
        //StartCoroutine(LoadOldAsync());
    }

    IEnumerator LoadOldAsync()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Main");

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
