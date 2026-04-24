using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JPA_DialogueSystem;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class TutorialManager : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField]
    private DialogueManager dialoguePanel;
    [SerializeField]
    private PatternManager patternManager;
    [Header("Dependency Options")]
    [SerializeField]
    private bool takeNodesFromChildren = true;
    [Header("Tutorial Nodes")]
    [SerializeField]
    private ITutorialNode[] nodes;
    private ITutorialNode currentNode;
    public UnityEvent OnEndTutorial;
    public void Start()
    {
        if (dialoguePanel == null)
        {
            dialoguePanel = FindFirstObjectByType<DialogueManager>();
        }
        if (patternManager == null)
        {
            patternManager = FindFirstObjectByType<PatternManager>();
        }
        if (takeNodesFromChildren)
        {
            nodes = GetComponentsInChildren<ITutorialNode>().Where(i => i.isDependant == false).ToArray();
        }
    }
    public void StartTutorial(int tutorialIndex = 0)
    {
        if (tutorialIndex >= nodes.Length)
        {
            OnEndTutorial?.Invoke();
            return;
        }
        currentNode = nodes[tutorialIndex];
        currentNode.ExecuteNode(() => { StartTutorial(tutorialIndex + 1); });
    }
}
