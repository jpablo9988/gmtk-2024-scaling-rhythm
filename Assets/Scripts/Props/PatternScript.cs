using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternScript : MonoBehaviour
{
    [Tooltip("Dependencies")]
    [SerializeField]
    private ParticleSystem currParticleSystem;
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private Animator animator;
    [Tooltip ("Attributes")]
    [SerializeField]
    private float deathParticleTimer = 0.5f;
    private bool isInWindow = false;
    private void OnEnable()
    {
        currParticleSystem.Stop();
        ScoreObserver.OnPlayerInput += GetCatched;
    }

    private void OnDisable()
    {
        ScoreObserver.OnPlayerInput -= GetCatched;

    }
    private void GetCatched(ScoreType score)
    {
        if ((int)score <= 2 && isInWindow)
        {
            isInWindow = false;
            ScoreTally.AddToScore(score);
            this.spriteRenderer.enabled = false;
            currParticleSystem.Play();
            StartCoroutine(Timers.GenericTimer(deathParticleTimer, () =>
            {
                Debug.Log("Enter here!");
                currParticleSystem.Stop();
                Destroy(gameObject);
            }));
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Window"))
        {
            isInWindow = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Window"))
        {
            isInWindow = false;
        }
    }
    public void InitiatePattern(float beatType, Conductor conductor, Sprite[] sprite)
    {
        float animSpeed = (conductor.BPM / 60);
        this.spriteRenderer.sprite = sprite[Random.Range(0, sprite.Length)];
        animator.SetFloat("speed", animSpeed);
        animator.Play("VerticalMove");
    }
}
