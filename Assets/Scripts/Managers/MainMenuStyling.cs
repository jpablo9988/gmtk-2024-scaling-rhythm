using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuStyling : MonoBehaviour
{
    private static readonly int BoopHash = Animator.StringToHash("Boop");
    [SerializeField]
    private RhythmTrack mainMenuTrack;

    [SerializeField]
    private AudioManager audioManager;

    [SerializeField]
    private Animator titleAnimator;

    private Conductor conductor;

    void Start()
    {
        audioManager.IsMusicAffectedByPause(false);
        conductor = audioManager.BaseConductor;
        audioManager.PlayMusicTrack(mainMenuTrack, false);
        InvokeRepeating(nameof(PlayTitleAnimation), conductor.BPS, conductor.BPS * 2);
    }
    void OnDisable()
    {
        CancelInvoke();
    }
    private void PlayTitleAnimation()
    {
        titleAnimator.SetTrigger(BoopHash);
    }
}
