using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreObserver : IPausable
{
    public delegate void EventWithScoreType(ScoreType type);
    public static event EventWithScoreType OnGainScore;

    public void InputScore(ScoreType type)
    {
        if (!isGamePaused) OnGainScore?.Invoke(type);
    }
}
