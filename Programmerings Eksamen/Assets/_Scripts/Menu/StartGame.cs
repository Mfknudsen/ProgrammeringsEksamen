#region Systems
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
#endregion

public class StartGame : MonoBehaviour
{
    public void StartFreshGame()
    {
        StartCoroutine(LoadMainAsync());
    }

    IEnumerator LoadMainAsync()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Main");

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
