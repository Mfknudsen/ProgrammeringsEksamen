#region Systems:
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;
#endregion

namespace AI_VoiceReg {
    public class VoiceRecognition : MonoBehaviour
    {
        private DictationRecognizer Recognizer;
        private Master Master;

        public void StartListening(Master M)
        {
            Master = M;
            Recognizer = new DictationRecognizer();

            Recognizer.DictationResult += (text, confindence) =>
            {
                ReturnTextFromSpeech(text);
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

        public void ReturnTextFromSpeech(string newText)
        {
            Master.ReceiveNewSpeechText(newText);
        }
    }
}