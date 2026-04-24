using System;
using System.Collections.Generic;
using System.Linq;
using JPA_DialogueSystem;
using JPA_DialogueSystem.Utils;
using UnityEngine;
using UnityEngine.Playables;

public class LevelOneProgression : ILevelProgressor
{
    [SerializeField]
    private AudioManager _audioManager;
    [SerializeField]
    private List<int> rangesWhereTriple;
    [SerializeField]
    private int noPatterns;
    [SerializeField]
    private int startingBeat;
    [SerializeField]
    private PatternType beatType;
    [Header("Dependencies when no tutorial plays")]
    public GameObject beatLines;
    public BGMovementManagerLv1 backgroundMover;
    public List<ParticleSystem> bubbles = new();
    public PlayableDirector noTutorialDirector;
    private void BuildMap(RhythmTrack track)
    {
        List<RhythmMap.BeatInformation> beatsToAdd = new();
        int rangeIndex = 0;
        int nextThreeCounter = 0;
        int extraDuts = 0;
        float newStartingBeatCounter = startingBeat;
        for (int i = 0; i < rangesWhereTriple.Count; i++)
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
        track.Map.SetWholeMapInfo(beatsToAdd);
    }
    private bool isPlayingCutscene = false;
    public override void StartRhythmTrack(RhythmTrack rhythmTrack, PatternManager patternManager, bool trackScore, Track introTrack = null, PlayableDirector cinematicIntro = null, bool playedTutorial = false)
    {
        if (!playedTutorial)
        {
            isPlayingCutscene = true;
            noTutorialDirector.Play();
            StartCoroutine(Timers.GenericTimer((float)noTutorialDirector.duration, () =>
            {
                StartRhythmTrack(rhythmTrack, patternManager, trackScore, introTrack, cinematicIntro, true);
                Camera.main.orthographicSize = 5f;
                isPlayingCutscene = false;
            }));
            return;
        }
        if (cinematicIntro != null)
        {
            cinematicIntro.Play();
        }
        backgroundMover.enabled = true;
        beatLines.SetActive(true);
        bubbles.ToList().ForEach(i => i.Play());
        ScoreTally.ResetScore();
        if (!introTrack)
        {
            _audioManager.PlayMusicTrack(rhythmTrack, true, patternManager);
            return;
        }
        _audioManager.PlayMusicTrack(introTrack, true);
        StartCoroutine(Timers.GenericTimer(introTrack.MusicClip.length, () =>
        {
            _audioManager.PlayMusicTrack(rhythmTrack, true, patternManager);
        }));
    }

    public override void StartTrack(Track track)
    {
        _audioManager.PlayMusicTrack(track, true);
    }
    public override void Pause(bool isPaused)
    {
        base.Pause(isPaused);
        if (isPlayingCutscene)
        {
            if (isPaused)
            {
                noTutorialDirector.Pause();
            }
            else
            {
                noTutorialDirector.Resume();
            }
        }
    }

    public override void LoadRelevantAssets(RhythmTrack rhythmTrack)
    {
        BuildMap(rhythmTrack);
    }
}
