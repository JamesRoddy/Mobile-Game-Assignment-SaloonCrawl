using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesertTerrainType : TerrainType
{

    private Vector3 enemySpawnPosition;
    private float spawnInterval = 0.0f;
    private float maxSpawnInterval = 0.0f;
    private bool firstSpawn = true;
    


    public override void setSpawnPositions()
    {
        enemySpawnPosition = new Vector3(transform.position.x+transform.localScale.x/2.0f,transform.position.y,transform.position.z);    
        hasSpawnPositions = true;
        Debug.Log(" SPAWNING ENEMY setting spawn position for  "+classification + " position "+ enemySpawnPosition);
    }



    public override void spawnEnemy(ref GameObject enemy, EnemyDescriptorInfo currentDescriptor)
    {
      
        Vector3 spawnPos =  new Vector3(enemySpawnPosition.x - enemy.transform.localScale.x/2.0f, enemySpawnPosition.y + enemy.transform.localScale.y, enemySpawnPosition.z);
     
        if (firstSpawn)
        {

           /* Debug.Log("SPAWNING enemy first spawn " + classification);*/
            activateObject(ref enemy,spawnPos);
            maxSpawnInterval = currentDescriptor.SpawnInterval;
            firstSpawn = false;
            return;
        }
        if(maxSpawnInterval != currentDescriptor.SpawnInterval)
        {
/*            Debug.Log("SPAWNING ENEMY " + classification + " new spawn interval "+currentDescriptor.SpawnInterval);*/
            spawnInterval = 0.0f;
            maxSpawnInterval = currentDescriptor.SpawnInterval;
        }
        
        if(spawnInterval < maxSpawnInterval )
        {
           
            spawnInterval += Time.deltaTime;
            return;
        }

       /* Debug.Log("time until next spawn reached " + classification + " spawn interval " + spawnInterval);*/
  
        activateObject(ref enemy,spawnPos);
        spawnInterval = 0.0f;

    }
    public override void ResetTerrain()
    {
        hasSpawnPositions = false;
        currentEnemySpawnCount = 0;
        NextTerrainType = null;
    /*    Debug.Log("reset terrain called for " + classification + " has spawn positions is now false " + hasSpawnPositions );*/
    }
    public override void TerrainEnable()
    {
   /*     Debug.Log("terrain enable called for " + classification );*/
        assignSpawnVlaue();
    }
    public override bool Validate()
    {
      /*  Debug.Log("desert trigger condition hit " + true);*/


        return true;
    }
    public override void TerrainStart()
    {

        firstSpawn = true;
        hasSpawnPositions = false;
       /* Debug.Log(" ENEMY SPAWN terrain start " + classification + "first spawn " + firstSpawn + " has spawn positions " + hasSpawnPositions);*/


    }

}
