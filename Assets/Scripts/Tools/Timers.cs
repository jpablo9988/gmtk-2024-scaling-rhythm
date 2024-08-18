using System;
using System.Collections;
using UnityEngine;


public static class Timers 
{
    public static IEnumerator GenericTimer(float duration, Action parameterAction = null)
    {
        yield return new WaitForSeconds(duration);
        parameterAction?.Invoke();
    }
    public static IEnumerator GenericTimer<T>(float duration, Action<T> parameterAction, T parameter)
    {
        yield return new WaitForSeconds(duration);
        parameterAction?.Invoke(parameter);
    }
}

