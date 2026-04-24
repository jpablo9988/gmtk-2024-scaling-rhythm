using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Events;

public class ThrowerScript : MonoBehaviour
{
    //Reference to the right hand animator.
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject rock;
    [SerializeField] private GameObject spawnPoint;
    [SerializeField] private Conductor conductor;
    [SerializeField] private CinemachineSmoothPath rockPath_Small;
    [SerializeField] private CinemachineSmoothPath rockPath_Fail;
    [SerializeField] private CinemachineSmoothPath rockPath_Big;

    public UnityEvent onThrow;
    void OnEnable()
    {
        PatternManager.beatTelegraph += DoThrowAnimation;
    }
    private void OnDisable()
    {
        PatternManager.beatTelegraph -= DoThrowAnimation;
    }
    public void DoThrowAnimation(PatternManager.TelegraphPackage pck)
    {
        float beatsUntilHit = pck.beatsUntilHit;
        animator.CrossFade("Throw", 0); //Do Right hand animation.
        GameObject newRock = Instantiate(rock, spawnPoint.transform.position, spawnPoint.transform.rotation);
        RockScript rockReference = newRock.GetComponentInChildren<RockScript>();
        if (beatsUntilHit == 2)
        {
            rockReference.RockPath = rockPath_Small;
        }
        else if (beatsUntilHit == 3)
        {
            rockReference.RockPath = rockPath_Big;
        }
        rockReference.FailPath = rockPath_Fail;
        rockReference.InitiateRock(beatsUntilHit, conductor);
        onThrow?.Invoke();
    }
}
