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
 
        private void Update()
        {
            if (Recognizer != null)
            {
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

                Recognizer.DictationResult += (text, confidence) =>
                {
                    ReturnTextFromSpeech(text);
                };
            }
        }

        public void StartListening(Master M)
        {
            Master = M;
            Recognizer = new DictationRecognizer();

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