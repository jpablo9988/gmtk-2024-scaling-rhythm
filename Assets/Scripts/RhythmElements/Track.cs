using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A wrapper for an audioclip with more information. 
/// </summary>
[CreateAssetMenu(menuName = "Music/Regular Track", fileName = "New Music Track")]
public class Track : ScriptableObject
{
    [Tooltip("Name of the track. ")]
    [SerializeField]
    private string musicName;
    [Tooltip("Reference to the audio clip. ")]
    [SerializeField]
    private AudioClip musicClip;
    [Tooltip("Will the track loop? ")]
    [SerializeField]
    private bool isLoopable;

    public AudioClip MusicClip { get { return musicClip; } private set { musicClip = value; } }
    public bool IsLoopable { get { return isLoopable; } private set { isLoopable = value; } }
}
