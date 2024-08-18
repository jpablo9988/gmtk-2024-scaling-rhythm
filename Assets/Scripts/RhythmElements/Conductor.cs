using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Tracks the timing (in beats and seconds) of a Rhythm Track. 
/// </summary>
public class Conductor : MonoBehaviour
{
    [Header("Dependencies ")]
    [SerializeField]
    private PatternManager _patternManager;
    [Header("Music Trackers(ReadOnly)")]
    [Tooltip("The current position in seconds of a rhythm track. ")]
    [ReadOnly]
    [SerializeField]
    private float _positionInSeconds;
    [Tooltip("The current position in beats of a rhythm track. Each beat is the equivalent of a quarter note in 4/4")]
    [ReadOnly]
    [SerializeField]
    private float _totalPositionInBeats;
    [Tooltip("The current position in beats of a rhythm track tracking a loop. If it isn't loopable, it's equal to the total. ")]
    [ReadOnly]
    [SerializeField]
    private float _positionInBeatsLoop;
    [Tooltip("The local beats per minute of a rhythm track. Can change in case of a tempo change. ")]
    [ReadOnly]
    [SerializeField]
    private float _localBPM;
    [Tooltip("Current position of the track from 0 - 1. Accounts for loops. ")]
    [ReadOnly]
    [SerializeField]
    private float _positionInAnalog;
    [Tooltip("Is the conductor currently tracking a rhythm track? ")]
    [ReadOnly]
    [SerializeField]
    private bool _isTracking;

    private float _dspTime;
    private float _secondsPassedSinceUpdateLoop = 0;
    private float _localBPS;
    private int _completedLoops = 0;
    private float _beatsPerLoop = 0;
    private AudioSource _source;
    private RhythmTrack _musicTrack;

    public float CurrentBeat { get { return _positionInBeatsLoop;  } private set { _positionInBeatsLoop = value; } }
    public float PositionInAnalog { get { return _positionInAnalog;  } private set { _positionInAnalog = value; } }
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
        _totalPositionInBeats = 0;
        _localBPM = musicTrack.BPM;
        _localBPS = 60f / _localBPM;
        _beatsPerLoop = musicTrack.BPM * (musicTrack.MusicClip.length / 60);
        this._musicTrack = musicTrack;
        // --- set input pattern map: ---- //
        _patternManager.SetPlayableMap(this, musicTrack.Map);
        _isTracking = true;
    }
    private void Update()
    {
        if (_isTracking)
        {
            if (!_source.isPlaying)
            {
                _isTracking = false;
                _patternManager.StopPlayableMap();
                return;
            }
            _positionInSeconds = (float)(AudioSettings.dspTime - _dspTime - _musicTrack.OffsetUntilStart);
            _totalPositionInBeats += (_positionInSeconds - _secondsPassedSinceUpdateLoop) / _localBPS;
            _secondsPassedSinceUpdateLoop = _positionInSeconds;
            //Calculations for Loops:
            if (_totalPositionInBeats >= (_completedLoops + 1) * _beatsPerLoop && _musicTrack.IsLoopable)
            {
                _completedLoops++;
                _localBPM = _musicTrack.BPM;
            }
            if (_musicTrack.IsLoopable) _positionInBeatsLoop = _totalPositionInBeats - _completedLoops * _beatsPerLoop;
            else _positionInBeatsLoop = _totalPositionInBeats; 
            _positionInAnalog = _positionInBeatsLoop / _beatsPerLoop;
        }
    }

}
