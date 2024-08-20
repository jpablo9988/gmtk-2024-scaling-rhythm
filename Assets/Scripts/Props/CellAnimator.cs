using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class CellAnimator : MonoBehaviour
{
    [SerializeField]
    private string[] animStates; // -- index corresponds to enum cellSizes
    [SerializeField]
    private int[] ranges;
    [SerializeField] private Animator animator;
    void OnEnable()
    {
        ScoreObserver.OnPlayerInput += DoEatAnimation;
    }
    private void OnDisable()
    {
        ScoreObserver.OnPlayerInput -= DoEatAnimation;

    }
    private void DoEatAnimation(ScoreType score)
    {
        //build anim string
        if (score != ScoreType.Miss)
        {
            int index = 0;
            foreach (int range in ranges)
            {
                if (ScoreTally.TotalScore >= range)
                {
                    index++;
                }
            }
            this.animator.Play(animStates[index], -1, 0);
        }
    }
}
