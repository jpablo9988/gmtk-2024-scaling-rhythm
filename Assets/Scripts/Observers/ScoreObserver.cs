using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreObserver : MonoBehaviour
{
    public delegate void EventWithScoreType(ScoreType type);
    public static event EventWithScoreType OnPlayerInput;
    public void InputScore(ScoreType type)
    {
        OnPlayerInput?.Invoke(type);
    }
}
