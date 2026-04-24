using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class DEMOInputs : IPausable
{
    public PatternManager patternManager;
    public Conductor conductor;
    private bool isDemoing;
    List<IEnumerator> currentProcesess = new();

    protected override void OnEnable()
    {
        base.OnEnable();
        PatternManager.beatTelegraph += RecieveBeatInstance;
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        PatternManager.beatTelegraph -= RecieveBeatInstance;
    }
    void Start()
    {
        if (patternManager == null)
        {
            patternManager = FindFirstObjectByType<PatternManager>();
        }
        if (conductor == null)
        {
            conductor = FindFirstObjectByType<Conductor>();
        }
    }

    private void RecieveBeatInstance(PatternManager.TelegraphPackage pck)
    {
        if (!isDemoing) return;
        if (conductor == null || patternManager == null)
        {
            Debug.LogError("DEMOInputs didn't find appropiate dependenies for Conductor or PatterManager");
            return;
        }
        IEnumerator auxEnumerator = Timers.GenericTimer(pck.beatsUntilHit * conductor.BPS, () =>
        {
            patternManager.ExecuteInput(InputType.Button1);
        });
        StartCoroutine(auxEnumerator);
        currentProcesess.Add(auxEnumerator);

    }
    public void StartDemo()
    {
        isDemoing = true;
    }
    public void StopDemo()
    {
        isDemoing = false;
        currentProcesess.ForEach(i => StopCoroutine(i));
        currentProcesess.Clear();
    }
}
