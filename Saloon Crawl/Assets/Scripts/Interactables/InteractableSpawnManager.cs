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
        Debug.Log("update spawns interactables");

        if ( !(player.CurrentTerrain.NextTerrainType.HasInteractables) &&  player.isCloseToEndOfCurrentterrain()  && !currentPool.HasDeffered  )
        {
            
            initialsSet = true;
            Debug.Log("condition to spawn interactables met terrain does not have interactables  " + !(player.CurrentTerrain.NextTerrainType.HasInteractables) + " player was close to end of current terrain is " + player.isCloseToEndOfCurrentterrain());
              currentTerrainType = player.CurrentTerrain.NextTerrainType;
            Debug.Log("current terrain type " + currentTerrainType.GetClassification);
            currentPoolType = player.CurrentTerrain.NextTerrainOn;
            Debug.Log("current pool type " + currentPoolType);
            currentPool = terrainManager.getInteractablePool(currentPoolType);
            currentPool.updateActiveObjects();
             
        }

        if(initialsSet && currentPool.hasAvailableObject( ))
        {
            Debug.Log("SPAWNING INTERACTABLES  spawning interactables for current pool " + currentPoolType + "has available object " + currentPool.hasAvailableObject() + "has deffered "+currentPool.HasDeffered);

            currentTerrainType.spawnInteractableObjects(currentPool);


        }

    }
}
