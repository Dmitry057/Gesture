using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRAnimatorController : MonoBehaviour
{
    private Animator animator;
    private Vector3 previosPos;
    private VRRig vrRig;
    public float speedTrechhold;
    [Range(0, 1)]
    public float smoothing = 1;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        vrRig = GetComponent<VRRig>();
        previosPos = vrRig.head.vrTarget.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 headsetSpeed = (vrRig.head.vrTarget.position - previosPos) / Time.deltaTime;
        headsetSpeed.y = 0;

        Vector3 headsetLocalSpeed = transform.InverseTransformDirection(headsetSpeed);
        previosPos = vrRig.head.vrTarget.position;

        float previousDirectionX = animator.GetFloat("DirectionX");
        float previousDirectionY = animator.GetFloat("DirectionY");

        animator.SetBool("isMoving", headsetLocalSpeed.magnitude > speedTrechhold);
        animator.SetFloat("DirectionX",Mathf.Lerp(previousDirectionX, Mathf.Clamp(headsetLocalSpeed.x,-1,1),smoothing));
        animator.SetFloat("DirectionY", Mathf.Lerp(previousDirectionY, Mathf.Clamp(headsetLocalSpeed.z, -1, 1), smoothing));

    }
}
