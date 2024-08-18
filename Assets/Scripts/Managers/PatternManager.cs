using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternManager : MonoBehaviour
{
    private RhythmMap map;
    private Conductor conductor;
    [Header("Music Trackers. ")]
    [ReadOnly]
    [SerializeField]
    private bool isTracking;
    [ReadOnly]
    [SerializeField]
    private int beatIndex;
    private List<Tuple<float, float>> whenToInputList = new();
    public void SetPlayableMap(Conductor conductor, RhythmMap map)
    {
        this.conductor = conductor;
        this.map = map;
        beatIndex = 0;
        isTracking = true;
    }
    public void StopPlayableMap()
    {
        isTracking = false;
    }
    private void Update()
    {
        if (isTracking)
        {
            if (beatIndex < map.BeatList.Count)
            {
                if (conductor.CurrentBeat >= map.BeatList[beatIndex].activationBeat)
                {
                    float nextBeatOnHit = map.BeatList[beatIndex].activationBeat + map.BeatList[beatIndex].beatType.beatsUntilHit;
                    Debug.Log("Ready or not here I come in " + map.BeatList[beatIndex].beatType.beatsUntilHit + "beats! ");
                    whenToInputList.Add(new(nextBeatOnHit, map.BeatList[beatIndex].beatType.inputWindow));
                    whenToInputList.Sort();
                    beatIndex++;
                    // .. Here is where the animation starts for a pattern/beats. 
                    // on nextBeat on Hit, have it so it reaches a position where the player can interact with it. 
                }
                // -- it's not done going through the beat list!
            }
            if (whenToInputList.Count > 0)
            {
                if ((conductor.CurrentBeat - whenToInputList[0].Item1) >= whenToInputList[0].Item2)
                {
                    // Miss!
                    Debug.Log("Ooooh I missed!");
                    whenToInputList.RemoveAt(0);
                }
            }
        }
    }
    public void HitNextBeat()
    {
        if (whenToInputList.Count > 0)
        {
            float reminder = Math.Abs(conductor.CurrentBeat - whenToInputList[0].Item1);
            if (reminder < whenToInputList[0].Item2)
            {
                if (reminder < whenToInputList[0].Item2 / 2)
                {
                    //Great!
                    Debug.Log("Poifect!");
                }
                // Good
                //Add to Success Counter. 
                Debug.Log("Aight!");
            }
            return;
        }
        Debug.Log("No input window in sight!");
    }
}
