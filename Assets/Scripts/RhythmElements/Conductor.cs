using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Tracks the timing (in beats and seconds) of a Rhythm Track. 
/// </summary>
public class Conductor : MonoBehaviour
{

    [Header("Music Trackers. ")]
    [Tooltip("The current position in seconds of a rhythm track. ")]
    [ReadOnly]
    [SerializeField]
    private float _positionInSeconds;
    [Tooltip("The current position in beats of a rhythm track. Each beat is the equivalent of a quarter note in 4/4")]
    [ReadOnly]
    [SerializeField]
    private float _positionInBeats;
    [Tooltip("The local beats per minute of a rhythm track. Can change in case of a tempo change. ")]
    [ReadOnly]
    [SerializeField]
    private float _localBPM;
    [Tooltip("Is the conductor currently tracking a rhythm track? ")]
    [ReadOnly]
    [SerializeField]
    private bool _isTracking;

    private float _dspTime;
    private float _secondsPassedSinceLoop = 0;
    private float _localBPS;
    private bool _loopable = false;
    private AudioSource _source;
    private RhythmTrack _musicTrack;

    public float CurrentBeat { get { return _positionInBeats;  } private set { _positionInBeats = value; } }
    /// <summary>
    /// Set current audio source used by the scene for Rhythm Tracks. 
    /// </summary>
    /// <param name="source"></param>
    public void SetSourceReference(AudioSource source)
    {
        this._source = source;
    }
    /// <summary>
    /// Start tracking a Rhythm Track.
    /// </summary>
    /// <param name="musicTrack"></param>
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
        if (_isTracking)
        {
            if (!_source.isPlaying)
            {
                _isTracking = false;
                return;
            }
            _positionInSeconds = (float)(AudioSettings.dspTime - _dspTime - _musicTrack.OffsetUntilStart);
            _positionInBeats += (_positionInSeconds - _secondsPassedSinceLoop) / _localBPS;
            _secondsPassedSinceLoop = _positionInSeconds;
        }
    }

}
