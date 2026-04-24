using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages all inputs coming from the player in regards to Gameplay.
/// </summary>
public class PlayerInputs : MonoBehaviour
{
    [SerializeField]
    private string b1_Name = "Button1";
    [SerializeField]
    private string b2_name = "Button2";
    [SerializeField]
    private string escape_name = "Cancel";
    [SerializeField]
    private InputObserver observer;

    // Update is called once per frame
    void Update()
    {

        if (Input.GetButtonDown(b1_Name))
        {
            observer.PlayerInputs(InputType.Button1);
            return;
        }
        if (Input.GetButtonDown(b2_name))
        {
            observer.PlayerInputs(InputType.Button2);
            return;
        }
        if (Input.GetButtonDown(escape_name))
        {
            observer.PlayerInputs(InputType.Escape);
            return;
        }

    }
}
