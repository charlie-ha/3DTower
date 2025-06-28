using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtMe : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject[] hotspots = GameObject.FindGameObjectsWithTag("hotspot");
        foreach (GameObject hs in hotspots)
        {
            hs.transform.LookAt(transform.position);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
