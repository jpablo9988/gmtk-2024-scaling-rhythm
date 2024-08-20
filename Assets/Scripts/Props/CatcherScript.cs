using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatcherScript : MonoBehaviour
{
    [SerializeField] private Animator animator;
    void OnEnable()
    {
        ScoreObserver.OnPlayerInput += DoCatchAnimation;
    }
    private void OnDisable()
    {
        ScoreObserver.OnPlayerInput -= DoCatchAnimation;

    }

    private void DoCatchAnimation(ScoreType score)
    {
        if ((int)score <= 2)
        {
            Debug.Log(score);
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
    public void SetCatcherIdleSpeed(float speed)
    {
        animator.SetFloat("speed", speed);
    }
}
