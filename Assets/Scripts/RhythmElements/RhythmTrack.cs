using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Child of Track that will be tracked by a conductor for a rhythm minigame. 
/// </summary>
[CreateAssetMenu(menuName = "Music/Rhythm Track", fileName = "New Music Track")]
public class RhythmTrack : Track
{
    [Tooltip("Beats per minute.")]
    [SerializeField]
    private float musicBPM;
    [Tooltip("In seconds, the time it takes for the song to start after 0:00:00")]
    [SerializeField]
    private float offsetUntilStart;
    [Tooltip("Contains a reference to the mapping information for when the player needs to input an action in rhythm. ")]
    [SerializeField]
    private RhythmMap map;
    [Tooltip("A list of the tempo changes the current song has. (Not supported yet). ")]
    [SerializeField]
    private List<SongTempoChanges> tempoChanges;
    public float BPM { get { return musicBPM; } private set { musicBPM = value; } }
    public float OffsetUntilStart { get { return offsetUntilStart; } private set { offsetUntilStart = value; } }

    public RhythmMap Map { get { return map; } private set { map = value; } }


    [System.Serializable]
    public struct SongTempoChanges
    {
        public float startingBeat;
        public float startingBPM;
        public float endingBeat;
        public float endingBPM;
    };
    /// <summary>
    /// Get tempo change information. Null if list is empty or index overflows. 
    /// </summary>
    /// <param name="orderIndex"></param>
    /// <returns></returns>
    public SongTempoChanges? GetTempoChangeInfo(int orderIndex)
    {
        if (tempoChanges.Count <= orderIndex) return null;
        return tempoChanges[orderIndex];
    }
}
