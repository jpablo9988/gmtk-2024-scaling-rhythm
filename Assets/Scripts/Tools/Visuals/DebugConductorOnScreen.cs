using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DebugConductorOnScreen : MonoBehaviour
{
    [SerializeField]
    private Conductor conductor;
    [SerializeField]
    private TextMeshProUGUI currentBeatText;
    [SerializeField]
    private TextMeshProUGUI positionInAnalogText;
    [SerializeField]
    private TextMeshProUGUI positionInSeconds;
    [SerializeField]
    private TextMeshProUGUI positionInSecondsSample;
    [SerializeField]
    private TextMeshProUGUI positionInSecondsSampleDst;
    [SerializeField]
    private TextMeshProUGUI nextInputBeat;
    [SerializeField]
    private TextMeshProUGUI completedLoopsText;
    [SerializeField]
    private PatternManager patternManager;


    void Update()
    {
        currentBeatText.text = "CurrentBeat: " + conductor.CurrentBeat.ToString();
        positionInAnalogText.text = "Position in Analog: " + conductor.PositionInAnalog.ToString();
        positionInSeconds.text = "Position in Seconds: " + conductor.PositionInSeconds.ToString();
        if (patternManager.NextInput.HasValue)
        {
            nextInputBeat.text = "Next beat is in: " + patternManager.NextInput.Value.actionBeat;
        }
        positionInSecondsSample.text = "Position in sample: " + conductor.PositionInSample.ToString();
        positionInSecondsSampleDst.text = "Position in sample dst: " + conductor.PositionInSampleDst.ToString();
        completedLoopsText.text = "Completed Loops: " + conductor.CompletedLoops;
    }


}
