using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtMe : MonoBehaviour
{
    

    // Update is called once per frame
    void Update()
    {
        GameObject[] hotspots = GameObject.FindGameObjectsWithTag("hotspot");
        foreach (GameObject hs in hotspots)
        {
            hs.transform.LookAt(transform.position);
        }
    }
}
