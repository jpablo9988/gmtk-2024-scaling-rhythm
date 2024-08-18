using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Beats/Type")]
public class BeatType : ScriptableObject
{
    public float beatsUntilHit;
    public AudioClip sfxActivation;
    public AudioClip sfxOnHit;
}
