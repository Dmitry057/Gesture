using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class VRFootIK : MonoBehaviour
{
    private Animator anim;
    [Range(0, 1)]
    public float rightFootRotWeight = 1;
    [Range(0, 1)]
    public float leftFootRotWeight = 1;
    [Range(0, 1)]
    public float rightFootPosWeight = 1;
    [Range(0, 1)]
    public float leftFootPosWeight = 1;

    public Vector3 footOffset;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnAnimatorIK(int layerIndex)
    {
        Vector3 rightFootPos = anim.GetIKPosition(AvatarIKGoal.RightFoot);
        RaycastHit hit;

        bool hasHit = Physics.Raycast(rightFootPos + Vector3.up, Vector3.down, out hit);
        if(hasHit)
        {
            anim.SetIKPositionWeight(AvatarIKGoal.RightFoot, rightFootPosWeight);
            anim.SetIKPosition(AvatarIKGoal.RightFoot, hit.point + footOffset);

            Quaternion rightFootRotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(transform.forward, hit.normal), hit.normal);
            anim.SetIKRotationWeight(AvatarIKGoal.RightFoot, rightFootRotWeight);
            anim.SetIKRotation(AvatarIKGoal.RightFoot, rightFootRotation);
        }
        else
        {
            anim.SetIKPositionWeight(AvatarIKGoal.RightFoot, 0);
        }


        Vector3 leftFootPos = anim.GetIKPosition(AvatarIKGoal.LeftFoot);
        

         hasHit = Physics.Raycast(leftFootPos + Vector3.up, Vector3.down, out hit);
        if (hasHit)
        {
            anim.SetIKPositionWeight(AvatarIKGoal.LeftFoot, leftFootPosWeight);
            anim.SetIKPosition(AvatarIKGoal.LeftFoot, hit.point + footOffset);

            Quaternion leftFootRotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(transform.forward, hit.normal), hit.normal);
            anim.SetIKRotationWeight(AvatarIKGoal.LeftFoot, leftFootRotWeight);
            anim.SetIKRotation(AvatarIKGoal.LeftFoot, leftFootRotation);
        }
        else
        {
            anim.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 0); ;
        }
    }
}
