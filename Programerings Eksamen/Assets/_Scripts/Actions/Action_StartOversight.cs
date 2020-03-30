using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action_StartOversight : MonoBehaviour
{
    #region Public Data
    public Action ActionHandler;
    #endregion

    private void Start()
    {
        ActionHandler = GetComponent<Action>();
        ActionHandler.Asyncronus = true;
        ActionHandler.Contenius = true;
    }

    private void StartNow()
    {

    }

    private void UpdateNow()
    {
        
    }
}
