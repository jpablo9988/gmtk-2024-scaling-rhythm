using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PatternManager : MonoBehaviour
{
    private RhythmMap map;
    private AudioManager audioManager;
    private Conductor conductor;
    [Header("Music Trackers. ")]
    [ReadOnly]
    [SerializeField]
    private bool isTracking;
    [ReadOnly]
    [SerializeField]
    private int beatIndex;
    private List<ActionableBeat> whenToInputList = new();
    [SerializeField]
    private ScoreObserver scoreObserver;

    private struct ActionableBeat
    {
        public float actionBeat;
        public PatternType.InputWindowInformation inputWindowInfo;
        public InputType type;
        public PatternType.SfxInformation sfxInfo;
        public ActionableBeat(float actionBeat, PatternType.InputWindowInformation inputInfo, InputType type, PatternType.SfxInformation sfxInfo)
        {
            this.actionBeat = actionBeat; 
            this.inputWindowInfo = inputInfo;
            this.type = type;
            this.sfxInfo = sfxInfo;
        }
    }

    private void OnEnable()
    {
        InputObserver.OnPlayerInput += HitNextBeat;
    }

    private void OnDisable()
    {
        InputObserver.OnPlayerInput -= HitNextBeat;
    }
    public void SetPlayableMap(Conductor conductor, AudioManager manager, RhythmMap map)
    {
        this.conductor = conductor;
        this.audioManager = manager;
        this.map = map;
        beatIndex = 0;
        isTracking = true;
    }
    public void ResetCurrentMap()
    {
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
                    PatternType patternInfo = map.BeatList[beatIndex].beatType;
                    float nextBeatOnHit = map.BeatList[beatIndex].activationBeat + patternInfo.beatsUntilHit;
                    Debug.Log("Ready or not here I come in " + patternInfo.beatsUntilHit + "beats! ");
                    whenToInputList.Add(new(nextBeatOnHit
                        , patternInfo.inputWindowInfo
                        , patternInfo.triggerInput
                        , patternInfo.sfxInfo));
                    whenToInputList = whenToInputList.OrderBy(o => o.actionBeat).ToList() ;
                    audioManager.PlaySFX(patternInfo.sfxInfo.sfxOnTell, map.PanningInfoSFX.Item1);
                    beatIndex++;
                    
                    // .. Here is where the animation starts for a pattern/beats. 
                    // on nextBeat on Hit, have it so it reaches a position where the player can interact with it. 
                }
                // -- it's not done going through the beat list!
            }
            if (whenToInputList.Count > 0)
            {
                if ((conductor.CurrentBeat - whenToInputList[0].actionBeat) >= whenToInputList[0].inputWindowInfo.InputWindow)
                {
                    scoreObserver.InputScore(ScoreType.Miss);
                    Debug.Log("Ooooh I missed!");
                    audioManager.PlaySFX(whenToInputList[0].sfxInfo.sfxOnMiss);
                    whenToInputList.RemoveAt(0);
                }
            }
        }
    }
    public void HitNextBeat(InputType type)
    {
        if (whenToInputList.Count > 0)
        {
            float reminder = Math.Abs(conductor.CurrentBeat - whenToInputList[0].actionBeat);
            PatternType.InputWindowInformation windowInfo = whenToInputList[0].inputWindowInfo;
            if (reminder <= windowInfo.InputWindow)
            {
                if (type != whenToInputList[0].type)
                {
                    Debug.Log("Missed input!!");
                    return;
                }
                if (reminder <= windowInfo.PerfectWindow)
                {
                    scoreObserver.InputScore(ScoreType.Perfect);
                    audioManager.PlaySFX(whenToInputList[0].sfxInfo.sfxOnPerfect);
                    Debug.Log("Poifect!");
                }
                else if (reminder <= windowInfo.GoodWindow)
                {
                    scoreObserver.InputScore(ScoreType.Good);
                    audioManager.PlaySFX(whenToInputList[0].sfxInfo.sfxOnGood);
                    Debug.Log("Aight!");
                }
                else if (reminder <= windowInfo.MehWindow)
                {
                    scoreObserver.InputScore(ScoreType.Meh);
                    audioManager.PlaySFX(whenToInputList[0].sfxInfo.sfxOnMeh);
                    Debug.Log("Meh!");
                }
                else
                {
                    scoreObserver.InputScore(ScoreType.Poor);
                    audioManager.PlaySFX(whenToInputList[0].sfxInfo.sfxOnMeh);
                    Debug.Log("Poor!");
                }
                whenToInputList.RemoveAt(0);
                return;
            }
            Debug.Log("No input window!");
            return;
        }
        Debug.Log("No Pattern in sight!");
    }
}
