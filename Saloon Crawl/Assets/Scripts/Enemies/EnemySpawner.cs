using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // Start is called before the first frame update


    TerrainManager terrainManager;
    playerController player;
    private GameObject nextTileToSpawnEnemiesOn;
    private Enemypool currentObjectPool;
    private TerrainClassifications currentPool;
    private TerrainType currentTerrainType;
    private EnemyDescriptorInfo currentDescriptorInfo;
    void Start()
    {
        terrainManager = GetComponent<TerrainManager>();
        player = FindObjectOfType<playerController>();

        Debug.Log("player is null for enemy spawner " + player == null);
        


    }

    // Update is called once per frame
   public void UpdateSpawns()
    {
      
        if ( !(player.CurrentTerrain.NextTerrainType.HasSpawnPositions) && player.isCloseToEndOfCurrentterrain())
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
            currentObjectPool = terrainManager.getEnemPool(currentPool);
            Debug.Log(" SPAWNING ENEMY  type of pool " + currentPool +" pool has available object "+currentObjectPool.hasAvailableObject());


       }


        if (currentObjectPool != null &&  currentObjectPool.hasAvailableObject())
        {
            Debug.Log(" SPAWNING ENEMY requesting object object pool has object: "+currentObjectPool.hasAvailableObject());
            GameObject enemy = currentObjectPool.requestAvaialbeObject();
            if(currentDescriptorInfo != enemy.GetComponent<EnemyDescriptorInfo>())
            {
                Debug.Log(" SPAWNING ENEMY new enemy descriptor");
                currentDescriptorInfo = enemy.GetComponent<EnemyDescriptorInfo>();
                currentDescriptorInfo.assignSpawnValues();
                Debug.Log(" SPAWNING ENEMY assigning values new spawn interval " + currentDescriptorInfo.SpawnInterval);
                
            }
            currentTerrainType.spawnEnemy(ref enemy,currentDescriptorInfo); 




        }





    }


   



   




}
