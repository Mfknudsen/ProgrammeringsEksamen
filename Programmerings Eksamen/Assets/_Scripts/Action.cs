#region Systems:
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#endregion

public class Action : MonoBehaviour
{
    public bool Asyncronus = false;
    public bool Contenius = false;
    public bool IsStarted = false;
    public bool IsDone = false;

    public void StartAction()
    {
        if (!IsStarted)
        {
            SendMessage("StartNow");
        }
    }

    public virtual void UpdateAction()
    {
        if (IsStarted && !IsDone)
        {
            SendMessage("UpdateNow");
        }
    }

    public void DestroyObject()
    {
        Destroy(gameObject);
    }
}
