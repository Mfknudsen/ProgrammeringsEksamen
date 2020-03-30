using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action_HideShowAILog : MonoBehaviour
{
    public Action ActionHandler;
    public bool ToShow = true;
    public GameObject Log;

    private void StartNow()
    {
        ActionHandler = GetComponent<Action>();

        if (ActionHandler != null)
        {
            ActionHandler.IsStarted = true;
        }

        if (Log != null)
        {
            Log.SetActive(ToShow);
        }
    }

    private void UpdateNow()
    {
        if (Log != null)
        {
            if (Log.activeSelf == ToShow)
            {
                ActionHandler.IsDone = true;
            }
            else
            {
                Log.SetActive(ToShow);
            }
        }
        else
        {
            ActionHandler.IsDone = true;
        }
    }
}
