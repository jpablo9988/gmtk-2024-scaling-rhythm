using System;
using System.Collections;
using System.Collections.Generic;
using JPA_DialogueSystem;
using JPA_DialogueSystem.Utils;
using UnityEngine;

public class TutorialDialogueNode : ITutorialNode
{
    [Header("Specific Cutscene Dependencies")]
    [SerializeField]
    private DialogueManager dialogueManager;
    [SerializeField]
    private ScriptableStory dialogueContents;
    [SerializeField]
    private InputObserver inputObserver;
    [SerializeField]
    private float delayDialogueBy = 0.0f;
    private IEnumerator dialogueEnumerator;
    private bool waitingOnDialogueEnumerator = false;
    void Start()
    {
        dialogueManager = GetDependant(dialogueManager);
        inputObserver = GetDependant(inputObserver);
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        if (IsActive)
        {
            DialogueManager.StoryEnd += CleanNode;
        }
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        if (IsActive)
        {
            DialogueManager.StoryEnd -= CleanNode;
        }
    }
    public override void ExecuteNode(Action onEndExecute)
    {
        base.ExecuteNode(onEndExecute);
        if (dialogueContents)
        {
            if (delayDialogueBy > 0f)
            {
                waitingOnDialogueEnumerator = true;
                dialogueEnumerator = Timers.GenericTimer(delayDialogueBy, () =>
                {
                    dialogueManager.StartStory(dialogueContents);
                    waitingOnDialogueEnumerator = false;
                });
                StartCoroutine(dialogueEnumerator);
            }
            else
            {
                waitingOnDialogueEnumerator = false;
                dialogueManager.StartStory(dialogueContents);
            }
            if (inputObserver.currentControlSchema != ControlSchemas.DIALOGUE) inputObserver.ChangeControlSchema(ControlSchemas.DIALOGUE);
        }
        else
        {
            Debug.LogError("No Dialogue Contents assigned to TutorialDialogueNode " + name + ". Skipping...");
            CleanNode();
            return;
        }
        DialogueManager.StoryEnd += CleanNode;
    }
    public override void CleanNode()
    {
        DialogueManager.StoryEnd -= CleanNode;
        if (waitingOnDialogueEnumerator)
        {
            StopCoroutine(dialogueEnumerator);
            waitingOnDialogueEnumerator = false;
        }

        base.CleanNode();
    }
}
