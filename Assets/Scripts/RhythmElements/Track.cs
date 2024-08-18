using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Music/Regular Track", fileName = "New Music Track")]
public class Track : ScriptableObject
{
    [SerializeField]
    private string musicName;
    [SerializeField]
    private AudioClip musicClip;

    public AudioClip MusicClip { get { return musicClip; } private set { musicClip = value; } }
}
