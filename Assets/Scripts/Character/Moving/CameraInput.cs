using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
public class CameraInput : MonoBehaviour
{
    [Header("Options")]
    [Range(0, 5)]
    [SerializeField] private float _minXZ_offset;
    [Range(0, 10)]
    [SerializeField] private float _globalSpeed;
    [SerializeField] private bool _PC_mode;
    [Range(0, 5)]
    [SerializeField] private float _maxXZvelocity;
    private float _horizontal, _frontal, _vertical;

    [Header("Links")]
    [SerializeField] private CameraMovement _camMovement;
    [SerializeField] private Transform _vrCamera;
    private Transform _body;
    private BodyMovement _bodyMovement;
    public void StartAction()
    {
        if (!transform.Find("Body(Clone)"))
        {
            Debug.Log("Can't find Body(Clone)");
            return;
        }

        _body = transform.Find("Body(Clone)");
        _bodyMovement = _body.gameObject.GetComponent<BodyMovement>();
        _bodyMovement.SetStartPosition(_vrCamera.position);
    }
    private float SetAxisXZ(float Axis)
    {
        if (Axis > 0)
        {
            Axis -= _minXZ_offset;
            if (Axis < 0) Axis = 0;

            if(Axis > _maxXZvelocity)
            {
                Axis = _maxXZvelocity;
            }
        }
        else
        {
            Axis += _minXZ_offset;
            if (Axis > 0) Axis = 0;

            if (Axis < -_maxXZvelocity)
            {
                Axis = -_maxXZvelocity;
            }
        }
        return Axis;
    }
   public void SetInput()
   {
        if (_PC_mode)
        {
            _horizontal = Input.GetAxis("Horizontal");
            _frontal = Input.GetAxis("Vertical");
            Debug.Log("SetInputWorking");
        }
        else
        {
            _horizontal = _vrCamera.position.x - _body.transform.position.x;
            _vertical = _vrCamera.position.y - _body.position.y;
            _frontal = _vrCamera.position.z - _body.transform.position.z;

            _horizontal = SetAxisXZ(_horizontal);
            _frontal = SetAxisXZ(_frontal);
            if (_horizontal == 0 && _frontal == 0)
            {
                _bodyMovement.SetPosition(_vrCamera.position);
            }
        }

        Vector3 physicVector = new Vector3(_horizontal, 0, _frontal);

        _camMovement.globalPosition(physicVector, _globalSpeed);

        _bodyMovement.ChangeScale(_vertical, _vrCamera.position);
        _bodyMovement.SetRotation(Quaternion.Euler(0, _vrCamera.eulerAngles.y, 0));
   }
}