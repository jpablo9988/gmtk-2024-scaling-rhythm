
using UnityEngine;

public abstract class IInputReciever : IPausable
{
    [Header("Type of Control Schema")]
    [SerializeField]
    protected ControlSchemas recievingSchema = ControlSchemas.GAMEPLAY;
    protected bool isRecievingInputs = false;
    protected override void OnEnable()
    {
        base.OnEnable();
        InputObserver.OnChangeSchema += IsCurrentlyActive;
        if (recievingSchema == ControlSchemas.ALWAYS)
        {
            isRecievingInputs = true;
            InputObserver.OnPlayerInput += ExecuteInput;
            return;
        }
        if (isRecievingInputs || recievingSchema == ControlSchemas.ALWAYS)
        { InputObserver.OnPlayerInput += ExecuteInput; }

    }
    protected override void OnDisable()
    {
        base.OnDisable();
        InputObserver.OnChangeSchema -= IsCurrentlyActive;
        if (recievingSchema == ControlSchemas.ALWAYS)
        {
            InputObserver.OnPlayerInput -= ExecuteInput;
            return;
        }
        if (isRecievingInputs)
        { InputObserver.OnPlayerInput -= ExecuteInput; }


    }
    public void IsCurrentlyActive(ControlSchemas schema)
    {
        if (recievingSchema == ControlSchemas.ALWAYS)
        {
            return;
        }
        if (schema == recievingSchema)
        {
            Debug.Log(recievingSchema);
            if (isRecievingInputs)
            {
                Debug.LogWarning("This function is being called twice. Careful, now!");
                return;
            }
            InputObserver.OnPlayerInput += ExecuteInput;
            isRecievingInputs = true;
        }
        else
        {
            InputObserver.OnPlayerInput -= ExecuteInput;
            isRecievingInputs = false;
        }
    }
    public abstract void ExecuteInput(InputType type);
}
