using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Conductor : MonoBehaviour
{

    [Header("Debug Trackers")]
    [ReadOnly]
    [SerializeField]
    private float _positionInSeconds;
    [ReadOnly]
    [SerializeField]
    private float _positionInBeats;
    [ReadOnly]
    [SerializeField]
    private float _localBPM;
    [ReadOnly]
    [SerializeField]
    private bool _isTracking;

    private float _dspTime;
    private float _secondsPassedSinceLoop = 0;
    private float _localBPS;
    private AudioSource _source;
    private RhythmTrack _musicTrack;
    public void SetSourceReference(AudioSource source)
    {
        this._source = source;
    }
    public void ConductMusicTrack(RhythmTrack musicTrack)
    {
        // --- reset variables to default ! ---- //
        _dspTime = (float)AudioSettings.dspTime;
        _positionInBeats = 0;
        _localBPM = musicTrack.BPM;
        _localBPS = 60f / _localBPM;
        this._musicTrack = musicTrack;
        _isTracking = true;
    }
    private void Update()
    {
        if (_isTracking && _source.isPlaying)
        {
            _positionInSeconds = (float)(AudioSettings.dspTime - _dspTime - _musicTrack.OffsetUntilStart);
            _positionInBeats += (_positionInSeconds - _secondsPassedSinceLoop) / _localBPS;
            _secondsPassedSinceLoop = _positionInSeconds;
        }
    }

}
