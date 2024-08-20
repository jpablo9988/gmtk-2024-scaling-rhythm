using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelOneProgression : MonoBehaviour
{
    [SerializeField]
    private AudioManager _audioManager;
    [SerializeField]
    private RhythmTrack rhythmTrack;
    [SerializeField]
    private List<int> rangesWhereTriple;
    [SerializeField]
    private int noPatterns;
    [SerializeField]
    private int startingBeat;
    [SerializeField]

    private void Start()
    {
        ScoreTally.ResetScore();
        _audioManager.PlayMusicTrack(rhythmTrack, true);
    }
    private void BuildMap()
    {
        List<RhythmMap.BeatInformation> beatsToAdd = new();
        int rangeIndex = 0;
        int nextThreeCounter = 0;
        int newStartingBeatCounter = startingBeat;
        for (int i = 0; i < noPatterns; i++)
        {
            if (rangeIndex < rangesWhereTriple.Count)
            {
                if (rangesWhereTriple[rangeIndex] == i)
                {
                    nextThreeCounter = 3;
                    rangeIndex++;
                }
            }
            if (nextThreeCounter <= 0)
            {
                RhythmMap.BeatInformation aux = new();
                newStartingBeatCounter += 1;
                aux.activationBeat = newStartingBeatCounter;
            }
        }
    }
}
