using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // Start is called before the first frame update


    TerrainManager terrainManager;
    float spawnInterval;
    playerController player;
    private List<GameObject> currentTerrainToSpawnOn = new List<GameObject>();
    private GameObject nextTileToSpawnEnemiesOn;
    private Enemypool currentObjectPool;
    private TerrainClassifications currentPool;
    private TerrainType currentTerrainType;
   
    void Start()
    {
        terrainManager = GetComponent<TerrainManager>();
        player = GetComponent<playerController>();
        
      
        


    }

    // Update is called once per frame
   public void UpdateSpawns()
    { 
        
       if( !(player.CurrentTerrain.NextTerrainType.HasSpawnPositions) && player.isCloseToEndOfCurrentterrain())
       {
            Debug.LogWarning(" SPAWNING ENEMY  conditions hit to generate new enemies terrain had no spawn positions was  " + !(player.CurrentTerrain.NextTerrainType.HasSpawnPositions) + "player was too close to terrain was " + player.isCloseToEndOfCurrentterrain());
            currentPool = player.CurrentTerrain.NextTerrainOn;
            Debug.LogWarning(" SPAWNING ENEMY  current pool for spawining is " + player.CurrentTerrain.NextTerrainOn);
            nextTileToSpawnEnemiesOn = player.CurrentTerrain.NextAdjacentTerrainTile;
            Debug.LogWarning(" SPAWNING ENEMY type of current terrain tile to spawn on " + player.CurrentTerrain.NextAdjacentTerrainTile.GetComponent<TerrainType>().GetClassification);
            currentTerrainType = player.CurrentTerrain.NextAdjacentTerrainTile.GetComponent<TerrainType>();
            Debug.Log(" SPAWNING ENEMY type of current terrain type " + player.CurrentTerrain.NextAdjacentTerrainTile.GetComponent<TerrainType>().GetClassification);
            currentTerrainType.setSpawnPositions(); 
            
            currentObjectPool = terrainManager.getEnemPool(currentPool);
            Debug.Log(" SPAWNING ENEMY  type of pool " + currentPool);


       }


        if (currentObjectPool.hasAvailableObject())
        {
            Debug.Log(" SPAWNING ENEMY requesting object object pool has object: "+currentObjectPool.hasAvailableObject());
            GameObject enemy = currentObjectPool.requestAvaialbeObject();

            currentTerrainType.spawnEnemy(ref enemy); 




        }





    }


   



   




}
