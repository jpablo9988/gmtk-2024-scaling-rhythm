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
    private string b2_Name = "Button2";
    [SerializeField]
    private InputObserver observer;

    // Update is called once per frame
    void Update()
    {
        // while button1 or 2 are being held down, check if the other got pushed down to trigger a BothButtons event. 
        if (Input.GetButton(b1_Name))
        {
            if (Input.GetButtonDown(b2_Name))
            {
                observer.PlayerInputs(InputType.BothButtons);
                return;
            }
        }
        if (Input.GetButton(b2_Name))
        {
            if (Input.GetButtonDown(b1_Name))
            {
                observer.PlayerInputs(InputType.BothButtons);
                return;
            }
        }
        if (Input.GetButtonDown(b1_Name))
        {
            observer.PlayerInputs(InputType.Button1);
            return;
        }
        if (Input.GetButtonDown(b2_Name))
        {
            observer.PlayerInputs(InputType.Button2);
            return;
        }

    }
}
