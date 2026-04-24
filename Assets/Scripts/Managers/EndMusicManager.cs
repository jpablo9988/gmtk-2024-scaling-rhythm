using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndMusicManager : MonoBehaviour
{
    [SerializeField]
    private Track resultsTrack;

    [SerializeField]
    private Track perfectJingle;

    [SerializeField]
    private Track goodJingle;

    [SerializeField]
    private Track ehJingle;

    [SerializeField]
    private Track poorJingle;

    private AudioManager audioManager;

    void Start()
    {
        //TODO: Put on own script
        audioManager = FindFirstObjectByType<AudioManager>();
    }
    public void PlayEndJingle(ScoreType resultingScore)
    {
        Track jingleToPlay;
        switch (resultingScore)
        {
            case ScoreType.Perfect:
                jingleToPlay = perfectJingle;
                break;
            case ScoreType.Good:
                jingleToPlay = goodJingle;
                break;
            case ScoreType.Meh:
                jingleToPlay = ehJingle;
                break;
            default:
                jingleToPlay = poorJingle;
                break;
        }
        audioManager.PlayMusicTrack(jingleToPlay, false);
        StartCoroutine(Timers.GenericTimer(jingleToPlay.MusicClip.length, () =>
        {
            audioManager.PlayMusicTrack(resultsTrack, false);
        }));
    }
}
