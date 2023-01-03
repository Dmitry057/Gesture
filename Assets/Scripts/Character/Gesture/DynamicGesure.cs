using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicGesure : MonoBehaviour
{
    
    public DynamicHand Left;
    public DynamicHand Right;

    [Range(0.001f, 2)]
    [SerializeField]
    private float _offsetPos;

    private bool _functionCompleted;

    [HideInInspector] 
    public GestureInputProcessor GestureProcessor;
    private void Update()
    {
        if (!_functionCompleted)
        {
            Left._id = CheckDistance(Left._points, Left._hand, Left._id);
            Right._id = CheckDistance(Right._points, Right._hand, Right._id);
        }
    }
    private int CheckDistance(Transform[] points, Transform hand, int id)
    {   
        
        if (points.Length > id)
        {
            if (Vector3.Distance(hand.position, points[id].position) < _offsetPos)
            {
                points[id].gameObject.SetActive(false);
                id++;

                if (hand == Left._hand)
                {
                    if (points.Length > id)
                    {
                        points[id].GetComponent<MeshRenderer>().material = Left.mainMaterial;
                    }
                    else
                    {
                        Left.isComplited = true;
                    }
                }
                else
                {
                    if (points.Length > id)
                    {
                        points[id].GetComponent<MeshRenderer>().material = Right.mainMaterial;
                    }
                    else
                    {
                        Right.isComplited = true;
                    }
                }
               
            }
        }
        else
        {
            if (Right.isComplited && Left.isComplited)
            {
                GestureFunction();
                Right.isComplited = false;
                Left.isComplited = false;
                _functionCompleted = true;
            }
        }
        return id;
    }
    private void GestureFunction() => GestureProcessor.FinishDynamicGesture();
     
}


[System.Serializable]
public class DynamicHand
{
    public Material mainMaterial;
    public Transform[] _points;
    [HideInInspector] public Transform _hand;
    [HideInInspector] public int _id;
    [HideInInspector] public bool isComplited;
}
