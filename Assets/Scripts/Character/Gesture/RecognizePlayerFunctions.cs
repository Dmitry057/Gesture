using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using Photon.Pun;
using System;

public delegate void MyViewAction();
public class RecognizePlayerFunctions: MonoBehaviour
{
    public PhotonView view;
    public CameraInput cameraInput;
    public Gesture.HandJests handJests;

    private void Start()
    {
        cameraInput = GetComponentInChildren<CameraInput>();
        handJests = GetComponentInChildren<Gesture.HandJests>();

        if (!view.IsMine) 
            return; 

        cameraInput.StartAction();
        handJests.StartAction();
    }
    private void Update()
    {
        if (!view.IsMine)
            return;

        handJests.UpdateAction();
    }
    private void FixedUpdate()
    {
        if (!view.IsMine)
            return;
        cameraInput.SetInput();
    }
}
