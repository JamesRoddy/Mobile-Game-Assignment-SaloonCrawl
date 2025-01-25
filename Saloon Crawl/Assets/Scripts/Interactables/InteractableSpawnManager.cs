using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableSpawnManager : MonoBehaviour
{
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
      
        Debug.Log((Cam == null) + "cam was null  terrain manager null " + (terrainManager == null) + "player null " + player == null + " player cam " + (playerCam == null));
       
    }

   public void UpdateSpawns()
    {

        Debug.Log( "current pool " + (playerCam == null));
        if ( playerCam.playerCanSeeEnd() && player.CurrentTerrain.NextTerrainType != null && !(player.CurrentTerrain.NextTerrainType.HasInteractables)  && !currentPool.HasDeffered  )
        {
            
            initialsSet = true;
/*            Debug.Log("condition to spawn interactables met terrain does not have interactables  " + !(player.CurrentTerrain.NextTerrainType.HasInteractables) + " player was close to end of current terrain is " + playerCam.playerCanSeeEnd() +"pool has not deffered "+!currentPool.HasDeffered);
*/           currentTerrainType = player.CurrentTerrain.NextTerrainType;
           /* Debug.Log("current terrain type " + currentTerrainType.GetClassification);*/
            currentPoolType = player.CurrentTerrain.NextTerrainOn;
/*            Debug.Log("current pool type " + currentPoolType);*/
            currentPool.updateActiveObjects();
            currentPool = terrainManager.getInteractablePool(currentPoolType);
         
             
        }

        if(initialsSet && currentPool.hasAvailableObject(currentTerrainType))
        {
           /* Debug.Log("SPAWNING INTERACTABLES  spawning interactables for current pool " + currentPoolType + "has available object " + currentPool.hasAvailableObject(currentTerrainType) + "has deffered "+currentPool.HasDeffered);*/

            currentTerrainType.spawnInteractableObjects(currentPool);


        }

    } 


    public InteractablePool CurrentPool
    {
        set { currentPool = value; }


    }
}
