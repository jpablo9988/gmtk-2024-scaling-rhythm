using System;
using System.Collections;
using UnityEngine;


public static class Timers
{
    public static bool PauseTimers = false;
    public static IEnumerator GenericTimer(float duration, Action parameterAction = null)
    {
        float timeLeft = duration;
        while (timeLeft > 0f)
        {
            if (!PauseTimers) timeLeft -= Time.deltaTime;
            yield return null;
        }
        parameterAction?.Invoke();
    }
    public static IEnumerator GenericTimer<T>(float duration, Action<T> parameterAction, T parameter)
    {
        float timeLeft = duration;
        while (timeLeft > 0f)
        {
            if (!PauseTimers) timeLeft -= Time.deltaTime;
            yield return null;
        }
        parameterAction?.Invoke(parameter);
    }
}

