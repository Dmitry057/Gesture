using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandObjects : MonoBehaviour
{
    //Pathes
    private string _indexPath = "Bones/Hand_WristRoot/Hand_Index1/Hand_Index2/Hand_Index3/Hand_IndexTip";
    private string _gestureCenterPath = "GestureParticleCenter";
    //Transforms
    [HideInInspector] public Transform indexTip;
    [HideInInspector] public Transform gestureCenter;
    private Transform handObjectTransform(string path) => transform.Find(path) ? transform.Find(path) : transform;
    
    public void Initialise()
    {
        indexTip = handObjectTransform(_indexPath);
        gestureCenter = handObjectTransform(_gestureCenterPath);
    }
    
}
