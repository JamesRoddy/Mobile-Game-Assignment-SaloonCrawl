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
    private List<Vector3> spawnPositions = new List<Vector3>();
   
    void Start()
    {
        terrainManager = GetComponent<TerrainManager>();
        player = GetComponent<playerController>();
        
      
        


    }

    // Update is called once per frame
    void Update()
    { 
        
       if(player.isCloseToEndOfCurrentterrain())
       {
            currentPool = player.CurrentTerrain.NextTerrainOn;
            nextTileToSpawnEnemiesOn = player.CurrentTerrain.NextAdjacentTerrainTile;
            currentTerrainType = player.CurrentTerrain.NextAdjacentTerrainTile.GetComponent<TerrainType>();
            currentTerrainType.setSpawnPositions();
            currentObjectPool = terrainManager.getEnemPool(currentPool);


       }


        if (currentObjectPool.hasAvailableObject())
        {
            GameObject enemy = currentObjectPool.requestAvaialbeObject();
            Debug.Log("requesting object");
            currentTerrainType.spawnEnemy(ref enemy); 




        }





    }


   



   




}
