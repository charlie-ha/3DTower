using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRayCast : MonoBehaviour
{
    Ray ray;
    RaycastHit hit;
    public static PlayerRayCast instance;

    private Camera mainCam;
    public Transform cameraTransform; // Drag your main camera here in Inspector
    private GameObject previousHotspot;
    private GameObject lastHoveredHotspot = null;

    public List<GameObject> allHotspots = new List<GameObject>();//put all hotspots in a list


    private Vector3 defaultScale = new Vector3(1f,1f,0.0001f);//default scale of the hotspot
    private Vector3 hoverScale = new Vector3(1.2f,1.2f,0.0001f);//hovered scale

    private Color defaultColor = Color.white;
    private Color hoverColor = new Color(203f/255f, 4f/255f, 4f/255f);

    public bool enabled = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Cache all hotspots to the list at the start
        GameObject[] hotspots = GameObject.FindGameObjectsWithTag("hotspot");
        foreach (GameObject hs in hotspots)
        {
            allHotspots.Add(hs);
             
        }
        mainCam = Camera.main;
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if(enabled)//make sure we dont click through UI
        {
            ray = mainCam.ScreenPointToRay(Input.mousePosition);//create ray from mouse to object
            if(Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("hotspot"))
                {
                    //hover on hotspot increase size and turn them red
                    GameObject hoveredHotspot = hit.collider.gameObject;

                    if (lastHoveredHotspot != null && lastHoveredHotspot != hoveredHotspot)
                    {
                        // Reset previous
                        lastHoveredHotspot.transform.localScale = defaultScale;
                        lastHoveredHotspot.GetComponent<Renderer>().material.color = defaultColor;
                    }

                    hoveredHotspot.transform.localScale = hoverScale;
                    Renderer hoveredRenderer = hoveredHotspot.GetComponent<Renderer>();
                    hoveredRenderer.material.color = hoverColor;

                    lastHoveredHotspot = hoveredHotspot;

                    if(Input.GetMouseButtonDown(0))//click on hotspot
                    {   
                        if(hoveredHotspot.GetComponent<HotspotScript>().isSpawningPrefab == false)
                        {
                            GameObject clickedHotspot = hit.collider.gameObject;

                            // Move the camera to the hotspot's x and y position
                            Vector3 newPosition = new Vector3(
                                clickedHotspot.transform.position.x,
                                clickedHotspot.transform.position.y,
                                cameraTransform.position.z
                            );

                            cameraTransform.position = newPosition;

                            // Reactivate all other hotspots
                            foreach (GameObject hs in allHotspots)
                            {
                                if (hs != clickedHotspot)
                                    hs.SetActive(true);
                            }

                            // Deactivate the one just clicked
                            clickedHotspot.SetActive(false);
                            
                        }
                        else if(hoveredHotspot.GetComponent<HotspotScript>().isSpawningPrefab == true)//spawn image/video/text
                        {
                            Instantiate(hoveredHotspot.GetComponent<HotspotScript>().prefabToSpawn);
                            enabled = false;//disable raycast so we dont click through UI
                        }

                        
                    }
                }
                else
                {
                    // foreach (GameObject hs in allHotspots)//make sure all hotpots are at default scale
                    // {
                    //     hs.transform.localScale = defaultScale;
                    //     Renderer defaultRenderer = hs.GetComponent<Renderer>();
                    //     defaultRenderer.material.color = defaultColor;
                    // }

                    // Reset last hovered if we're not on any hotspot
                    if (lastHoveredHotspot != null)
                    {
                        lastHoveredHotspot.transform.localScale = defaultScale;
                        lastHoveredHotspot.GetComponent<Renderer>().material.color = defaultColor;
                        lastHoveredHotspot = null;
                    }
                }
            }
        }
    }
}
