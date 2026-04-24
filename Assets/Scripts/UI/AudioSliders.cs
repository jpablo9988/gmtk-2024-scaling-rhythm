using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSliders : MonoBehaviour
{
    [SerializeField]
    private AudioManager audioManager;
    void Start()
    {
        if (audioManager == null)
        {
            audioManager = FindFirstObjectByType<AudioManager>();
        }
    }
    public void OnChangeVolume(VolumePackage package)
    {
        string[] volumeGroup = package.ChangeParameter.Split(',');
        PlayerSettings.SetSpecificValue(volumeGroup[0], package.Slider.value / 100f);
        for (int i = 0; i < volumeGroup.Length; i++)
        {
            audioManager.ChangeVolume(volumeGroup[i], package.Slider.value / 100f);
        }
        package.textMarker.text = package.Slider.value.ToString();
    }
    public void OnChangeFullScreen(bool value)
    {
        Screen.fullScreen = value;
    }
}
