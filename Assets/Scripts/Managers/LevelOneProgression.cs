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
    private PatternType beatType;

    private void Start()
    {
        ScoreTally.ResetScore();
        BuildMap();
        _audioManager.PlayMusicTrack(rhythmTrack, true);
    }
    private void BuildMap()
    {
        List<RhythmMap.BeatInformation> beatsToAdd = new();
        int rangeIndex = 0;
        int nextThreeCounter = 0;
        int extraDuts = 0;
        float newStartingBeatCounter = startingBeat;
        for(int i = 0; i < rangesWhereTriple.Count; i++)
        {
            rangesWhereTriple[i] = rangesWhereTriple[i] - (this.startingBeat + 4) + extraDuts;
            extraDuts++;
        }
        noPatterns += extraDuts;
        for (int i = 0; i < noPatterns; i++)
        {
            if (rangeIndex < rangesWhereTriple.Count)
            {
                if (rangesWhereTriple[rangeIndex] == i)
                {
                    nextThreeCounter = 2;
                    rangeIndex++;
                }
            }
            if (nextThreeCounter <= 0)
            {
                RhythmMap.BeatInformation aux = new();
                newStartingBeatCounter += 1;
                aux.activationBeat = newStartingBeatCounter;
                aux.beatType = this.beatType;
                beatsToAdd.Add(aux);
            }
            else
            {
                RhythmMap.BeatInformation aux = new();
                newStartingBeatCounter += 0.5f;
                aux.activationBeat = newStartingBeatCounter;
                aux.beatType = this.beatType;
                nextThreeCounter--;
                beatsToAdd.Add(aux);
            }
        }
        this.rhythmTrack.Map.SetWholeMapInfo(beatsToAdd);
    }
}
