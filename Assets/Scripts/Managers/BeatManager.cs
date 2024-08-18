using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatManager : MonoBehaviour
{
    private RhythmMap map;
    private Conductor conductor;
    private bool isTracking;
    private int beatIndex;
    private List<float> inputBeatsList;
    public void SetPlayableMap(Conductor conductor, RhythmMap map)
    {
        this.conductor = conductor;
        this.map = map;
        beatIndex = 0;
        isTracking = true;
    }
    private void Update()
    {
        if (isTracking)
        {
            if (Mathf.Approximately(map.BeatList[beatIndex].activationBeat , conductor.CurrentBeat))
            {
                float nextBeatOnHit = map.BeatList[beatIndex].activationBeat + map.BeatList[beatIndex].beatType.beatsUntilHit;
                inputBeatsList.Add(nextBeatOnHit);
                inputBeatsList.Sort();
                // .. Here is where the animation starts for a pattern/beats. 
                // on nextBeat on Hit, have it so it reaches a position where the player can interact with it. 
            }
        }
    }
    public void HitNextBeat()
    {
        //If it's within the range 
    }
}
