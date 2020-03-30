using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CommandField : MonoBehaviour
{
    #region Public Data
    public string commandText = "";
    public string commandTypeText = "";
    public List<string> commandInputText = new List<string>();
    public TextMeshProUGUI commandPlaceHolder;
    public TextMeshProUGUI inputPlaceHolder;
    public Vector3 StartPos;
    public GameObject[] inputSources;
    public int ListNumber = -1;
    #endregion

    private void Start()
    {
        GetNewCommandType(0);
    }

    public void GetStartData(Dictionary<string, string[]> input, int i)
    {
        ListNumber = i;

        int n = 0;

        foreach (string key in input.Keys)
        {
            if (key == "command")
            {
                string t = "";

                foreach (string text in input[key])
                {
                    if (t == "")
                    {
                        t = text;
                    }
                    else
                    {
                        t += text;
                    }
                }

                inputSources[n].GetComponent<TMP_InputField>().text = t;
                GetNewCommand(t);
            }
            else if (key == "commandType")
            {
                if (input[key][0] == "Open Url")
                {
                    inputSources[n].GetComponent<TMP_Dropdown>().value = 0;
                    GetNewCommandType(0);
                }
                else if (input[key][0] == "Open External App")
                {
                    inputSources[n].GetComponent<TMP_Dropdown>().value = 1;
                    GetNewCommandType(1);
                }
            }
            else
            {
                string t = "";

                foreach (string s in input[key])
                {
                    if (t == "")
                    {
                        t = s;
                    }
                    else
                    {
                        t += "," + s;
                    }
                }

                inputSources[n].GetComponent<TMP_InputField>().text = t;
                GetNewCommandInput(t);
            }
            n++;
        }
    }

    public Dictionary<string, string[]> SaveCommand()
    {
        if (commandText != "")
        {
            Dictionary<string, string[]> tempCommand = new Dictionary<string, string[]>();
            tempCommand.Add("command", new string[] { commandText });
            tempCommand.Add("commandType", new string[] { commandTypeText, "true" });
            tempCommand.Add("commandInput", commandInputText.ToArray());

            return tempCommand;
        }
        else
        {
            return null;
        }
    }

    public void GetNewCommand(string inputText)
    {
        commandText = inputText.ToLower();
    }

    public void GetNewCommandType(int i)
    {
        if (i == 0)
        {
            commandTypeText = "Open Url";

            commandPlaceHolder.text = "Spoken Command, Example: computer open youtube";
            inputPlaceHolder.text = "Input For The Command \nExample: www.youtube.com \nThe character ',' is used to seperate the different input values.";
        }
        else if (i == 1)
        {
            commandTypeText = "Open External App";

            commandPlaceHolder.text = "Spoken Command, Example: computer open discord";
            inputPlaceHolder.text = "Input For The Command \nExample: C:/Users/User/AppData/Roaming/Microsoft/Windows/Start Menu/Programs/Discord/Discord.exe \nThe character ',' is used to seperate the different input values.";
        }
    }

    public void GetNewCommandInput(string inputText)
    {
        string[] splitText = inputText.Split(',');

        foreach (string t in splitText)
        {
            commandInputText.Add(t);
        }
    }
}
