using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class Action_OpenExternalApp : MonoBehaviour
{
    public string AppName = "";
    public Master M;
    private Process P;
    private Action A;

    private void StartNow()
    {
        A = GetComponent<Action>();

        LoadApplication();
    }

    private void UpdateNow()
    {
        P.WaitForExit();

        UnityEngine.Debug.Log(P.ExitCode);
        
        if (P.ExitCode > 0)
        {
            A.IsDone = true;
        }
    }

    private void LoadApplication()
    {
        ProcessStartInfo StartInfo = new ProcessStartInfo();

        StartInfo.FileName = AppName;

        P = Process.Start(StartInfo);

        A.IsStarted = true;
    }
}
