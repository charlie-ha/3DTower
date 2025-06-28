using UnityEngine;

public class CanvasUIScript : MonoBehaviour
{
    public void DestroyGameObject(GameObject thisGameObject)
    {
        Destroy(thisGameObject);
    }
    public void EnableRaycast(bool boolean)
    {
        PlayerRayCast.instance.enabled = boolean;
    }
}
