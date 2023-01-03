using System.Collections;
using UnityEngine;

public class GestureGrab : OVRGrabber
{
    // Boolean used to check if is Grabbing or not.
    [SerializeField]
    bool isGrabbing = false;

    protected override void Start()
    {
        // We begin by initialize the base.Start function where are set few variable from OVRGrabber like:
        // m_lastPos, m_last_Rot and the m_parentTransform.
        base.Start();
    }

    // Function used as a switch to determinate if we are grabbing or not by passing as argument
    // the string "true" in the gesture when detected and "false" when is not recognize
    public void DetectGrabbing(string _isGrabbing)
    {
        // if "_isGrabbing" is true, we set isGrabbing to true
        if (_isGrabbing.Equals("true"))
        {
            isGrabbing = true;
        }
        // else if "_isGrabbing" is false, we set isGrabbing to false
        else if (_isGrabbing.Equals("false"))
        {
            isGrabbing = false;
        }
    }

    public override void Update()
    {
        // we call the base.Update to make sure that OVRGrabber update some values
        base.Update();

        // if we are not grabbin anything and we have a candidate able to be grabbed
        // and isGrabbing (found by the gesture detector on this case) is true
        if (!m_grabbedObj && m_grabCandidates.Count > 0 && isGrabbing)
        {
            // we call the GrabBegin the object
            GrabBegin();
        }
        // else if there is an object that we are grabbing and the isGrabbing is false
        else if (m_grabbedObj != null && !isGrabbing)
        {
            // we call the override GrabEnd
            GrabEnd();
        }
    }

    // To call in the gestures for refrech the position and rotation when releasing
    public void isReleasing()
    {
        m_lastPos = transform.position;
        m_lastRot = transform.rotation;
    }
   
    protected void GrabEnd()
    {
        // if there is an object we are grabbing
        if (m_grabbedObj != null)
        {
            // we calculate the linearVelocity calculate by:
            // transform.parent.position (position of --HandAnchor) minus the last position recorded
            // everything divided by time.fixedDeltaTime
            Vector3 linearVelocity = (transform.parent.position - m_lastPos) / Time.fixedDeltaTime;
            // the same operation is calculated but in this case is calculated on the EulerAngles
            Vector3 angularVelocity = (transform.parent.eulerAngles - m_lastRot.eulerAngles) / Time.fixedDeltaTime;

            // And we call the function that make us able to release the grab with the velocities we calculated          
            GrabbableRelease(linearVelocity, angularVelocity);
        }

        // And the we restore de collider used for the grabbing
        GrabVolumeEnable(true);
    }
}