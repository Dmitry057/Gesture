using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
   
    public void globalPosition(Vector3 direction, float speed)
    {
        rb?.MovePosition(transform.position + direction * speed * Time.deltaTime);
    }
}
