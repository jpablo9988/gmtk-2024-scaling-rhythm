using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PatternManager : IInputReciever
{
    private RhythmMap map;
    private AudioManager audioManager;
    private Conductor conductor;
    [Header("Music Trackers. ")]
    [SerializeField]
    private bool isTracking;
    [SerializeField]
    private int beatIndex;
    private List<ActionableBeat> whenToInputList = new();
    [SerializeField]
    private ScoreObserver scoreObserver;
    public static event Action<TelegraphPackage> beatTelegraph;
    public bool IsTracking => isTracking;
    public ActionableBeat? NextInput
    {
        get
        {
            if (whenToInputList.Count == 0 || whenToInputList == null)
            {
                return null;
            }
            else return whenToInputList[0];
        }
    }
    public struct ActionableBeat
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
    public struct TelegraphPackage
    {
        public float beatsUntilHit;
        public float? distanceToPrevious;
        public float? distanceToNext;
    }

    public void SetPlayableMap(Conductor conductor, AudioManager manager, RhythmMap map)
    {
        whenToInputList.Clear();
        this.conductor = conductor;
        audioManager = manager;
        this.map = map;
        beatIndex = 0;
        isTracking = true;
    }
    public void ResetCurrentMap(RhythmTrack track)
    {
        beatIndex = 0;
    }
    public void StopPlayableMap()
    {
        isTracking = false;
        beatIndex = 0;
    }
    private TelegraphPackage BuildTelegraphPackage(RhythmMap map, int currBeatIndex)
    {
        PatternType patternInfo = map.BeatList[currBeatIndex].beatType;
        TelegraphPackage telegraphPackage = new()
        {
            beatsUntilHit = patternInfo.beatsUntilHit,
            distanceToPrevious = null,
            distanceToNext = null
        };
        if (currBeatIndex - 1 >= 0)
            telegraphPackage.distanceToPrevious = map.BeatList[currBeatIndex].activationBeat -
            map.BeatList[currBeatIndex - 1].activationBeat;
        if (currBeatIndex + 1 < map.BeatList.Count)
            telegraphPackage.distanceToNext = map.BeatList[currBeatIndex + 1].activationBeat -
                map.BeatList[currBeatIndex].activationBeat;
        return telegraphPackage;
    }
    private void Update()
    {
        if (isGamePaused) return;
        if (isTracking)
        {
            if (beatIndex < map.BeatList.Count)
            {
                if (conductor.CurrentBeat >= map.BeatList[beatIndex].activationBeat)
                {
                    PatternType patternInfo = map.BeatList[beatIndex].beatType;
                    float nextBeatOnHit = map.BeatList[beatIndex].activationBeat + patternInfo.beatsUntilHit;
                    beatTelegraph?.Invoke(BuildTelegraphPackage(map, beatIndex));
                    whenToInputList.Add(new(nextBeatOnHit
                        , patternInfo.inputWindowInfo
                        , patternInfo.triggerInput
                        , patternInfo.sfxInfo));
                    whenToInputList = whenToInputList.OrderBy(o => o.actionBeat).ToList();
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
                    audioManager.PlaySFX(whenToInputList[0].sfxInfo.sfxOnMiss);

                    whenToInputList.RemoveAt(0);
                }
            }
        }
    }
    public override void ExecuteInput(InputType type)
    {
        if (!isTracking) return;
        if (type != InputType.Button1) return;
        if (whenToInputList.Count <= 0)
        {
            scoreObserver.InputScore(ScoreType.Other);
            return;
        }
        ActionableBeat auxBeat = whenToInputList[0];
        float reminder = Math.Abs(conductor.CurrentBeat - auxBeat.actionBeat);
        //float tangibleScore = 100 - reminder;
        PatternType.InputWindowInformation windowInfo = auxBeat.inputWindowInfo;
        if (reminder <= windowInfo.InputWindow)
        {
            if (type != auxBeat.type)
            {
                scoreObserver.InputScore(ScoreType.Other);
                return;
            }
            if (reminder <= windowInfo.PerfectWindow)
            {
                scoreObserver.InputScore(ScoreType.Perfect);
                audioManager.PlaySFX(auxBeat.sfxInfo.sfxOnPerfect);
            }
            else if (reminder <= windowInfo.GoodWindow)
            {
                scoreObserver.InputScore(ScoreType.Good);
                audioManager.PlaySFX(auxBeat.sfxInfo.sfxOnGood);
            }
            else if (reminder <= windowInfo.MehWindow)
            {
                scoreObserver.InputScore(ScoreType.Meh);
                audioManager.PlaySFX(auxBeat.sfxInfo.sfxOnMeh);
            }
            else
            {
                scoreObserver.InputScore(ScoreType.Poor);
                audioManager.PlaySFX(auxBeat.sfxInfo.sfxOnMeh);
            }
            if (whenToInputList.Count > 0) whenToInputList.RemoveAt(0);
            return;
        }
        scoreObserver.InputScore(ScoreType.Other);
        return;
    }
}
