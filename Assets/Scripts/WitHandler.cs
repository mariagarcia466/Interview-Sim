using System;
using Facebook.WitAi.Dictation.Data;
using Oculus.Voice.Dictation;
using UnityEngine;
using UnityEngine.Serialization;

public class WitHandler : MonoBehaviour
{
    public static WitHandler instance = null;
    public string tempTranscript;
    public string fullTranscript;

    [FormerlySerializedAs("_dictation")] [SerializeField]
    private AppDictationExperience dictation;

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
        Debug.Log("We do a stuff maybe");
        dictation.DictationEvents.onDictationSessionStarted.AddListener(StartListening);
        dictation.DictationEvents.onDictationSessionStopped.AddListener(StopListening);
        dictation.DictationEvents.onError.AddListener(OnError);
        dictation.DictationEvents.onPartialTranscription.AddListener(LiveTranscriptionHandler);
        dictation.DictationEvents.OnFullTranscription.AddListener(FullTranscriptionHandler);

        tempTranscript = "";
        fullTranscript = "";
        
        dictation.Activate();
    }

    private void OnDisable()
    {
        dictation.DictationEvents.onDictationSessionStarted.RemoveListener(StartListening);
        dictation.DictationEvents.onDictationSessionStopped.RemoveListener(StopListening);
        dictation.DictationEvents.onError.RemoveListener(OnError);
        dictation.DictationEvents.onPartialTranscription.RemoveListener(LiveTranscriptionHandler);
        dictation.DictationEvents.OnFullTranscription.RemoveListener(FullTranscriptionHandler);
        
        dictation.Deactivate();
    }

    void StartListening(DictationSession session)
    {
        _showTranscription = true;
    }

    void StopListening(DictationSession session)
    {
        _showTranscription = false;
        dictation.Activate();
    }

    void OnError(string err, string msg)
    {
        Debug.LogWarning("ERROR: " + msg);
    }

    private string lastContent = "";
    void LiveTranscriptionHandler(string content)
    {
        if (lastContent.Split(" ").Length < content.Split(" ").Length)
        {
            lastContent = content;
        } else if (lastContent.Split(" ").Length > content.Split(" ").Length)
        {
            fullTranscript += lastContent + " ";
            lastContent = content;
        }

        tempTranscript = content;
        Debug.Log(content);
    }

    void FullTranscriptionHandler(string content)
    {
        //fullTranscript += content;
        Debug.Log(content);
        dictation.Activate();
    }

    private void Start()
    {
        Awake();
    }
}