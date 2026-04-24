using System;
using System.Collections;
using System.Collections.Generic;
using JPA_DialogueSystem;
using JPA_DialogueSystem.Utils;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;

public class LevelManager : IPausable
{
    [Header("Dependencies")]
    public RhythmTrack mainTrack;
    public Track introTrack;
    public ResultsMenu resultsMenu;
    public PatternManager patternManager;
    public ILevelProgressor levelProgressor;
    public EndMusicManager endMusicManager;
    public DialogueManager dialogueManager;
    public InputObserver inputObserver;
    public PlayableDirector cinematicIntro;
    public TutorialManager tutorialManager;
    [Header("Settings")]
    public bool WillPlayTutorial = false;
    private bool IsPlayingTutorial = false;

    void Awake()
    {
        resultsMenu = FindFirstObjectByType<ResultsMenu>(); //ToImprove: Move to dedicated injector.
        patternManager = FindFirstObjectByType<PatternManager>(); //ToImprove: Move to dedicated injector.
        levelProgressor = FindFirstObjectByType<ILevelProgressor>();
        dialogueManager = FindFirstObjectByType<DialogueManager>();
        inputObserver = FindFirstObjectByType<InputObserver>();
    }
    void Start()
    {
        levelProgressor.LoadRelevantAssets(mainTrack);
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        Conductor.TrackEnd += OnTrackEnd;
        Conductor.TrackReset += OnTrackReset;
        if (IsPlayingTutorial)
        {
            tutorialManager.OnEndTutorial.AddListener(StartLevelProper);

        }
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        Conductor.TrackEnd -= OnTrackEnd;
        Conductor.TrackReset -= OnTrackReset;
        if (IsPlayingTutorial)
        {
            tutorialManager.OnEndTutorial.RemoveListener(StartLevelProper);

        }
    }
    public void StartTutorialSequence()
    {
        ScoreTally.IsTrackingScore = false;
        if (tutorialManager)
        {
            IsPlayingTutorial = true;
            tutorialManager.StartTutorial();
            tutorialManager.OnEndTutorial.AddListener(StartLevelProper);
        }
        else
        {
            Debug.LogError("No Tutorial Registered. Skipping to main level...");
            StartLevelProper();
        }
    }
    public void StartLevelProper()
    {
        inputObserver.ChangeControlSchema(ControlSchemas.GAMEPLAY);
        if (tutorialManager) tutorialManager.OnEndTutorial.RemoveListener(StartLevelProper);
        ScoreTally.IsTrackingScore = true;
        levelProgressor.StartRhythmTrack(mainTrack, patternManager, true, introTrack, cinematicIntro, IsPlayingTutorial);
        IsPlayingTutorial = false;
    }
    private void OnTrackReset(RhythmTrack track)
    {
        patternManager.ResetCurrentMap(track);
    }
    private void OnTrackEnd(RhythmTrack rhythmTrack)
    {
        patternManager.StopPlayableMap();
        if (rhythmTrack.name != mainTrack.name) return;
        if (IsPlayingTutorial) return;
        //If it's a main track, show the results, or do whatever's next.
        StartCoroutine(Timers.GenericTimer(2.0f, () =>
                {
                    ScoreType finalResult = this.resultsMenu.ComputeResults(rhythmTrack);
                    this.endMusicManager.PlayEndJingle(finalResult);
                })
        );
    }
    public override void Pause(bool isPaused)
    {
        base.Pause(isPaused);
        if (isPaused) cinematicIntro.Pause();
        else cinematicIntro.Resume();

    }
}
