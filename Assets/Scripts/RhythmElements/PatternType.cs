using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Pattern/Type")]
public class PatternType : ScriptableObject
{
    public float beatsUntilHit;
    public float inputWindow;
    public InputType triggerInput;
    public SfxInformation sfxInfo;

    [System.Serializable]
    public struct SfxInformation
    {
        public AudioClip sfxOnTell;
        public AudioClip sfxOnPerfect;
        public AudioClip sfxOnGood;
        public AudioClip sfxOnMiss;
    }
}
