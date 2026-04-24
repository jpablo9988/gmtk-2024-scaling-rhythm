using JPA_DialogueSystem;
using UnityEngine;
using UnityEngine.Playables;

public class LevelTwoProgression : ILevelProgressor
{
    [SerializeField]
    private AudioManager _audioManager;
    [SerializeField]
    private CatcherScript catcher;
    [SerializeField]
    private HandsTranslator startingHandsGameObject;
    public PlayableDirector noTutorialDirector;

    public override void StartRhythmTrack(RhythmTrack rhythmTrack, PatternManager patternManager, bool trackScore, Track introTrack = null, PlayableDirector initialCutscene = null, bool playedTutorial = false)
    {
        if (!playedTutorial && noTutorialDirector)
        {
            isPlayingCutscene = true;
            noTutorialDirector.Play();
            StartCoroutine(Timers.GenericTimer((float)noTutorialDirector.duration, () =>
                        {
                            StartRhythmTrack(rhythmTrack, patternManager, trackScore, introTrack, initialCutscene, true);
                            Camera.main.orthographicSize = 5f;
                            isPlayingCutscene = false;
                        }));
            return;
        }
        if (initialCutscene != null)
        {
            initialCutscene.Play();
        }
        ScoreTally.ResetScore();
        if (!introTrack)
        {
            _audioManager.PlayMusicTrack(rhythmTrack, true, patternManager);
            catcher.SetCatcherIdleSpeed(rhythmTrack.BPM / 60);
            return;
        }
        _audioManager.PlayMusicTrack(introTrack, true);
        StartCoroutine(Timers.GenericTimer(introTrack.MusicClip.length, () =>
        {
            _audioManager.PlayMusicTrack(rhythmTrack, true, patternManager);
            catcher.SetCatcherIdleSpeed(rhythmTrack.BPM / 60);
        }));
    }
    public void StartCatcherIdle(RhythmTrack rhythmTrack)
    {
        catcher.SetCatcherIdleSpeed(rhythmTrack.BPM / 60);
    }
    public void StopCatcherIdle()
    {
        catcher.SetCatcherIdleSpeed(0);

    }
    public override void StartTrack(Track track)
    {
        _audioManager.PlayMusicTrack(track, true);
    }
    private bool isPlayingCutscene = false;
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
        return;
    }
}
