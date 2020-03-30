/// Summary:
/// This script will handle all object 
/// or similar that is connected to UI 
/// that the user can activly see.

#region Systems:
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
#endregion

public class UIManager : MonoBehaviour
{
    #region Public Data
    [Header("Text Fields:")]
    public TextMeshProUGUI AI_Log;
    public TextMeshProUGUI SpeechLog;
    public TextMeshProUGUI ActionLog;
    [Space]
    [Header("Screens:")]
    public GameObject AI_Screen;
    public GameObject CommandScreen;
    [Space]
    [Header("AI Parts:")]
    public Commands COM;
    public Master M;
    public Database D;
    [Space]
    [Header("Function Values:")]
    public Transform FieldTransform;
    public GameObject FieldObject;
    public float ScrollWheelSpeed = 1f;
    bool isSaving = false;
    #endregion

    #region Private Data
    private List<GameObject> Fields = new List<GameObject>();
    private List<Vector3> FieldPositions = new List<Vector3>();
    private Transform FieldParent;
    float moveDirection = 0;
    #endregion

    private void Start()
    {
        COM = GetComponent<Commands>();
        M = GetComponent<Master>();
        D = GetComponent<Database>();

        AI_Log.text = "";
        AddToLog("Local Json datapath: \n" + Application.persistentDataPath);
        ReplaceSpeechText("Ready for Speech!");

        CommandScreen.SetActive(false);
    }

    private void Update()
    {
        if (!isSaving)
            CommandMenu();
    }

    public void AddToLog(string Addition)
    {
        int hour = System.DateTime.Now.Hour;
        int min = System.DateTime.Now.Minute;
        int second = System.DateTime.Now.Second;
        string Time = "TIME:[" + hour + ":" + min + ":" + second + "]";
        AI_Log.text = Time + "\n" + Addition + "\n \n" + AI_Log.text;
    }

    public void ReplaceSpeechText(string newText)
    {
        if (newText.Length > 0)
        {
            string First = char.ToUpper(newText[0]) + newText.Substring(1);
            SpeechLog.text = First;
        }
    }

    public void UpdateActionLog(List<Action> NonAsync, List<Action> Async)
    {
        string AsyncText = "";

        if (Async.Count > 1)
        {
            AsyncText = "Async Actions:";
        }
        else if (Async.Count == 1)
        {
            AsyncText = "Async Action:";
        }

        foreach (Action W in Async)
        {
            string T = W.ActionName;

            AsyncText += "\n" + T;
        }

        if (NonAsync.Count > 0)
        {
            string NonAsyncText = "";
            NonAsyncText = NonAsync[0].ActionName;

            ActionLog.text = NonAsyncText + "\n\n" + AsyncText;
        }
        else
        {
            ActionLog.text = AsyncText;
        }
    }

    public void ChangeToCommandMenu(bool b)
    {
        if (b)
        {
            AI_Screen.SetActive(false);
            CommandScreen.SetActive(true);

            SetupFields();
            Debug.Log("Setting up the fields");
        }
        else
        {
            SaveCommandList();

            AI_Screen.SetActive(true);
            CommandScreen.SetActive(false);


            List<GameObject> toDestroy = new List<GameObject>();
            toDestroy = Fields;

            foreach (GameObject G in toDestroy)
            {
                Fields.Remove(G);
                Destroy(G);
            }
        }
    }

    public void SetupFields()
    {
        isSaving = false;
        moveDirection = 0;

        int n = 0;

        foreach (Dictionary<string, string[]> temp in COM.CommandList)
        {
            if (temp["commandType"][1] == "true")
            {
                GameObject obj = Instantiate(FieldObject, Vector3.zero, Quaternion.identity, FieldTransform) as GameObject;
                obj.transform.localPosition = FieldObject.transform.localPosition + new Vector3(420, -200 * n, 0);

                obj.SetActive(true);
                obj.GetComponent<CommandField>().GetStartData(temp, n);

                Fields.Add(obj);
                n++;
            }
        }
    }

    private void CommandMenu()
    {
        float wheel = Input.GetAxis("Mouse ScrollWheel");
        moveDirection = wheel * ScrollWheelSpeed * Time.deltaTime;

        if (moveDirection <= 0)
            moveDirection = 0;

        foreach (GameObject G in Fields)
        {
            G.transform.position = G.transform.position + new Vector3(0, moveDirection, 0);
        }
    }

    public void Save()
    {
        List<string> newText = new List<string>();
        newText.Add("computer");
        newText.Add("switch");
        newText.Add("to");
        newText.Add("main");
        newText.Add("screen");
        COM.FindActionBasedOnText(newText);
    }

    public void SaveCommandList()
    {
        isSaving = true;
        List<Dictionary<string, string[]>> toSave = new List<Dictionary<string, string[]>>();

        foreach (Dictionary<string, string[]> toRemember in COM.CommandList)
        {
            if (toRemember["commandType"][1] == "false")
            {
                toSave.Add(toRemember);
            }
        }

        foreach (GameObject F in Fields)
        {
            CommandField Field = F.GetComponent<CommandField>();

            bool isSaved = false;

            foreach (Dictionary<string, string[]> d in toSave)
            {
                if (d["command"][0] == Field.commandText)
                {
                    isSaved = true;
                }
            }

            if (!isSaved)
            {
                if ((Field.commandText != "" || Field.commandText != " ") && Field.commandInputText.Count > 0)
                {
                    Dictionary<string, string[]> newCommand = new Dictionary<string, string[]>();

                    newCommand.Add("command", new string[] { Field.commandText });
                    newCommand.Add("commandType", new string[] { Field.commandTypeText, "true" });
                    newCommand.Add("commandInput", Field.commandInputText.ToArray());

                    toSave.Add(newCommand);
                }
            }
        }

        D.CommandList = toSave;
        COM.CommandList = D.CommandList;
    }

    public void AddCommandList()
    {
        GameObject obj = Instantiate(FieldObject, Vector3.zero, Quaternion.identity, FieldTransform);
        obj.transform.localPosition = FieldObject.transform.localPosition + new Vector3(420, -200 * Fields.Count, 0);
        obj.SetActive(true);
        obj.GetComponent<CommandField>().ListNumber = -1;
        Fields.Add(obj);
    }
}
