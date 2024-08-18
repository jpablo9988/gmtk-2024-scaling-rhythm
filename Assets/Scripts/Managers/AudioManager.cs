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
    [Tooltip("A reference to which Audio Mixer the source is outputting towards. ")]
    [SerializeField]
    private AudioMixer musicMixer;
    [Tooltip("The name of the exposed parameter representing volume in the target MixerGroup")]
    [SerializeField]
    private string musicMixerExposedParam;
    [Header("Fading Properties")]
    [SerializeField]
    private float fadeOutDuration, fadeInDuration;


    private AudioSource audioSource;
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
        StartCoroutine(FadeAudioMixer.StartFade(musicMixer, musicMixerExposedParam, fadeOutDuration, 0.0001f));
    }
    /// <summary>
    /// Plays a new audio clip (taken from track as track.MusicClip) with the option of fading it in. If a clip is currently playing, it will fade out or stop
    /// according to preference.
    /// </summary>
    /// <param name="track"> Which clip it will play. </param>
    /// <param name="fadeTrack"> Will it fade in the audio? </param>
    public void PlayMusicTrack(Track track, bool fadeTrack)
    {

        float vol = PlayerSettings.preferedVolume;
        if (!fadeTrack)
        {
            audioSource.Stop();
            audioSource.clip = track.MusicClip;
            FadeAudioMixer.SetVolume(musicMixer, musicMixerExposedParam,
                PlayerSettings.preferedVolume);
            audioSource.Play();
            return;
        }
        if (!audioSource.isPlaying)
        {
            FadeAudioMixer.SetVolume(musicMixer, musicMixerExposedParam, 0.0001f);
            audioSource.clip = track.MusicClip;
            audioSource.Play();
            StartCoroutine(FadeAudioMixer.StartFade(musicMixer, musicMixerExposedParam,
                fadeInDuration, vol, track, StartRhythmTracking));
        }
        else
        {
            StartCoroutine(FadeAudioMixer.StartFade(musicMixer, musicMixerExposedParam,
                fadeOutDuration, 0, track, (paramClip) =>
                {
                    audioSource.Stop();
                    audioSource.clip = paramClip.MusicClip;
                    audioSource.Play();
                    StartRhythmTracking(paramClip);
                    StartCoroutine(FadeAudioMixer.StartFade(musicMixer, musicMixerExposedParam
                        , fadeInDuration, vol));
                }));
        }
    }
    private void StartRhythmTracking(Track track)
    {
        
        if (track is RhythmTrack rhythmTrack)
        {
            conductor.ConductMusicTrack(rhythmTrack);
        }
    }
}
