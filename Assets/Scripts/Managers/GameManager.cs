using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private AudioManager _audioManager;
    [SerializeField]
    private Track musicTrack;
    [SerializeField]
    private Track rhythmTrack;
    void Start()
    {
        // -- testing audio -- //
        _audioManager.PlayMusicTrack(musicTrack, true);
        StartCoroutine(Timers.GenericTimer(1.0f, () =>
        {
            _audioManager.PlayMusicTrack(rhythmTrack, true);
        }));
    }
}
