using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesertTerrainType : TerrainType
{

    private Vector3 enemySpawnPosition;
    private float spawnInterval = 0.0f;
    private float maxSpawnInterval = 0.0f;
    private bool firstSpawn = true;
    private GameObject currentEnemy;


    public override void setSpawnPositions()
    {
        enemySpawnPosition = new Vector3(transform.position.x+transform.localScale.x/2.0f,transform.position.y,transform.position.z);    
        hasSpawnPositions = true;
    }



    public override void spawnEnemy(ref GameObject enemy, EnemyDescriptorInfo currentDescriptor)
    {

        Vector3 spawnPos =  new Vector3(enemySpawnPosition.x - enemy.transform.localScale.x/2.0f, enemySpawnPosition.y , enemySpawnPosition.z);
     
        if (firstSpawn)
        {

            activateObject(ref enemy,spawnPos);
            enemy.transform.position = new Vector3(enemy.transform.position.x, enemy.transform.position.y + enemy.GetComponent<Collider2D>().bounds.size.y, enemy.transform.position.z);
            maxSpawnInterval = currentDescriptor.SpawnInterval;
            firstSpawn = false;
            return;
        }
        isSpawningEnemy = true;
        if (maxSpawnInterval != currentDescriptor.SpawnInterval)
        {
            spawnInterval = 0.0f;
            maxSpawnInterval = currentDescriptor.SpawnInterval;
        }
        
        if(spawnInterval < maxSpawnInterval )
        {
           
            spawnInterval += Time.deltaTime;
            return;
        }

        isSpawningEnemy = false;
        
        activateObject(ref enemy,spawnPos);
        enemy.transform.position = new Vector3(enemy.transform.position.x, enemy.transform.position.y + enemy.GetComponent<Collider2D>().bounds.size.y, enemy.transform.position.z);

        currentDescriptor.resetDeath();
        currentDescriptor.EnemyEnable();
        spawnInterval = 0.0f;

    }
    public override void ResetTerrain()
    {
        hasSpawnPositions = false;
        currentEnemySpawnCount = 0;
        NextTerrainType = null;
        isSpawningEnemy = false;
    }
    public override void TerrainEnable()
    {
        assignSpawnVlaue();
    }
    public override bool Validate()
    {


        return true;
    }
    public override void TerrainStart()
    {

        firstSpawn = true;
        hasSpawnPositions = false;


    }

}
