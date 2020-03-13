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
    private bool HasBeenActivated = false;
    #endregion

    private void Start()
    {
        Master = GetComponent<Master>();

        RecognizerSetup();
    }

    private void LateUpdate()
    {
        if (Recognizer == null && HasBeenActivated)
        {
            HasBeenActivated = false;
            StartCoroutine(Retry());

            RecognizerSetup();

            Recognizer.Start();
        } else if (Recognizer != null)
        {
            if (Recognizer.Status != SpeechSystemStatus.Running) {
                Recognizer.Start();
            }
        }
    }

    public void StartListening()
    {
        Recognizer.Start();
        Master.ReceiveNewSpeechText("Dictation Recognizer is ready!");
    }

    public void RecognizerSetup()
    {
        Recognizer = new DictationRecognizer();
        Recognizer.InitialSilenceTimeoutSeconds = -1;

        Recognizer.DictationResult += (text, confindence) =>
        {
            string T = "";

            for (int i = 0; i < text.Length; i++)
            {
                T += text[i];
            }

            if (T.Length > 0)
            {
                Master.ReceiveNewSpeechText(T);
            }
            Debug.LogFormat("Dictation result: {0}" + text);
        };

        Recognizer.DictationHypothesis += (text) =>
        {
            //Debug.LogWarningFormat("Dictation hypothesis: {0}", text);
        };

        Recognizer.DictationComplete += (completeCause) =>
        {
            HasBeenActivated = true;

            if (completeCause != DictationCompletionCause.Complete)
            {
                Debug.LogErrorFormat("Dictation completed unsuccessfully: {0}.", completeCause);
            }

            Recognizer = null;
        };

        Recognizer.DictationError += (error, result) =>
        {
            Debug.LogErrorFormat("Dictation error: {0}; HResult = {1}.", error, result);
        };
    }

    IEnumerator Retry()
    {
        yield return new WaitForSeconds(1);

        HasBeenActivated = true;
    }

    private void OnApplicationQuit()
    {
        if (Recognizer != null) {
            if (Recognizer.Status == SpeechSystemStatus.Running) {
                Recognizer.Stop();
            }
        }
    }
}