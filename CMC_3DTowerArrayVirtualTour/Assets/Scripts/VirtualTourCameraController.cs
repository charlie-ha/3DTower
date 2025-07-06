using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class VirtualTourCameraController : MonoBehaviour
{
    public float sensitivity = 0.1f; // lower this for touch-friendly control

    private float _yaw = 0f;//rotate y axis (vertical)
    private float _pitch = 0f;//rotate x axics (horizontal)
    [SerializeField] private float pitchClamp = 80f;//stop camera from flipping
    
    private Vector2 _inputDelta;

    private InputAction lookAction;

    void Awake()
    {
        // Create a new InputAction for look control (mouse delta or touch drag)
        lookAction = new InputAction(
            name: "Look",
            type: InputActionType.Value,
            binding: "<Pointer>/delta"
        );
        lookAction.Enable();
    }

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
        _inputDelta = lookAction.ReadValue<Vector2>();
        _yaw += _inputDelta.x * sensitivity * Time.deltaTime;
        _pitch -= _inputDelta.y * sensitivity * Time.deltaTime;
    }
    void RotateCamera(Quaternion rotation)
    {
        transform.rotation = rotation;
    }
    void OnDisable()
    {
        lookAction?.Disable();
    }

    void OnDestroy()
    {
        lookAction?.Dispose();
    }
}
