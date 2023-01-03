using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StaticFunctions 
{
   public static RecognizePlayerFunctions playerFunctions = GameObject.Find("Player").GetComponent<RecognizePlayerFunctions>();

    public static IEnumerator Timer(float time, Action action)
    {
        yield return new WaitForSeconds(time);
        action.Invoke();
    }
}

