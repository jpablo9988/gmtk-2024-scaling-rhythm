using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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
    [SerializeField]
    private float _positionInSeconds;
    [Tooltip("The current position in beats of a rhythm track. Each beat is the equivalent of a quarter note in 4/4")]
    [SerializeField]
    private float _totalPositionInBeats;
    [Tooltip("The current position in beats of a rhythm track tracking a loop. If it isn't loopable, it's equal to the total. ")]
    [SerializeField]
    private float _positionInBeatsLoop;
    [Tooltip("The local beats per minute of a rhythm track. Can change in case of a tempo change. ")]
    [SerializeField]
    private float _localBPM;
    [Tooltip("Current position of the track from 0 - 1. Accounts for loops. ")]
    [SerializeField]
    private float _positionInAnalog;
    [Tooltip("Is the conductor currently tracking a rhythm track? ")]
    [SerializeField]
    private bool _isTracking;
    [SerializeField]
    private ResultsMenu resultUI;

    private float _dspTime;
    private float _secondsPassedSinceUpdateLoop = 0;
    private float _localBPS;
    private int _completedLoops = 0;
    private float _beatsPerLoop = 0;
    private bool _isPaused = false;
    private AudioSource _source;
    private RhythmTrack _musicTrack;

    public float CurrentBeat { get { return _positionInBeatsLoop; } private set { _positionInBeatsLoop = value; } }
    public float PositionInAnalog { get { return _positionInAnalog; } private set { _positionInAnalog = value; } }
    public float PositionInSeconds => _positionInSeconds;
    public float PositionInSample => _source.time;
    public float PositionInSampleDst => _source.timeSamples;
    public int CompletedLoops => _completedLoops;
    public bool IsConducting => _isTracking && !_isPaused;

    public float BPM { get { return this._localBPM; } private set { } }
    public float BPS
    {
        get
        {
            return this._localBPS;
        }
    }

    public delegate void OnTrackDo(RhythmTrack track);
    public static event OnTrackDo TrackEnd;
    public static event OnTrackDo TrackReset;


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
        _dspTime = (float)_source.time;
        _totalPositionInBeats = 0;
        _positionInBeatsLoop = 0;
        _positionInAnalog = 0;
        _positionInSeconds = 0;
        _completedLoops = 0;
        _secondsPassedSinceUpdateLoop = 0;
        _localBPM = musicTrack.BPM;
        _localBPS = 60f / _localBPM;
        _beatsPerLoop = musicTrack.BPM * (musicTrack.MusicClip.length / 60);
        this._musicTrack = musicTrack;
        // --- set input pattern map: ---- //
        _isTracking = true;
        _isPaused = false;
    }
    public void StopCurrentTrack()
    {
        _source.Stop();
        _isTracking = false;
        _isPaused = false;
    }
    public void PauseCurrentTrack()
    {
        _source.Pause();
        AudioListener.pause = true;
        _isPaused = true;
    }
    public void ResumeCurrentTrack()
    {
        if (!_source) return;
        if (!_isPaused) return;
        AudioListener.pause = false;
        _source.UnPause();
        _isPaused = false;
    }
    private void Update()
    {
        if (_isTracking && !_isPaused)
        {
            if (!_source.isPlaying && HasReachedEndOfTrack())
            {
                if (!_musicTrack.IsLoopable)
                    _isTracking = false;

                TrackEnd?.Invoke(_musicTrack);
                return;
            }
            _positionInSeconds = _source.time - _dspTime - _musicTrack.OffsetUntilStart;
            _totalPositionInBeats += (_positionInSeconds - _secondsPassedSinceUpdateLoop) / _localBPS;
            _secondsPassedSinceUpdateLoop = _positionInSeconds;
            //Calculations for Loops:
            if (HasReachedEndOfTrack() && _musicTrack.IsLoopable)
            {
                //_completedLoops++;
                _localBPM = _musicTrack.BPM;
                TrackReset?.Invoke(_musicTrack);
            }
            else if (HasReachedEndOfTrack())
            {
                _isTracking = false;
                TrackEnd?.Invoke(_musicTrack);
                return;
            }
            if (_musicTrack.IsLoopable) _positionInBeatsLoop = _totalPositionInBeats - _completedLoops * _beatsPerLoop;
            else _positionInBeatsLoop = _totalPositionInBeats;
            _positionInAnalog = _positionInBeatsLoop / _beatsPerLoop;
        }
    }
    private bool HasReachedEndOfTrack()
    {
        return _positionInAnalog >= 0.99 && _totalPositionInBeats <= 1;
    }
}

