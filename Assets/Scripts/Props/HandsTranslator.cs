using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HandsTranslator : IPausable
{
    [SerializeField]
    private float moveAmount = 1.0f;
    [SerializeField]
    private float speed = 10.0f;
    [SerializeField]
    private float moveStarter = 4;
    [SerializeField]
    private float rockPerMove = 1;
    [SerializeField]
    private float rateZoomout = 3f;
    public UnityEvent OnEndMovement;
    protected override void OnEnable()
    {
        base.OnEnable();
        ScoreObserver.OnGainScore += MoveUpwardsSmooth;
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        ScoreObserver.OnGainScore -= MoveUpwardsSmooth;
    }
    public void MoveUpwardsSmooth(ScoreType type)
    {
        if (!ScoreTally.IsTrackingScore) return;
        if (ScoreTally.TotalScore >= moveStarter && (int)type <= (int)ScoreType.Good)
        {
            if (ScoreTally.TotalScore % rockPerMove == 0)
            {
                MoveSmooth(transform.position.y + moveAmount);
            }
        }
    }
    public void MoveSmooth(float positionY)
    {
        Vector2 targetVector = new(transform.position.x, positionY);
        StartCoroutine(MoveTowardsTarget(targetVector));
    }

    private IEnumerator MoveTowardsTarget(Vector2 target)
    {
        while (!this.transform.position.Equals(target))
        {
            if (isGamePaused)
            {
                yield return null;
            }
            else
            {
                transform.position = Vector2.MoveTowards(this.transform.position, target, speed * Time.deltaTime);
                Camera.main.orthographicSize += Time.deltaTime * rateZoomout;
                yield return null;
            }
        }
        OnEndMovement?.Invoke();
    }
}
