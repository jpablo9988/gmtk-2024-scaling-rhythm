using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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
    private RaycastHit _hit;

    public GraphicRaycaster m_Raycaster;
    PointerEventData m_PointerEventData;
    EventSystem m_EventSystem;

    void Start()
    {
        m_EventSystem = FindFirstObjectByType<EventSystem>();
    }
    // Update is called once per frame
    void Update()
    {

        if (Input.GetButtonDown(b1_Name))
        {
            if (m_Raycaster)
            {
                m_PointerEventData = new PointerEventData(m_EventSystem)
                {
                    //Set the Pointer Event Position to that of the mouse position
                    position = Input.mousePosition
                };

                //Create a list of Raycast Results
                List<RaycastResult> results = new();
                //Raycast using the Graphics Raycaster and mouse click position
                m_Raycaster.Raycast(m_PointerEventData, results);

                //For every result returned, output the name of the GameObject on the Canvas hit by the Ray
                foreach (RaycastResult result in results)
                {
                    if (result.gameObject.CompareTag("UI"))
                    {
                        return;
                    }
                }
            }
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
