using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WorldCursorHalo : MonoBehaviour
{
    public Camera mainCamera;
    public Transform halo;
    public LayerMask interactableLayers;

    private InputAction pointerPositionAction;

    void Awake()
    {
        // Binds to both mouse and touchscreen pointer position
        pointerPositionAction = new InputAction(
            name: "PointerPosition",
            type: InputActionType.Value,
            binding: "<Pointer>/position"
        );
        pointerPositionAction.Enable();
    }

    void Update()
    {
        Vector2 pointerPosition = pointerPositionAction.ReadValue<Vector2>();

        Ray ray = mainCamera.ScreenPointToRay(pointerPosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f, interactableLayers))
        {
            //halo.position = hit.point + Vector3.up * 0.01f; // slightly offset above surface
            //halo.LookAt(mainCamera.transform); // face camera

            // Set position slightly above the surface to avoid z-fighting
            halo.position = hit.point + hit.normal * 0.01f;
            // Align the halo's up direction with the surface normal
            halo.rotation = Quaternion.LookRotation(mainCamera.transform.forward, hit.normal);
        }
    }
    void OnDisable()
    {
        pointerPositionAction?.Disable();
    }

    void OnDestroy()
    {
        pointerPositionAction?.Dispose();
    }
}
