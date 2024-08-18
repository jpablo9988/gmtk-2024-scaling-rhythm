using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public static class UtilitiesAudioMixer
{
    public static void SetVolume(AudioMixer mixer, string exposedParam, float targetVolume)
    {
        float parametrizedVolume = Mathf.Log10(targetVolume) * 20;
        mixer.SetFloat(exposedParam, parametrizedVolume);
    }
    public static IEnumerator StartFade(AudioMixer audioMixer, string exposedParam,
        float duration, float targetVolume)
    {
        float currentTime = 0;
        audioMixer.GetFloat(exposedParam, out float currentVol);
        currentVol = Mathf.Pow(10, currentVol / 20);
        float targetValue = Mathf.Clamp(targetVolume, 0.0001f, 1);
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            float newVol = Mathf.Lerp(currentVol, targetValue, currentTime / duration);
            audioMixer.SetFloat(exposedParam, Mathf.Log10(newVol) * 20);
            yield return null;
        }
        yield break;
    }
    public static IEnumerator StartFade(AudioMixer audioMixer, string exposedParam,
        float duration, float targetVolume, AudioClip audio, Action<AudioClip> trigger)
    {
        float currentTime = 0;
        audioMixer.GetFloat(exposedParam, out float currentVol);
        currentVol = Mathf.Pow(10, currentVol / 20);
        float targetValue = Mathf.Clamp(targetVolume, 0.0001f, 1);
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            float newVol = Mathf.Lerp(currentVol, targetValue, currentTime / duration);
            audioMixer.SetFloat(exposedParam, Mathf.Log10(newVol) * 20);
            yield return null;
        }
        trigger?.Invoke(audio);
        yield break;
    }
    public static IEnumerator StartFade(AudioMixer audioMixer, string exposedParam,
        float duration, float targetVolume, Track rhythmTrack, Action<Track> trigger)
    {
        float currentTime = 0;
        audioMixer.GetFloat(exposedParam, out float currentVol);
        currentVol = Mathf.Pow(10, currentVol / 20);
        float targetValue = Mathf.Clamp(targetVolume, 0.0001f, 1);
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            float newVol = Mathf.Lerp(currentVol, targetValue, currentTime / duration);
            audioMixer.SetFloat(exposedParam, Mathf.Log10(newVol) * 20);
            yield return null;
        }
        trigger?.Invoke(rhythmTrack);
        yield break;
    }
}
