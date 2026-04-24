
using Unity.VisualScripting;
using UnityEngine;

public abstract class IPausable : MonoBehaviour
{
    public virtual void Pause(bool isPaused)
    {
        isGamePaused = isPaused;
    }
    protected bool isGamePaused = false;
    protected virtual void OnEnable()
    {
        PauseMenu.OnPaused += Pause;
    }
    protected virtual void OnDisable()
    {
        PauseMenu.OnPaused -= Pause;
    }
}
