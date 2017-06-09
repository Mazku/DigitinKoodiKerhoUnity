using System;
using UnityEngine;
using System.Collections;

public class BaseBehaviour : MonoBehaviour
{
    public void DelayThenDo(float delay, Action func)
    {
        StartCoroutine(ExecuteAfterTime(delay, func));
    }

    IEnumerator ExecuteAfterTime(float time, Action func)
    {
        yield return new WaitForSeconds(time); 

        func();
    }
}

