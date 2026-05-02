using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanoramicObj : IPausable
{
    public PanoramicMovement.PanoramaType panoramaType;
    public void MoveTowards(float time)
    {
        StartCoroutine(MoveTowardsTarget(time));
    }
    private IEnumerator MoveTowardsTarget(float time)
    {
        float timer = time;
        while (timer >= 0f)
        {
            if (!isGamePaused)
            {
                timer -= Time.deltaTime;
                Vector3 currPosition = transform.position;
                currPosition.y += Time.deltaTime * PanoramicMovement.panoramaSpeedValues[panoramaType];
                transform.position = currPosition;
            }

            yield return null;
        }
    }
}
