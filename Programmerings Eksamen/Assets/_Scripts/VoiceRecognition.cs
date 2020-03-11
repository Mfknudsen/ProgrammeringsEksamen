#region Systems:
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;
#endregion

public class VoiceRecognition : MonoBehaviour
{
    #region Private Data
    private DictationRecognizer Recognizer;
    private Master Master;
    #endregion

    private void Start()
    {
        Master = GetComponent<Master>();
    }

    public void StartListening()
    {
        Recognizer = new DictationRecognizer();

        Recognizer.DictationResult += (text, confindence) =>
        {
            Master.ReceiveNewSpeechText(text);
            Debug.LogFormat("Dictation result: {0}" + text);
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