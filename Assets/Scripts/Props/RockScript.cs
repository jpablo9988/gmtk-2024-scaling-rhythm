using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] private Animator animator;
    private void OnEnable()
    {
        ScoreObserver.OnPlayerInput += GetCatched;
    }

    private void OnDisable()
    {
        ScoreObserver.OnPlayerInput -= GetCatched;

    }
    private void GetCatched(ScoreType score)
    {
        if ((int)score <= 2)
        {
            if (!catched && touchingHand)
            {
                rb.velocity = Vector2.zero;
                rb.constraints = RigidbodyConstraints2D.FreezePositionX;
                catched = true;
                rb.isKinematic = false;
                animator.SetTrigger("Stay");            
            }
        }
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

        float animSpeed = (conductor.BPM / 60) * (1 / beatType);
        animator.SetFloat("throwSpeed", animSpeed);
        animator.Play("RockParabolla");
    }
}
