using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Content.Interaction;

public class RailControllerHandleScript : MonoBehaviour
{
    public XRKnob trainControllerHandle;
    public Light trainTerminalLight;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(trainControllerHandle.value == 1)
        {
            trainTerminalLight.intensity = 100;//turn on the light when handle turned right
        }
        else if (trainControllerHandle.value == 0)
        {
            trainTerminalLight.intensity = 0;//turn off the light when handle turned left
        }
    }
}
