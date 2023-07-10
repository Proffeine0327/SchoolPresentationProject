using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InvokeUtility
{
    public static void Invoke(this MonoBehaviour m, System.Action action, float t)
    {
        m.StartCoroutine(WaitInvoke(action, t));
    }

    private static IEnumerator WaitInvoke(System.Action action, float t)
    {
        yield return new WaitForSeconds(t);
        action.Invoke();
    }
}
