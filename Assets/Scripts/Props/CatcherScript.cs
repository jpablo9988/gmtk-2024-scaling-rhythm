using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatcherScript : IPausable
{
    [SerializeField] private Animator animator;
    protected override void OnEnable()
    {
        base.OnEnable();
        ScoreObserver.OnGainScore += DoCatchAnimation;
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        ScoreObserver.OnGainScore -= DoCatchAnimation;
    }

    private void DoCatchAnimation(ScoreType score)
    {
        if ((int)score <= 2)
        {
            ScoreTally.AddToScore(score);
        }
        if (score != ScoreType.Miss)
        {
            if (score == ScoreType.Poor || score == ScoreType.Other)
            {
                animator.CrossFade("Miss", 0);
            }
            else
            {
                animator.CrossFade("Catch", 0);
            }
        }
    }
    private float currentCatchedIdleSpeed = 0f;
    public void SetCatcherIdleSpeed(float speed)
    {
        currentCatchedIdleSpeed = speed;
        animator.SetFloat("speed", currentCatchedIdleSpeed);
    }
    public override void Pause(bool isPaused)
    {
        if (isPaused) animator.SetFloat("speed", 0f);
        else animator.SetFloat("speed", currentCatchedIdleSpeed);
    }

}
