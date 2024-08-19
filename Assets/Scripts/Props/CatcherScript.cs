using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatcherScript : MonoBehaviour
{
    [SerializeField] private Animator animator;
    void Start()
    {
        ScoreObserver.OnPlayerInput += DoCatchAnimation;
    }

    private void DoCatchAnimation(ScoreType score)
    {
        if (score == ScoreType.Perfect)
        {
            animator.CrossFade("Catch", 0);
        } else
        {
            animator.CrossFade("Miss", 0);
        }
    }
}
