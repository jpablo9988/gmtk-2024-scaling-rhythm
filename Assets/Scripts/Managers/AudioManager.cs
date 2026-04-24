using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : IPausable
{
    [Header("Dependencies")]
    [Tooltip("A reference to the Conductor which tracks rhythm properties of a track in-game. ")]
    [SerializeField]
    private Conductor conductor;
    [SerializeField]
    private SFXManager sfxManager;
    [Tooltip("A reference to which Audio Mixer the source is outputting towards. ")]
    [SerializeField]
    private AudioMixer musicMixer;
    [SerializeField]
    private AudioMixer sfxMixer;
    [SerializeField]
    private AudioMixer allMixer;
    [Tooltip("The name of the exposed parameter representing volume in the target MusicMixer")]
    [SerializeField]
    private string musicMixerExposedParam;
    [Tooltip("The name of the exposed parameter representing volume in the target MusicMixer")]
    [SerializeField]
    private string sfxMixerExposedParam;
    [Tooltip("The name of the exposed parameter representing volume in the target MusicMixer")]
    [SerializeField]
    private string allMixerExposedParam;
    [Header("Fading Properties")]
    [SerializeField]
    private float fadeOutDuration;
    [SerializeField]
    private float fadeInDuration;
    private AudioSource audioSource;
    public Conductor BaseConductor => conductor;
    private IEnumerator stoppingTrackProcess;
    private bool isCurrentlyStoppingTrack = false;
    private Track currTrack;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        PlayerSettings.MasterVolumeName = allMixerExposedParam;
        PlayerSettings.MusicVolumeName = musicMixerExposedParam;
        PlayerSettings.SfxVolumeName = sfxMixerExposedParam;
        conductor.SetSourceReference(this.audioSource);
    }

    /// <summary>
    /// Fades out or completely stops current playing audio clip. 
    /// </summary>
    /// <param name="fadeTrack"> Will it fade out the track?</param>
    public void StopCurrentTrack(bool fadeTrack)
    {
        if (!fadeTrack)
        {
            audioSource.Stop();
            return;
        }
        isCurrentlyStoppingTrack = true;
        stoppingTrackProcess = UtilitiesAudioMixer.StartFade(musicMixer, musicMixerExposedParam, fadeOutDuration, 0.0001f, audio: null, trigger: (_) =>
        {
            isCurrentlyStoppingTrack = false;
        });
        StartCoroutine(stoppingTrackProcess);
        conductor.StopCurrentTrack();
    }
    public void OnStopTrackNonLoopable()
    {
        audioSource.Stop();
        audioSource.clip = null;
    }
    /// <summary>
    /// Plays a new audio clip with the option of fading it in. If a clip is currently playing, it will fade out or stop
    /// according to preference.
    /// </summary>
    /// <param name="track"> Which clip it will play. </param>
    /// <param name="fadeTrack"> Will it fade in the audio? </param>
    public void PlayMusicTrack(Track track, bool fadeTrack, PatternManager patternManager = null)
    {

        float vol = PlayerSettings.MusicVolume;
        this.currTrack = track;

        if (!fadeTrack)
        {
            UtilitiesAudioMixer.SetVolume(musicMixer, musicMixerExposedParam,
                PlayerSettings.MusicVolume);
            if (isCurrentlyStoppingTrack)
            {
                isCurrentlyStoppingTrack = false;
                StopCoroutine(stoppingTrackProcess);
            }
            PlayTrack(track);
            StartRhythmTracking(track, patternManager);
            return;
        }
        if (!audioSource.isPlaying)
        {
            UtilitiesAudioMixer.SetVolume(musicMixer, musicMixerExposedParam, 0.0001f);
            PlayTrack(track);
            StartRhythmTracking(track, patternManager);
            StartCoroutine(UtilitiesAudioMixer.StartFade(musicMixer, musicMixerExposedParam,
                fadeInDuration, vol));
        }
        else
        {
            StartCoroutine(UtilitiesAudioMixer.StartFade(musicMixer, musicMixerExposedParam,
                fadeOutDuration, 0, track, (paramClip) =>
                {
                    PlayTrack(paramClip);
                    StartRhythmTracking(paramClip, patternManager);
                    StartCoroutine(UtilitiesAudioMixer.StartFade(musicMixer, musicMixerExposedParam
                        , fadeInDuration, vol));
                }));
        }
    }
    public void PlaySFX(AudioClip clip, float pan = 0)
    {
        if (clip != null)
        {
            sfxManager.PlaySFX(clip, PlayerSettings.SfxVolume, pan);
        }
    }
    private void PlayTrack(Track track, PatternManager patternManager = null)
    {
        audioSource.Stop();
        audioSource.pitch = track.Pitch;
        audioSource.clip = track.MusicClip;
        audioSource.loop = track.IsLoopable;
        audioSource.PlayScheduled(1.0f);
    }
    private void StartRhythmTracking(Track track, PatternManager patternManager)
    {
        if (track is RhythmTrack rhythmTrack)
        {
            conductor.ConductMusicTrack(rhythmTrack);
            if (patternManager == null) return;
            patternManager.SetPlayableMap(conductor, this, rhythmTrack.Map);
        }
    }
    public void ChangeMusicVolume(float newValue)
    {
        UtilitiesAudioMixer.SetVolume(musicMixer, musicMixerExposedParam,
                        newValue);
    }
    public void ChangeSFXVolume(float newValue)
    {
        UtilitiesAudioMixer.SetVolume(sfxMixer, sfxMixerExposedParam,
                                newValue);
    }
    public void ChangeMasterVolume(float newValue)
    {
        UtilitiesAudioMixer.SetVolume(allMixer, allMixerExposedParam,
                                        newValue);
    }
    public void ChangeVolume(string parameter, float newValue)
    {
        if (string.Compare(parameter, musicMixerExposedParam, true) == 0)
        {
            ChangeMusicVolume(newValue);
            return;
        }
        if (string.Compare(parameter, sfxMixerExposedParam, true) == 0)
        {
            ChangeSFXVolume(newValue);
            return;
        }
        if (string.Compare(parameter, allMixerExposedParam, true) == 0)
        {
            ChangeMasterVolume(newValue);
            return;
        }
    }
    public override void Pause(bool isPaused)
    {
        base.Pause(isPaused);
        if (isPaused)
        {
            audioSource.Pause();
            conductor.PauseCurrentTrack();
            sfxManager.PauseSFX();
        }
        else
        {
            audioSource.UnPause();
            conductor.ResumeCurrentTrack();
            sfxManager.ResumeSFX();
        }
    }
    public void IsMusicAffectedByPause(bool isAffected)
    {
        audioSource.ignoreListenerPause = !isAffected;
    }
}
