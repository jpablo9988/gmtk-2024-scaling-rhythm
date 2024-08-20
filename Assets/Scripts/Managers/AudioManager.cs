using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    [Header ("Dependencies")]
    [Tooltip("A reference to the Conductor which tracks rhythm properties of a track in-game. ")]
    [SerializeField]
    private Conductor conductor;
    [SerializeField]
    private SFXManager sfxManager;
    [Tooltip("A reference to which Audio Mixer the source is outputting towards. ")]
    [SerializeField]
    private AudioMixer musicMixer;
    [Tooltip("The name of the exposed parameter representing volume in the target MixerGroup")]
    [SerializeField]
    private string musicMixerExposedParam;
    [Header("Fading Properties")]
    [SerializeField]
    private float fadeOutDuration;
    [SerializeField]
    private float fadeInDuration;


    private AudioSource audioSource;
    private Track currTrack;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    private void Start()
    {
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
        StartCoroutine(UtilitiesAudioMixer.StartFade(musicMixer, musicMixerExposedParam, fadeOutDuration, 0.0001f));
    }
    /// <summary>
    /// Plays a new audio clip with the option of fading it in. If a clip is currently playing, it will fade out or stop
    /// according to preference.
    /// </summary>
    /// <param name="track"> Which clip it will play. </param>
    /// <param name="fadeTrack"> Will it fade in the audio? </param>
    public void PlayMusicTrack(Track track, bool fadeTrack)
    {

        float vol = PlayerSettings.preferedVolume;
        this.currTrack = track;

        if (!fadeTrack)
        {
            UtilitiesAudioMixer.SetVolume(musicMixer, musicMixerExposedParam,
                PlayerSettings.preferedVolume);
            PlayTrack(track);
            StartRhythmTracking(track);
            return;
        }
        if (!audioSource.isPlaying)
        {
            UtilitiesAudioMixer.SetVolume(musicMixer, musicMixerExposedParam, 0.0001f);
            PlayTrack(track);
            StartRhythmTracking(track);
            StartCoroutine(UtilitiesAudioMixer.StartFade(musicMixer, musicMixerExposedParam,
                fadeInDuration, vol));
        }
        else
        {
            StartCoroutine(UtilitiesAudioMixer.StartFade(musicMixer, musicMixerExposedParam,
                fadeOutDuration, 0, track, (paramClip) =>
                {
                    PlayTrack(paramClip);
                    StartRhythmTracking(paramClip);
                    StartCoroutine(UtilitiesAudioMixer.StartFade(musicMixer, musicMixerExposedParam
                        , fadeInDuration, vol));
                }));
        }
    }
    public void PlaySFX(AudioClip clip, float pan = 0)
    {
        if (clip != null)
        {
            sfxManager.PlaySFX(clip, PlayerSettings.sfxVolue, pan);
        }
    }
    private void PlayTrack(Track track)
    {
        audioSource.Stop();
        audioSource.clip = track.MusicClip;
        audioSource.loop = track.IsLoopable;
        audioSource.Play();
    }
    private void StartRhythmTracking(Track track)
    {
        
        if (track is RhythmTrack rhythmTrack)
        {
            conductor.ConductMusicTrack(this, rhythmTrack);
        }
    }
}
