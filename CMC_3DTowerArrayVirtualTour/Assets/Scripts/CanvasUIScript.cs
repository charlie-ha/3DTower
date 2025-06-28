using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasUIScript : MonoBehaviour
{
    public GameObject prefab;
    public void DestroyGameObject(GameObject thisGameObject)
    {
        Destroy(thisGameObject);
    }
    public void EnableRaycast(bool boolean)
    {
        PlayerRayCast.instance.enabled = boolean;
    }

    public void SpawnPrefab()// spawn prefab/images/videos for VR project
    {
        Instantiate(prefab, new Vector3(8f,301.7f, -4f), Quaternion.identity);
    }
}
