using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Pattern/PatternType")]
public class PatternType : ScriptableObject
{
    [Header("Attributes")]
    [Tooltip("Additive to activationBeat on dependent map. What beats does it take from activation to expected player input.")]
    public float beatsUntilHit;
    public InputType triggerInput;
    [Tooltip("General information about the input window the player has (in BPM ).")]
    public InputWindowInformation inputWindowInfo;
    [Header("Sound")]
    public SfxInformation sfxInfo;

    [System.Serializable]
    public struct SfxInformation
    {
        public AudioClip sfxOnTell;
        public AudioClip sfxOnPerfect;
        public AudioClip sfxOnGood;
        public AudioClip sfxOnMeh;
        public AudioClip sfxOnMiss;
    }
    [System.Serializable]
    public struct InputWindowInformation
    {
        [SerializeField]
        private float inputWindow;
        [SerializeField]
        private float mehWindow;
        [SerializeField]
        private float goodWindow;
        [SerializeField]
        private float perfectWindow;

        public float InputWindow
        {
            get { return inputWindow; }
            private set { inputWindow = value; }
        }
        public float MehWindow
        {
            get {  
            if (inputWindow < mehWindow || mehWindow <= 0)
                return inputWindow;
            else
                return mehWindow; 
            }
            private set { mehWindow = value; }
        }
        public float GoodWindow
        {
            get
            {
                if (MehWindow < goodWindow || goodWindow <= 0)
                    return MehWindow;
                else
                    return goodWindow;
            }
            private set { goodWindow = value; }
        }

        public float PerfectWindow
        {
            get
            {
                if (GoodWindow < perfectWindow || perfectWindow <= 0)
                    return GoodWindow;
                else
                    return perfectWindow;
            }
            private set { perfectWindow = value; }
        }

    }
}
