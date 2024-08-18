using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputObserver : MonoBehaviour
{
    public delegate void EventWithInputType(InputType type);
    public static event EventWithInputType OnPlayerInput;


    public void PlayerInputs(InputType type)
    {
        OnPlayerInput?.Invoke(type);
    }
}
