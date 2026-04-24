using System;
using JPA_DialogueSystem;
using JPA_DialogueSystem.Utils;
using UnityEngine;

public class TutorialPlayableNode : ITutorialNode
{
    [Header("Specific Playable Dependencies")]
    [SerializeField]
    private Barks goodBarks;
    [SerializeField]
    private Barks badBarks;
    [SerializeField]
    private DialogueManager dialogueManager;
    [SerializeField]
    private InputObserver inputObserver;
    [Header("Progression Attributes")]
    [SerializeField]
    private int hitsUntilBark;
    [SerializeField]
    private int correctCyclesUntilEnd;
    //Local Counter Variables
    private int badBarkCounter = 0;
    private int correctHitCounter = 0;
    private int allHitCounter = 0;
    private int correctCyclesCounter = 0;
    void Start()
    {
        dialogueManager = GetDependant(dialogueManager);
        inputObserver = GetDependant(inputObserver);
    }
    private int barkCounter = 0;
    protected override void OnEnable()
    {
        base.OnEnable();
        if (IsActive)
        {
            ScoreObserver.OnGainScore += CheckTutorialScore;
        }
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        if (IsActive)
        {
            ScoreObserver.OnGainScore -= CheckTutorialScore;
        }
    }
    public override void ExecuteNode(Action onEndExecute)
    {
        base.ExecuteNode(onEndExecute);
        if (inputObserver.currentControlSchema != ControlSchemas.GAMEPLAY) inputObserver.ChangeControlSchema(ControlSchemas.GAMEPLAY);
        ScoreObserver.OnGainScore += CheckTutorialScore;
    }
    public override void CleanNode()
    {
        ScoreObserver.OnGainScore -= CheckTutorialScore;
        base.CleanNode();
    }
    private void CheckTutorialScore(ScoreType incoming)
    {
        if (incoming == ScoreType.Other) return;
        if (incoming == ScoreType.Perfect || incoming == ScoreType.Good)
        {
            correctHitCounter++;
        }
        allHitCounter++;
        if (allHitCounter >= hitsUntilBark)
        {
            if (correctHitCounter >= hitsUntilBark)
            {
                correctCyclesCounter++;
                correctHitCounter = 0;
                allHitCounter = 0;
                if (correctCyclesCounter >= correctCyclesUntilEnd)
                {
                    correctCyclesCounter = 0;
                    CleanNode();
                }
                else
                {
                    dialogueManager.DoBark(new string[] { goodBarks.barks[barkCounter] }, false, name: goodBarks.dialogueName, barkSpeed: goodBarks.dialogueSpeed);
                    barkCounter++;
                    if (barkCounter >= goodBarks.barks.Length) barkCounter = 0;
                }
            }
            else
            {
                correctHitCounter = 0;
                allHitCounter = 0;
                dialogueManager.DoBark(new string[] { badBarks.barks[badBarkCounter] }, false, name: goodBarks.dialogueName, barkSpeed: goodBarks.dialogueSpeed);
                badBarkCounter++;
                if (badBarkCounter >= badBarks.barks.Length) badBarkCounter = 0;
            }
        }
    }
}
