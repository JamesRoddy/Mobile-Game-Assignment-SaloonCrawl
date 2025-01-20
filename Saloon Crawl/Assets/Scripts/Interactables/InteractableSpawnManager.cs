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
    void Start()
    {
        terrainManager = GetComponent<TerrainManager>();
        player = FindObjectOfType<playerController>();
        currentPool = GetComponent<InteractablePool>();
        Debug.Log("current pool is null " + (currentPool == null) + "current pool has deffered " + currentPool.HasDeffered);
    }

   public void UpdateSpawns()
    {
     
        if ( !(player.CurrentTerrain.NextTerrainType.HasInteractables) &&  player.isCloseToEndOfCurrentterrain()  && !currentPool.HasDeffered  )
        {
            
            initialsSet = true;
            Debug.Log("condition to spawn interactables met terrain does not have interactables  " + !(player.CurrentTerrain.NextTerrainType.HasInteractables) + " player was close to end of current terrain is " + player.isCloseToEndOfCurrentterrain() +"pool has not deffered "+!currentPool.HasDeffered);
              currentTerrainType = player.CurrentTerrain.NextTerrainType;
            Debug.Log("current terrain type " + currentTerrainType.GetClassification);
            currentPoolType = player.CurrentTerrain.NextTerrainOn;
            Debug.Log("current pool type " + currentPoolType);
            currentPool.updateActiveObjects();
            currentPool = terrainManager.getInteractablePool(currentPoolType);
         
             
        }

        if(initialsSet && currentPool.hasAvailableObject(currentTerrainType))
        {
            Debug.Log("SPAWNING INTERACTABLES  spawning interactables for current pool " + currentPoolType + "has available object " + currentPool.hasAvailableObject(currentTerrainType) + "has deffered "+currentPool.HasDeffered);

            currentTerrainType.spawnInteractableObjects(currentPool);


        }

    }
}
