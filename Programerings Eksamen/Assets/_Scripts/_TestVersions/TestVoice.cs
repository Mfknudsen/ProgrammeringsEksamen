#region Systems:
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;
using TMPro;
using UnityEngine.UI;
#endregion

public class TestVoice : MonoBehaviour
{
    public TextMeshProUGUI TextField;
    private DictationRecognizer Recognizer;

    public void StartNow()
    {
        Recognizer = new DictationRecognizer();
        Debug.Log(Recognizer);

        TextField.text = "Ready For Command!";

        StartListening();
    }

    public void StartListening()
    {
        Recognizer.DictationResult += (text, confindence) =>
        {
            TextField.text = text;
            //Debug.LogFormat("Dictation result: {0}" + text);
        };

        Recognizer.DictationHypothesis += (text) =>
        {
            Debug.LogFormat("Dictation hypothesis: {0}", text);
        };

        Recognizer.DictationComplete += (completeCause) =>
        {
            if (completeCause != DictationCompletionCause.Complete)
            {
                Debug.LogErrorFormat("Dictation completed unsuccessfully: {0}.", completeCause);
            }
        };

        Recognizer.DictationError += (error, result) =>
        {
            Debug.LogErrorFormat("Dictation error: {0}; HResult = {1}.", error, result);
        };

        Recognizer.Start();
    }

    private void OnApplicationQuit()
    {
        Recognizer.Stop();
    }

}
