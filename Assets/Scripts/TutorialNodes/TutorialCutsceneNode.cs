using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;

public class TutorialCutsceneNode : ITutorialNode
{
    [Header("Specific Cutscene Dependencies")]
    [SerializeField]
    private PlayableDirector cutsceneDirector;
    protected override void OnEnable()
    {
        base.OnEnable();
        if (IsActive && cutsceneDirector)
        {
            cutsceneDirector.stopped += CleanCutsceneDirector;
        }
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        if (IsActive && cutsceneDirector)
        {
            cutsceneDirector.stopped -= CleanCutsceneDirector;
        }
    }
    public override void ExecuteNode(System.Action onEndExecute)
    {
        base.ExecuteNode(onEndExecute);
        if (cutsceneDirector) cutsceneDirector.Play();
        else
        {
            Debug.LogError("No Cutscene Assigned to TutorialCutsceneNode " + gameObject.name + ". Skipping...");
            CleanNode();
        }
        cutsceneDirector.stopped += CleanCutsceneDirector;
    }
    private void CleanCutsceneDirector(PlayableDirector playableDirector)
    {
        CleanNode();
    }
    public override void CleanNode()
    {
        cutsceneDirector.stopped -= CleanCutsceneDirector;
        base.CleanNode();
    }
    public override void Pause(bool isPaused)
    {
        base.Pause(isPaused);
        if (isPaused)
        {
            cutsceneDirector.Pause();
        }
        else
        {
            cutsceneDirector.Resume();
        }
    }
}
