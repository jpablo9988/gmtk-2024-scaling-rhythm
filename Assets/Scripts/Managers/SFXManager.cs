using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    private AudioSource _sfxSource;
    private void Awake()
    {
        _sfxSource = GetComponent<AudioSource>();
        _sfxSource.ignoreListenerPause = true;
    }
    public void PlaySFX(AudioClip clip, float volume, float pan = 0)
    {
        _sfxSource.panStereo = pan;
        _sfxSource.PlayOneShot(clip, volume);
    }
    public void PauseSFX()
    {
        _sfxSource.Pause();
    }
    public void ResumeSFX()
    {
        _sfxSource.UnPause();
    }
}
