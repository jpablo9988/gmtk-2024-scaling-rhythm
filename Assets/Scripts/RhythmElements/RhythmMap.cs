using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Pattern/PatternsMap")]
public class RhythmMap : ScriptableObject
{
    [SerializeField]
    private List<BeatInformation> beatList;
    public List<BeatInformation> BeatList 
    { get { return beatList; } 
        private set
        {
            beatList = value;
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
