using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : IInputReciever
{
    [SerializeField]
    private GameObject pauseMenuObject;
    public delegate void BoolDelegate(bool isPaused);
    public static event BoolDelegate OnPaused;

    void Start()
    {
        TogglePauseMenu(false);
    }
    public void TogglePauseMenu()
    {
        pauseMenuObject.SetActive(!pauseMenuObject.activeSelf);
        Timers.PauseTimers = pauseMenuObject.activeSelf;
        if (pauseMenuObject.activeSelf)
            Physics2D.simulationMode = SimulationMode2D.Script;
        else
            Physics2D.simulationMode = SimulationMode2D.FixedUpdate;

        OnPaused?.Invoke(pauseMenuObject.activeSelf);
    }
    public void TogglePauseMenu(bool activatePause)
    {
        pauseMenuObject.SetActive(activatePause);
        Timers.PauseTimers = activatePause;
        if (activatePause)
            Physics2D.simulationMode = SimulationMode2D.Script;
        else
            Physics2D.simulationMode = SimulationMode2D.FixedUpdate;

        OnPaused?.Invoke(activatePause);
    }

    public override void ExecuteInput(InputType type)
    {
        if (type == InputType.Escape)
        {
            TogglePauseMenu();
        }
    }
}
