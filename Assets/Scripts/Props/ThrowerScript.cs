using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowerScript : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject rock;
    [SerializeField] private GameObject spawnPoint;
    [SerializeField] private Conductor conductor;
    void OnEnable()
    {
        PatternManager.beatTelegraph += DoThrowAnimation;
    }
    private void OnDisable()
    {
        PatternManager.beatTelegraph -= DoThrowAnimation;
    }
    public void DoThrowAnimation(float beatType)
    {
        Debug.Log(beatType);
        animator.CrossFade("Throw", 0);

        GameObject newRock = Instantiate(rock, spawnPoint.transform.position, spawnPoint.transform.rotation);
        RockScript rockAnim = newRock.GetComponentInChildren<RockScript>();
        rockAnim.InitiateRock(beatType, conductor);
        //no clue how this works
        /* if (beatType == 2)
        {
            rockRb.AddForce(new Vector2(-2.5f, 20), ForceMode2D.Impulse);
        } else if (beatType == 0.5f)
        {
            rockRb.gravityScale = 12;
            rockRb.AddForce(new Vector2(-2.5f, 20), ForceMode2D.Impulse);
        } */
    }
}
