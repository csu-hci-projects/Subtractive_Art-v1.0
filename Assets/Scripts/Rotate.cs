using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    private Quaternion previousRotation = Quaternion.identity;
    private bool squeeze = false;
    void Update()
    {
        //subtraction --> q = OVRInput.GetLocalControllerRotation(OVRInput.Controller.LTouch) * Quaternion.Inverse(rotation)
        if((OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger) > 0.9 && OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger) > 0.9)) { //left hand squeezing
            if (!squeeze) previousRotation = OVRInput.GetLocalControllerRotation(OVRInput.Controller.LTouch);
            transform.rotation = transform.rotation * (Quaternion.Inverse(previousRotation) * OVRInput.GetLocalControllerRotation(OVRInput.Controller.LTouch));
            previousRotation = OVRInput.GetLocalControllerRotation(OVRInput.Controller.LTouch);
            squeeze = true;
        }
        else if((OVRInput.Get(OVRInput.Axis1D.SecondaryHandTrigger) > 0.9 && OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger) > 0.9)) { //left hand squeezing
            if (!squeeze) previousRotation = OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTouch);
            transform.rotation = transform.rotation * (Quaternion.Inverse(previousRotation) * OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTouch));
            previousRotation = OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTouch);
            squeeze = true;
        } else {
            squeeze = false;
        }
        
    }
}
