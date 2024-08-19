using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Pattern/MapOfPatterns")]
public class RhythmMap : ScriptableObject
{
    [Tooltip("Properties")]
    [SerializeField]
    [Range(-1f, 1f)]
    private float sfx_TellPan;
    [SerializeField]
    [Range(-1f, 1f)]
    private float sfx_ActionPan;
    [Tooltip("Map")]
    [SerializeField]
    private List<BeatInformation> beatList;
    public List<BeatInformation> BeatList 
    { get { return beatList; } 
        private set
        {
            beatList = value;
        }
    }
    /// <summary>
    /// Panning Information for SFX Trigger (in Stereo).
    /// <br></br>Item 1: Panning of the Tell
    /// <br></br>Item 2: Panning of the Action
    /// </summary>
    public Tuple<float, float> PanningInfoSFX { 
        get
        {
            return new(sfx_TellPan, sfx_ActionPan);
        }
        private set
        {
            sfx_TellPan = value.Item1;
            sfx_ActionPan = value.Item2;
        }
    }

    [System.Serializable]
    public struct BeatInformation
    {
        /// <summary>
        /// when this beat first manifests. This is equivalent to the "tell". 
        /// </summary>
        public float activationBeat;
        public PatternType beatType;

    };
}
