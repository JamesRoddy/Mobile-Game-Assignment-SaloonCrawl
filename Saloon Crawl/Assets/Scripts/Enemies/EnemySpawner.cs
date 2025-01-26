using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // Start is called before the first frame update

    // main class responsible for spawning enemies of the current pool of the current terrain type this allows there to be one central class that spawns enemies from each enemy pool object that stores particualr enemies 
    TerrainManager terrainManager;
    private GameObject currentEnemy;
    playerController player;
    private GameObject nextTileToSpawnEnemiesOn;
    private Enemypool currentObjectPool;
    private TerrainClassifications currentPool; /// used to access and dynamcially adjust the current pool of enemies being used based on player current terrain and next terrain 
    private TerrainType currentTerrainType;
    private EnemyDescriptorInfo currentDescriptorInfo; //info about current enemy being spawned 
    public void EnemySpanwnManagerStart()
    {
        terrainManager = GetComponent<TerrainManager>();
        player = FindObjectOfType<playerController>();

        


    }

    // Update is called once per frame
   public void UpdateSpawns()
    {
        if ( player.CurrentTerrain.NextTerrainType != null && !(player.CurrentTerrain.NextTerrainType.HasSpawnPositions) && player.isCloseToEndOfCurrentterrain())
       {
            currentPool = player.CurrentTerrain.NextTerrainOn; // set the current terrain classifctaion for the current enemy pool to be used by the enemy spawner object
            nextTileToSpawnEnemiesOn = player.CurrentTerrain.NextAdjacentTerrainTile; // set the next terrain object 
            currentTerrainType = player.CurrentTerrain.NextAdjacentTerrainTile.GetComponent<TerrainType>();

            // set up the next terrain to be spawned on 
            currentTerrainType.setSpawnPositions();
            currentTerrainType.assignSpawnVlaue();
            currentEnemy = null;
            currentObjectPool = terrainManager.getEnemPool(currentPool);
            currentObjectPool.setEnemyValues();


       }

        // request an object while the current pool has an availabe inactive obejct and if the current terrian type has not met its current max spawn values 
        if (currentObjectPool != null &&  currentObjectPool.hasAvailableObject()  && !currentTerrainType.HasMetEnemyRequirements) 
        {
            if (!currentTerrainType.IsSpawningEnemy  ) 
            {
                currentEnemy = currentObjectPool.requestAvaialbeObject();
            }

            if(currentEnemy != null)
            {

                currentTerrainType.spawnEnemy(ref currentEnemy, currentEnemy.GetComponent<EnemyDescriptorInfo>());
            }
          




        }





    }


   



   




}
