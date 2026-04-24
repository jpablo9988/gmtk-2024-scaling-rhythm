using UnityEngine;

public class ScoreVisuals : MonoBehaviour
{
    [Header("Particle Systems")]
    [SerializeField]
    private ParticleSystem perfectParticleSystem;
    [SerializeField]
    private ParticleSystem goodParticleSystem;
    [SerializeField]
    private ParticleSystem mehParticleSystem;
    [SerializeField]
    private ParticleSystem missParticleSystem;

    void OnEnable()
    {
        ScoreObserver.OnGainScore += ShowScoreOnScreen;
    }

    void OnDisable()
    {
        ScoreObserver.OnGainScore -= ShowScoreOnScreen;
    }

    private void ShowScoreOnScreen(ScoreType scoreType)
    {
        Debug.Log(scoreType);
        switch (scoreType)
        {
            case ScoreType.Perfect:
                PlayParticleVisuals(perfectParticleSystem);
                break;
            case ScoreType.Good:
                PlayParticleVisuals(goodParticleSystem);
                break;
            case ScoreType.Meh:
                PlayParticleVisuals(mehParticleSystem);
                break;
            case ScoreType.Poor:
            case ScoreType.Miss:
                PlayParticleVisuals(missParticleSystem);
                break;
        }
    }
    private void PlayParticleVisuals(ParticleSystem scoreParticleSystem)
    {
        scoreParticleSystem.Play();
    }
}
