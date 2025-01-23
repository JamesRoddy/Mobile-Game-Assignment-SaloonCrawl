using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class CamMovement : MonoBehaviour
{

    private float offsetX = 5.0f;
    private Camera cam;
    private float offsetSmooth = 1.5f;
   
    private playerController player;
 
    private TerrainManager terrainManager;

    private float targetAspect = 16.0f/9.0f;
    private float offsetScalar = 1.0f;
    float defaultZoom = 3.0f;
    
    


    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        
        terrainManager = FindFirstObjectByType<TerrainManager>();
        cam.orthographicSize = 3.0f; 

        player = FindObjectOfType<playerController>();
        adjustCamOnStart();
    }

    // Update is called once per frame


    public bool canSeePosition(Vector3 position)
    {
        float x = cam.WorldToViewportPoint(position).x;

        return x >= 0.0f && x <= 1.0f;



    }

    void Update()
    {
        updateCamPosition();
    }


    private void adjustCamOnStart()
    {

         





    }

    public bool playerCanSeeEndWithoutNextTerrain()
    {

        float end = cam.WorldToViewportPoint(player.CurrentTerrain.SpawnRight).x;

        return player.CurrentTerrain.NextTerrainType == null && end > 0.0f && end <= 1.0f;
        

    }

    public bool playerCanSeeEnd()
    {

        float end = cam.WorldToViewportPoint(player.CurrentTerrain.SpawnRight).x;

        return  end > 0.0f && end < 1.0f;


    }
    void updateCamPosition()
    {

        Vector3 finalCamPos = Vector3.Lerp(cam.transform.position, new Vector3(player.transform.position.x + (offsetX*offsetScalar), cam.transform.position.y, cam.transform.position.z), offsetSmooth * Time.deltaTime);

 
        cam.transform.position = finalCamPos;




    }






}
