using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableSpawnManager : MonoBehaviour
{


    // main class for managing the current pool of inetractables needed by the terrain manager 
    TerrainManager terrainManager;
    playerController player;
    private TerrainType currentTerrainType;
    private TerrainClassifications currentPoolType;
    private InteractablePool currentPool;
    
  
    bool initialsSet = false;
    CamMovement playerCam;
    Camera Cam;

    public void  interactbleSpawnManagerStart()
    {
        Cam = FindFirstObjectByType<Camera>();
        playerCam =  Cam.GetComponent<CamMovement>();
        terrainManager = FindFirstObjectByType<TerrainManager>();
        player = FindFirstObjectByType<playerController>();
      
       
    }

   public void UpdateSpawns()
    {

        // if the player is getting close to the end of the current terrain and the next terrian has been activated and the current pool object is not waiting to spawn an object 
        if ( playerCam.playerCanSeeEnd() && player.CurrentTerrain.NextTerrainType != null && !(player.CurrentTerrain.NextTerrainType.HasInteractables)  && !currentPool.HasDeffered  )
        {
            
            initialsSet = true;
           currentTerrainType = player.CurrentTerrain.NextTerrainType;
            currentPoolType = player.CurrentTerrain.NextTerrainOn;
            currentPool.updateActiveObjects(); // go through the objects that are currenlty active and check if they need to be moved this only happens when we need to swicth terrain so we are not constantly looping when we need to spawn objects 
            currentPool = terrainManager.getInteractablePool(currentPoolType);
         
             
        }

        if(initialsSet && currentPool.hasAvailableObject(currentTerrainType)) // check if trhe current pool has available object 
        {

            currentTerrainType.spawnInteractableObjects(currentPool); // if so send it to the current terrain to be spawned 


        }

    } 


    public InteractablePool CurrentPool
    {
        set { currentPool = value; }


    }
}
