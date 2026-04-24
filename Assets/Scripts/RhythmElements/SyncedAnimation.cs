using System;
using Cinemachine;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

public class SyncedAnimation : IPausable
{
    public struct OnEnd_Package
    {
        public SyncedAnimation source;
        public CinemachineSmoothPath path;
        public float animDuration;
    }
    public enum SyncedMovementType
    {
        Linear,
        Parabola
    }
    [SerializeField]
    private CinemachineDollyCart rockPositionDolly;
    private float timeToReachEnd = 0;
    private bool startedAnim = false;
    private float currTimeInLerp = 0;

    private SyncedMovementType type;

    public UnityEvent<OnEnd_Package> animationEnd;


    //Then add the following code to Update() to set the animation:
    private float currentVelocity = 1.0f;
    void Update()
    {
        if (startedAnim && !isGamePaused)
        {
            if (type == SyncedMovementType.Linear)
                currTimeInLerp += Time.deltaTime * 1 / timeToReachEnd;
            else
            {
                if (currTimeInLerp < 0.6f)
                {
                    currentVelocity = Mathf.Lerp(2f, 0.5f, currTimeInLerp * 1.6666f);
                }
                else
                {
                    currentVelocity = Mathf.Lerp(0.5f, 2f, (currTimeInLerp - 0.6f) * 2.5f);
                }
                currTimeInLerp += Time.deltaTime * (currentVelocity / timeToReachEnd);
            }
            if (Mathf.Approximately(currTimeInLerp, 1.0f) || currTimeInLerp >= 1.0f)
            {
                OnEndReset();
                return;
            }
            DollyMovement(currTimeInLerp);
        }
    }
    private void OnEndReset()
    {
        startedAnim = false;
        OnEnd_Package sendPackage;
        sendPackage.source = this;
        sendPackage.path = rockPositionDolly.m_Path as CinemachineSmoothPath;
        sendPackage.animDuration = timeToReachEnd;
        currentVelocity = 1.0f;
        animationEnd?.Invoke(sendPackage);
    }
    public void PlayAnimation(CinemachineSmoothPath path, float timeToReachHand, SyncedMovementType type = SyncedMovementType.Linear)
    {
        if (path == null)
        {
            rockPositionDolly.enabled = false;
            return;
        }
        rockPositionDolly.enabled = true;
        startedAnim = true;
        timeToReachEnd = timeToReachHand;
        rockPositionDolly.m_Path = path;
        rockPositionDolly.m_Position = 0;
        currTimeInLerp = 0;
        this.type = type;
    }
    private void DollyMovement(float currTime)
    {
        Assert.IsTrue(startedAnim);
        rockPositionDolly.m_Position = Mathf.Lerp(0, 1, currTime);
    }
}
