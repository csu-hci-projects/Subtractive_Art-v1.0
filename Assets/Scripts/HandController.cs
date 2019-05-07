using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{
    public GameObject rightHand;
    public GameObject leftHand;
    

    // Update is called once per frame
    void Update()
    {
        rightHand.active = (OVRInput.Get(OVRInput.Axis1D.SecondaryHandTrigger) > 0.9 && OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger) < 0.1 && !OVRInput.Get(OVRInput.Touch.SecondaryIndexTrigger) && !leftHand.active); //right hand pointing
         
        leftHand.active = (OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger) > 0.9 && OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger) < 0.1 && !OVRInput.Get(OVRInput.Touch.PrimaryIndexTrigger) && !rightHand.active); //left hand pointing
         
    }
}
