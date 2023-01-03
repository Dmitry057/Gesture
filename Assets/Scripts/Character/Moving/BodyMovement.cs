using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyMovement : MonoBehaviour
{
    
    [Range(0, 5)]
    [SerializeField] private float _rotationSpeed;
    [Range(0, 10)]
    [SerializeField] private float _sizeSpeed = 3;
    [Range(0, 10)]
    [SerializeField] private float _yPosSpeed = 10;
    [Range(0, 5)]
    [SerializeField] private float _bodySpeed;
    [Range(0, 3)]
    [SerializeField] private float _maxHeight;
    [Range(0, 3)]
    [SerializeField] private float _minHeight;
    
    public Rigidbody rb;
    [SerializeField] private bool _onGround;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    public void SetStartPosition(Vector3 cameraPos)
    {
        transform.position = new Vector3(cameraPos.x, 0, cameraPos.z);
    }
    public void SetRotation(Quaternion other)
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, other, _rotationSpeed * Time.deltaTime); 
    }

    public void SetPosition(Vector3 Camera) 
    {
        transform.position = Vector3.Lerp(transform.position, new Vector3(Camera.x, transform.position.y, Camera.z), _bodySpeed * Time.deltaTime);
    }
    public void ChangeScale(float yOffset, Vector3 CameraPos)
    {
      
        if (yOffset < _maxHeight)
        {
            if(yOffset > _minHeight)
                changeYscale(yOffset);
        }
        else
        {
            changeYpos(CameraPos);
        }
    }

    private void changeYscale(float yOffset)
    {
        rb.useGravity = true;
        transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(transform.localScale.x, yOffset / 2, transform.localScale.z), _sizeSpeed * Time.deltaTime);
    }
    private void changeYpos(Vector3 cameraPos)
    {
        rb.useGravity = false;
        Vector3 pos = new Vector3(transform.position.x, cameraPos.y - _maxHeight, transform.position.z);
        transform.position = Vector3.Lerp(transform.position,pos, _yPosSpeed * Time.deltaTime);
    }

}
