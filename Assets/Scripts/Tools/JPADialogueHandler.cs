using System.Collections;
using System.Collections.Generic;
using JPA_DialogueSystem.Input;
using UnityEngine;

public class JPADialogueHandler : IInputReciever
{
    [SerializeField]
    private DialogueInputHandler inputHandler;

    void Start()
    {
        inputHandler = FindFirstObjectByType<DialogueInputHandler>();
    }

    public override void ExecuteInput(InputType type)
    {
        if (type == InputType.Button2 || type == InputType.BothButtons)
        {
            inputHandler.HandleDialogue(DialogueInputHandler.DialogueInputType.DIALOGUE_SKIP);
            return;
        }
        if (type == InputType.Button1)
        {
            inputHandler.HandleDialogue(DialogueInputHandler.DialogueInputType.DIALOGUE_FOWARD);
            return;
        }

    }
}
