using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTwoProgression : MonoBehaviour
{
    [SerializeField]
    private AudioManager _audioManager;
    [SerializeField]
    private Track musicTrack;
    [SerializeField]
    private RhythmTrack rhythmTrack;
    [SerializeField]
    private CatcherScript catcher;
    void Start()
    {
        // -- testing audio -- //
        ScoreTally.ResetScore();
        _audioManager.PlayMusicTrack(musicTrack, true);
        StartCoroutine(Timers.GenericTimer(musicTrack.MusicClip.length, () =>
        {
            _audioManager.PlayMusicTrack(rhythmTrack, true);
            catcher.SetCatcherIdleSpeed(rhythmTrack.BPM / 60);
        }));
    }
}
