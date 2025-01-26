using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // Start is called before the first frame update


    TerrainManager terrainManager;
    private GameObject currentEnemy;
    playerController player;
    private GameObject nextTileToSpawnEnemiesOn;
    private Enemypool currentObjectPool;
    private TerrainClassifications currentPool;
    private TerrainType currentTerrainType;
    private EnemyDescriptorInfo currentDescriptorInfo;
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
          Debug.Log(" SPAWNING ENEMY  conditions hit to generate new enemies terrain had no spawn positions was  " + !(player.CurrentTerrain.NextTerrainType.HasSpawnPositions) + "player was too close to terrain was " + player.isCloseToEndOfCurrentterrain());
          currentPool = player.CurrentTerrain.NextTerrainOn;
            Debug.Log(" SPAWNING ENEMY  current pool for spawining is " + player.CurrentTerrain.NextTerrainOn);
            nextTileToSpawnEnemiesOn = player.CurrentTerrain.NextAdjacentTerrainTile;
           Debug.Log(" SPAWNING ENEMY type of current terrain tile to spawn on " + player.CurrentTerrain.NextAdjacentTerrainTile.GetComponent<TerrainType>().GetClassification);
            currentTerrainType = player.CurrentTerrain.NextAdjacentTerrainTile.GetComponent<TerrainType>();
            Debug.Log(" SPAWNING ENEMY type of current terrain type " + player.CurrentTerrain.NextAdjacentTerrainTile.GetComponent<TerrainType>().GetClassification);
            currentTerrainType.setSpawnPositions();
            currentTerrainType.assignSpawnVlaue();
            currentEnemy = null;
            currentObjectPool = terrainManager.getEnemPool(currentPool);
            currentObjectPool.setEnemyValues();
           Debug.Log(" SPAWNING ENEMY  type of pool " + currentPool +" pool has available object "+currentObjectPool.hasAvailableObject());


       }


        if (currentObjectPool != null &&  currentObjectPool.hasAvailableObject()  && !currentTerrainType.HasMetEnemyRequirements)
        {
            Debug.Log(" SPAWNING ENEMY requesting object object pool has object: "+currentObjectPool.hasAvailableObject());
            if (!currentTerrainType.IsSpawningEnemy  )
            {
                currentEnemy = currentObjectPool.requestAvaialbeObject();
                Debug.Log("terrain was not spawning enemy " + currentTerrainType.GetClassification + "enemy descriptor was null "+ (currentEnemy.GetComponent<EnemyDescriptorInfo>() == null));
            }

            if(currentEnemy != null)
            {
                Debug.Log(" current enemy descriptor is null " + currentEnemy.GetComponent<EnemyDescriptorInfo>() == null);

                currentTerrainType.spawnEnemy(ref currentEnemy, currentEnemy.GetComponent<EnemyDescriptorInfo>());
            }
          




        }





    }


   



   




}
