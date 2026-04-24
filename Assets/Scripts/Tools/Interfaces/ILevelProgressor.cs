
using System;
using JPA_DialogueSystem;
using UnityEngine;
using UnityEngine.Playables;

public abstract class ILevelProgressor : IPausable
{
    public abstract void StartRhythmTrack(RhythmTrack rhythmTrack, PatternManager patternManager, bool trackScore, Track introTrack = null, PlayableDirector cinematicIntro = null, bool enteredTutorial = false);
    public abstract void LoadRelevantAssets(RhythmTrack rhythmTrack);
    public abstract void StartTrack(Track track);
}
