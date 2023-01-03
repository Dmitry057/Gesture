using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OculusSampleFramework;
public class GrabbleObjects : OVRGrabber
{
    public void GrabbleObj()
    {
        GrabBegin();
    }
    public void NotGrabble()
    {
        GrabEnd();
    }

}
