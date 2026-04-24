using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputObserver : IPausable
{
    public delegate void EventWithInputType(InputType type);
    public delegate void EventWithControlSchema(ControlSchemas schema);
    public static event EventWithInputType OnPlayerInput;
    public static event EventWithControlSchema OnChangeSchema;
    public ControlSchemas currentControlSchema;

    public void PlayerInputs(InputType type)
    {
        OnPlayerInput?.Invoke(type);
    }
    public void ChangeControlSchema(ControlSchemas controlSchemas)
    {
        currentControlSchema = controlSchemas;
        OnChangeSchema?.Invoke(controlSchemas);
    }

    public override void Pause(bool isPaused)
    {
        base.Pause(isPaused);
        if (isPaused)
        {
            OnChangeSchema?.Invoke(ControlSchemas.NONE);
        }
        else
        {
            ChangeControlSchema(currentControlSchema);
        }
    }
}
