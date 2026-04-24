using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialAudioNode : ITutorialNode
{
    [Header("Audio Specific Track")]
    [SerializeField]
    private AudioManager audioManager;
    [SerializeField]
    private Track audioTrack;
    [SerializeField]
    private bool fadeInto = false;
    [SerializeField]
    private bool fadeOut = false;
    [SerializeField]
    private bool keepPlaying = false;
    [Header("Rhythm Track Specifics")]
    [SerializeField]
    private PatternManager patternManager;

    void Start()
    {
        audioManager = GetDependant(audioManager);
        patternManager = GetDependant(patternManager);
    }
    public override void ExecuteNode(Action onEndExecute)
    {
        audioManager.PlayMusicTrack(audioTrack, fadeInto, patternManager);
        base.ExecuteNode(onEndExecute);
    }
    public override void CleanNode()
    {
        if (!keepPlaying)
        {
            audioManager.StopCurrentTrack(fadeOut);
        }
        if (patternManager != null)
        {
            if (patternManager.IsTracking) patternManager.StopPlayableMap();
        }
        base.CleanNode();
    }
}
