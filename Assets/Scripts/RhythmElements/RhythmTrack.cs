using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Music/Rhythm Track", fileName = "New Music Track")]
public class RhythmTrack : Track
{
    [SerializeField]
    private float musicBPM;
    [SerializeField]
    private List<SongTempoChanges> tempoChanges;
    [SerializeField]
    private float offsetUntilStart;
    public float BPM { get { return musicBPM; } private set { musicBPM = value; } }
    public float OffsetUntilStart { get { return offsetUntilStart; } private set { offsetUntilStart = value; } }


    [System.Serializable]
    public struct SongTempoChanges
    {
        public float startingBeat;
        public float startingBPM;
        public float endingBeat;
        public float endingBPM;
    };

    public SongTempoChanges? GetTempoChangeInfo(int orderIndex)
    {
        if (tempoChanges.Count <= orderIndex) return null;
        return tempoChanges[orderIndex];
    }
}
