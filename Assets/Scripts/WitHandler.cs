using System;
using System.Collections;
using System.Collections.Generic;
using Facebook.WitAi.Dictation.Data;
using Oculus.Voice.Dictation;
using UnityEngine;
using UnityEngine.Serialization;

public class WitHandler : MonoBehaviour
{
    public static WitHandler instance = null;

    [FormerlySerializedAs("_dictation")] [SerializeField] private AppDictationExperience dictation;

    private bool _showTranscription = false;

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
    }

    private void OnEnable()
    {
        dictation.DictationEvents.onDictationSessionStarted.AddListener(StartListening);
        dictation.DictationEvents.onDictationSessionStopped.AddListener(StopListening);
        dictation.DictationEvents.onError.AddListener(OnError);
        dictation.DictationEvents.onPartialTranscription.AddListener(LiveTranscriptionHandler);
        dictation.DictationEvents.OnFullTranscription.AddListener(FullTranscriptionHandler);
    }

    private void OnDisable()
    {
        dictation.DictationEvents.onDictationSessionStarted.RemoveListener(StartListening);
        dictation.DictationEvents.onDictationSessionStopped.RemoveListener(StopListening);
        dictation.DictationEvents.onError.RemoveListener(OnError);
        dictation.DictationEvents.onPartialTranscription.RemoveListener(LiveTranscriptionHandler);
        dictation.DictationEvents.OnFullTranscription.RemoveListener(FullTranscriptionHandler);
    }

    void StartListening(DictationSession session)
    {
        _showTranscription = true;
    }

    void StopListening(DictationSession session)
    {
        _showTranscription = false;
    }

    void OnError(string err, string msg)
    {
        Debug.LogWarning("ERROR: " + msg);
    }

    void LiveTranscriptionHandler(string content)
    {
        if (_showTranscription)
            print(content);
    }

    void FullTranscriptionHandler(string content)
    {
        print(content);
    }
}
