using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotspotScript : MonoBehaviour
{
    Ray ray;
    RaycastHit hit;

    public Transform cameraTransform; // Drag your main camera here in Inspector
    private GameObject previousHotspot;

    public List<GameObject> allHotspots = new List<GameObject>();//put all hotspots in a list

    public bool isSpawningPrefab =false;//spawn video/text/image
    public GameObject prefabToSpawn;
    
    private Vector3 defaultScale = new Vector3(1f,1f,0.0001f);//default scale of the hotspot
    private Vector3 hoverScale = new Vector3(1.2f,1.2f,0.0001f);//hovered scale

    private Color defaultColor = Color.white;
    private Color hoverColor = new Color(203f/255f, 4f/255f, 4f/255f);

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Cache all hotspots to the list at the start
        GameObject[] hotspots = GameObject.FindGameObjectsWithTag("hotspot");
        foreach (GameObject hs in hotspots)
        {
            allHotspots.Add(hs);
             
        }
    }

    // Update is called once per frame
    void Update()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);//create ray from mouse to object
        if(Physics.Raycast(ray, out hit))
        {
            if (hit.collider.CompareTag("hotspot"))
            {
                //hover on hotspot increase size and turn them red
                GameObject hoveredHotspot = hit.collider.gameObject;
                hoveredHotspot.transform.localScale = hoverScale;
                Renderer hoveredRenderer = hoveredHotspot.GetComponent<Renderer>();
                hoveredRenderer.material.color = hoverColor;

                if(Input.GetMouseButtonDown(0))//click on hotspot
                {   
                    if(isSpawningPrefab == false)
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
                    else if(isSpawningPrefab == true)//spawn image/video/text
                    {
                        Instantiate(prefabToSpawn);
                    }

                     
                }
            }
            else
            {
                foreach (GameObject hs in allHotspots)//make sure all hotpots are at default scale
                {
                    hs.transform.localScale = defaultScale;
                    Renderer defaultRenderer = hs.GetComponent<Renderer>();
                    defaultRenderer.material.color = defaultColor;
                }
            }
        }
        
    }
}
