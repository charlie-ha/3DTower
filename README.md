3D VR Tower Array Project Documentation
GitHub project: https://github.com/charlie-ha/3DTowerArrayTour

Build a 3D VR virtual tour of the tower array
https://www.youtube.com/results?search_query=cincinati+terminal+tower
https://www.youtube.com/watch?v=0h6CyRuKCKQ
Format: VR headset Meta Quest Pro (self-contained) and a point click web based virtual tour (on iPad).
Why: Visitors can’t always go see the tower, needs staff or mobility limited.
What: If you were there in 1920, what would you see.
Platform: Ipad (IOS build) and PC touchscreen (Windows build)
Web? 

Unity version? 6000.0.10f1?

Blueprint with measurements of the tower array.
https://www.trains.com/ctr/photos-videos/photo-of-the-day/nerve-center-of-cincinnati-union-terminal/
https://www.trainaficionado.com/tower-a/


Sizing
1 unit in Unity = 1 meter

Scenes
Note for versioning: Different people working on different scenes to avoid conflicts when versioning. If you need to bring other people’s work into the main scene, just 1 person does that and then push to GitHub.

MainTowerScene
Description: This is the main scene used for VR

TowerScene_LevelDesign
Description: This is the scene used for level design. 

Codes
CanvasUIScript. 
Attached in MediaImage>RawImageMedia>CloseButton
Used to spawn images for player in VR project
Used in close button to close the image/video on the screen in Mobile Tour project.

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


PlayerRaycast
Attached to VirtualTourCamera
Used for player to move around the scene by clicking on hotspots

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


HotspotScript
Attached to Hotspots and MediaImage and MediaVideo
Used to control what images or videos to spawn in Mobile Tour project.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HotspotScript : MonoBehaviour
{
  


   public bool isSpawningPrefab =false;//spawn video/text/image
   public GameObject prefabToSpawn;
   
}



VirtualTourCameraController
Attached to VirtualTourCamera
Used to rotate the camera around by swiping on screen on using the mouse.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class VirtualTourCameraController : MonoBehaviour
{
   private float sensitivity = 1000f;//how fast camera rotate


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



LookAtMe
Attached to VirtualTourCamera> LookAtGameobject
Used to make all hotspots (objects with “hotspot” tag) look at camera in Mobile Tour project.

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



