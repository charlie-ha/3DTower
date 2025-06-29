using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldCursorHalo : MonoBehaviour
{
    public Camera mainCamera;
    public Transform halo;
    public LayerMask interactableLayers;

    void Update()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
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
}
