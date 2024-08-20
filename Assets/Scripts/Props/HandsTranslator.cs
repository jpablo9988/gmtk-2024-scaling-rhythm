using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandsTranslator : MonoBehaviour
{
    [SerializeField]
    private float moveAmount = 1.0f;
    [SerializeField]
    private float speed = 10.0f;
    [SerializeField]
    private float moveStarter = 4;
    [SerializeField]
    private float rockPerMove = 1;
    void OnEnable()
    {
        ScoreObserver.OnPlayerInput += MoveUpwardsSmooth;
    }
    private void OnDisable()
    {
        ScoreObserver.OnPlayerInput -= MoveUpwardsSmooth;

    }
    public void MoveUpwardsSmooth(ScoreType type)
    {
        if (ScoreTally.TotalScore >= moveStarter && (int)type <= 2)
        {
            if (ScoreTally.TotalScore % rockPerMove == 0)
            {
                Vector2 targetVector = new(transform.position.x, transform.position.y + moveAmount);
                StartCoroutine(MoveTowardsTarget(targetVector));
            }
        }
    }

    private IEnumerator MoveTowardsTarget(Vector2 target)
    {
        while (!this.transform.position.Equals( target))
        {
            transform.position = Vector2.MoveTowards(this.transform.position, target, speed * Time.deltaTime);
            yield return null;
        }
    }
}
