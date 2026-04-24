
using JPA_DialogueSystem;
using UnityEngine;

public class PerLetterSFX : MonoBehaviour
{
    [Header("Dependencies")]
    public AudioSource audioSource;
    public AudioClip[] clipsToPlay;

    public float minPitch = 0.95f;
    public float maxPitch = 1.1f;
    public float cooldown = 0f;
    private bool isOnCooldown;
    void OnEnable()
    {
        DialogueManager.LetterIsDiscovered += ReproduceSFX;
    }
    void OnDisable()
    {
        DialogueManager.LetterIsDiscovered -= ReproduceSFX;

    }

    private void ReproduceSFX()
    {
        if (clipsToPlay.Length <= 0)
        {
            Debug.LogError("No Clips in PerLetterSFX");
            return;
        }
        if (audioSource == null)
        {
            Debug.LogError("AudioSource in PerLetterSFX is not set.");
            return;
        }
        if (isOnCooldown) return;
        float currentPitch = Random.Range(minPitch, maxPitch);
        int audioIndex = 0;
        if (clipsToPlay.Length > 1)
        {
            audioIndex = Random.Range(0, clipsToPlay.Length);
        }
        audioSource.pitch = currentPitch;
        audioSource.PlayOneShot(clipsToPlay[audioIndex]);
        if (cooldown > 0f)
        {
            isOnCooldown = true;
            Invoke(nameof(OffCooldown), cooldown);
        }
    }
    private void OffCooldown() { isOnCooldown = false; }
}
