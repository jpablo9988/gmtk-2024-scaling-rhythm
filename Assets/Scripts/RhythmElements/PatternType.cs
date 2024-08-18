using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Pattern/Type")]
public class PatternType : ScriptableObject
{
    public float beatsUntilHit;
    public float inputWindow;
    public AudioClip sfxActivation;
    public AudioClip sfxOnHit;
}
