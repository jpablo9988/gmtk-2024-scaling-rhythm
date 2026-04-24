using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class BGColumnMovementsLv1 : IPausable
{

    public float ScreenBorder = 0f;
    public UnityEvent<Transform, BGColumnMovementsLv1> onReachBorder;
    public SpriteRenderer[] childRenderers;
    void Awake()
    {
        childRenderers = GetComponentsInChildren<SpriteRenderer>();
    }
    void Update()
    {
        if (ScreenBorder != 0f)
        {
            if (transform.position.x <= ScreenBorder)
            {
                onReachBorder?.Invoke(this.transform, this);
            }
        }
    }
    public void TransitionToSprite(Conductor conductor, Sprite newSprite, int index)
    {
        if (index >= childRenderers.Length)
        {
            return;
        }
        StartCoroutine(AnimateTransition(
            conductor.BPS,
            childRenderers[index], newSprite,
        () =>
        {
            Debug.Log(index++);
            TransitionToSprite(conductor, newSprite, index++);
        }));
    }
    private IEnumerator AnimateTransition(float duration, SpriteRenderer target, Sprite newSprite, Action next)
    {
        float timer = 0;
        bool hasChangedSprite = false;
        float currentDegree;
        while (timer < duration)
        {
            while (isGamePaused)
            {
                yield return null;
            }
            if (timer <= duration / 2)
            {
                currentDegree = Mathf.Lerp(0, 90, timer / duration * 2);
            }
            else
            {
                currentDegree = Mathf.Lerp(-90, 0, ((timer / duration) - 0.5f) * 2);
                if (!hasChangedSprite)
                {
                    target.sprite = newSprite;
                    hasChangedSprite = true;
                }
            }
            target.transform.rotation = Quaternion.Euler(new Vector3(0, currentDegree, 0));
            timer += Time.deltaTime;
            yield return null;

        }
        target.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        Debug.Log("Animation finished");
        next?.Invoke();
    }
}
