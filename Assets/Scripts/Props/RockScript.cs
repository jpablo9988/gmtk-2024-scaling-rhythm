using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class RockScript : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Collider2D currCollider;
    [SerializeField] private Sprite[] smallRockSprites;
    [SerializeField] private Sprite[] bigRockSprites;
    [SerializeField] private Sprite[] gemSprites;
    [SerializeField] private bool touchingHand = false;
    [SerializeField] private bool catched;
    [SerializeField] private SpriteRenderer sprRenderer;
    [SerializeField] private SyncedAnimation animator;
    [SerializeField] private SpriteRotator spriteRotator;
    [SerializeField]
    private ObjectDestroyer objectDestroyer;


    public CinemachineSmoothPath RockPath;
    public CinemachineSmoothPath FailPath;

    private void OnEnable()
    {
        ScoreObserver.OnGainScore += GetCatched;
        animator.animationEnd.AddListener(OnAnimationEnd);
    }

    private void OnDisable()
    {
        ScoreObserver.OnGainScore -= GetCatched;
        animator.animationEnd.RemoveListener(OnAnimationEnd);
    }
    private void OnAnimationEnd(SyncedAnimation.OnEnd_Package package)
    {
        if (package.source != animator) return;
        if (package.path == RockPath && !catched)
        {
            package.source.PlayAnimation(FailPath, 0.5f);
        }
        else if (package.path == FailPath && !catched)
        {
            sprRenderer.color = Color.red;
            SwitchToPhysicsBasedMovement();
            objectDestroyer.DestroyObject(true);
        }
    }
    private void GetCatched(ScoreType score)
    {
        if ((int)score <= 1)
        {
            if (!catched && touchingHand)
            {
                catched = true;
                SwitchToPhysicsBasedMovement();

            }
        }
    }
    private void SwitchToPhysicsBasedMovement()
    {
        rb.velocity = Vector2.zero;
        rb.constraints = RigidbodyConstraints2D.FreezePositionX;
        rb.isKinematic = false;
        if (spriteRotator) spriteRotator.enabled = false;
        animator.PlayAnimation(null, 1.0f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Hand"))
        {
            touchingHand = true;
        }
        if (collision.gameObject.CompareTag("Floor"))
        {
            touchingHand = false;
            currCollider.isTrigger = true;
            rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezePositionX;
        }
        if (collision.gameObject.CompareTag("Rock") && catched)
        {
            currCollider.isTrigger = true;
            rb.isKinematic = true;
            rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezePositionX;
        }
        if (collision.gameObject.CompareTag("Destructor"))
        {
            objectDestroyer.DestroyObject(true);
        }
    }
    public void InitiateRock(float beatType, Conductor conductor)
    {
        switch (beatType)
        {
            case 2:
                sprRenderer.sprite = smallRockSprites[Random.Range(0, smallRockSprites.Length)];
                break;
            case 3:
                sprRenderer.sprite = bigRockSprites[Random.Range(0, bigRockSprites.Length)];
                break;
        }
        //beatType (in Seconds) times the time it takes to finish a bar in the current song
        animator.PlayAnimation(RockPath, beatType * (1 / (conductor.BPM / 60)), SyncedAnimation.SyncedMovementType.Parabola);
        catched = false;
    }
}
