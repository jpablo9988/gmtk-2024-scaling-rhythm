using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockScript : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Collider2D collider;
    [SerializeField] private Sprite[] smallRockSprites;
    [SerializeField] private Sprite[] bigRockSprites;
    [SerializeField] private Sprite[] gemSprites;
    [SerializeField] private bool touchingHand;
    [SerializeField] private bool catched;
    [SerializeField] private SpriteRenderer renderer;
    void Start()
    {
        ScoreObserver.OnPlayerInput += GetCatched;
        renderer.sprite = smallRockSprites[Random.Range(0,4)];
    }

    private void GetCatched(ScoreType score)
    {
        if (score != ScoreType.Miss)
        {
            if (touchingHand && !catched)
            {
                rb.velocity = Vector2.zero;
                rb.constraints = RigidbodyConstraints2D.FreezePositionX;
                catched = true;
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
            collider.isTrigger = false;
            rb.isKinematic = true;
            rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezePositionX;
        }
        if (collision.gameObject.CompareTag("Rock"))
        {
            if (collision.gameObject.GetComponent<Rigidbody2D>().isKinematic)
            {
                collider.isTrigger = false;
                rb.isKinematic = true;
                rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezePositionX;
            }
        }
    }
}
