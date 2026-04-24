using System;
using Cinemachine;
using UnityEngine;

public class PatternScript : MonoBehaviour
{
    [Tooltip("Dependencies")]
    [SerializeField]
    private ParticleSystem currParticleSystem;
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private SpriteMask spriteMask;
    [SerializeField]
    private SyncedAnimation animManager;
    [SerializeField]
    private Sprite halfNoteSprite;
    [Tooltip("Attributes")]
    [SerializeField]
    private float deathParticleTimer = 0.5f;
    private bool isInWindow = false;

    public CinemachineSmoothPath pathUntilPerfectHit;
    public CinemachineSmoothPath pathFail;
    public static event Action<PatternScript> OnPatternDestroy;

    private ObjectDestroyer objectDestroyer;

    void Start()
    {
        objectDestroyer = GetComponentInParent<ObjectDestroyer>();
    }
    private void OnEnable()
    {
        currParticleSystem.Stop();
        ScoreObserver.OnGainScore += GetCatched;
        animManager.animationEnd.AddListener(OnAnimationEnd);

    }

    private void OnDisable()
    {
        ScoreObserver.OnGainScore -= GetCatched;
        animManager.animationEnd.RemoveListener(OnAnimationEnd);
    }
    public void DestroyPatternObject(bool willFade)
    {
        OnPatternDestroy?.Invoke(this);
        objectDestroyer.DestroyObject(willFade);
    }
    private void OnAnimationEnd(SyncedAnimation.OnEnd_Package package)
    {
        if (package.path == pathFail)
        {
            if (objectDestroyer != null) DestroyPatternObject(false);
        }
        if (package.source != animManager) return;
        if (package.path != pathFail)
        {
            package.source.PlayAnimation(pathFail, package.animDuration);
        }
    }
    private void GetCatched(ScoreType score)
    {
        if ((int)score <= 2 && isInWindow)
        {
            isInWindow = false;
            ScoreTally.AddToScore(score);
            this.spriteRenderer.enabled = false;
            this.spriteMask.enabled = false;
            currParticleSystem.Play();
            StartCoroutine(Timers.GenericTimer(deathParticleTimer, () =>
            {
                currParticleSystem.Stop();
                if (objectDestroyer != null) DestroyPatternObject(false);
                else Destroy(gameObject);
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
    public void InitiatePattern(float beatType, Conductor conductor, Sprite[] sprite, bool isHalfNote)
    {
        this.spriteMask.sprite = sprite[UnityEngine.Random.Range(0, sprite.Length)];
        if (isHalfNote)
        {
            this.spriteRenderer.sprite = halfNoteSprite;
            //Special Particle System?
        }
        animManager.PlayAnimation(pathUntilPerfectHit, beatType * (1 / (conductor.BPM / 60)));
    }
}
