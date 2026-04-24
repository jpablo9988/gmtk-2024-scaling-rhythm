using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialDemoNode : ITutorialNode
{
    [SerializeField]
    private DEMOInputs demoInputs;
    void Start()
    {
        demoInputs = GetDependant(demoInputs);
    }
    public override void ExecuteNode(Action onEndExecute)
    {
        base.ExecuteNode(onEndExecute);
        demoInputs.StartDemo();
    }
    public override void CleanNode()
    {
        demoInputs.StopDemo();
        base.CleanNode();
    }
}
