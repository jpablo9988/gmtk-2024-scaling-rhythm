using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class ObjectDestroyer : IPausable
{
    [Header("Fade Properties")]
    [SerializeField]
    private float timeUntilIsDestroyed = 0.5f;
    [SerializeField]
    [Description("If is set to null, onDestroyFade will not happen.")]
    private SpriteRenderer spriteRenderer;

    public void DestroyObject(bool onDestroyFade)
    {
        if (onDestroyFade && spriteRenderer == null)
        {
            Debug.LogWarning("Sprite Renderer is not asigned to Object Destroyer");
            Destroy(gameObject);
            return;
        }
        if (!onDestroyFade)
        {
            Destroy(gameObject);
            return;
        }
        StartCoroutine(FadeOutObject(timeUntilIsDestroyed));
        Destroy(gameObject, timeUntilIsDestroyed);
    }
    IEnumerator FadeOutObject(float timer)
    {
        float initalTimer = timer;
        while (timer > 0)
        {
            if (isGamePaused) yield return null;
            else
            {
                timer -= Time.deltaTime;
                Color auxColor = spriteRenderer.color;
                auxColor.a = Mathf.InverseLerp(0, 1, timer / initalTimer);
                spriteRenderer.color = auxColor;
                yield return 0;
            }
        }
    }
}
