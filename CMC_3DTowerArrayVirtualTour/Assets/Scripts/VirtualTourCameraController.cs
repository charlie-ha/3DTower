using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualTourCameraController : MonoBehaviour
{
    public float sensitivity = 500f;//how fast camera rotate

    private float _yaw = 0f;//rotate y axis (vertical)
    private float _pitch = 0f;//rotate x axics (horizontal)
    [SerializeField] private float pitchClamp = 80f;//stop camera from flipping

    // Update is called once per frame
    void Update()
    {
        HandleInput();//handle input from player

        // Clamp pitch so the camera doesn't flip upside down
        _pitch = Mathf.Clamp(_pitch, -pitchClamp, pitchClamp);

        Quaternion yawRotation = Quaternion.Euler(_pitch, _yaw, 0f);
        //create Euler rotation based on user input; Quaternion represent rotation in 3D space 

        RotateCamera(yawRotation);//do the rotation
    }
    public void HandleInput()
    {
        Vector2 inputDelta = Vector2.zero;//track change in position of mouse or finger on touchscreen
        //Vector2.zero is (0,0,0)

        if(Input.touchCount > 0)//there are at least 1 touch on screen
        {
            Touch touch = Input.GetTouch(0);//get first touch being detected
            inputDelta = touch.deltaPosition;//change in position of touch point
        }
        else if (Input.GetMouseButton(0))
        {
            inputDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
            //change in position of mouse positions and store to inputDelta
        }
        _yaw += inputDelta.x * sensitivity * Time.deltaTime;
        _pitch -= inputDelta.y * sensitivity * Time.deltaTime;
    }
    void RotateCamera(Quaternion rotation)
    {
        transform.rotation = rotation;
    }
}
