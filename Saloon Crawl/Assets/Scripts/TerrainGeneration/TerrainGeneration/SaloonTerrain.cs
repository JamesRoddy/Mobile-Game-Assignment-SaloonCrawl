using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.Mathematics;
using UnityEngine;

public class SallonTerrainType : TerrainType
{

    Vector3 previousTransFormPosition;
    playerController player = null;
    Vector3 enemySpawnLeft;
    Vector3 enemySpawnRight;
    float baseSaloonDist = 65.0f;
    float enemySpawnDivider = 4.0f;
    List<Vector3> spawnPositions =  new List<Vector3>(); 

    public override bool Validate()
    {
        Debug.Log("saloon trigger condition hit " + (Vector3.Distance(previousTransFormPosition, player.transform.position) > baseSaloonDist));
        return (Vector3.Distance(previousTransFormPosition, player.transform.position) > baseSaloonDist);

    }
    public override void spawnInteractables(List<GameObject> interactables)
    {

    }
    public override void setSpawnPositions()
    {

        for(int i = 0; i < currentEnemiesCount; i++)
        {
            float randomX = UnityEngine.Random.Range(enemySpawnLeft.x, enemySpawnRight.x + 1.0f);
            spawnPositions.Add(new Vector3(randomX, transform.position.y, transform.position.z));
            

        } 

        hasSpawnPositions = true;
        Debug.Log(" SPAWNING ENEMY current terrain to spawn on " + classification + " has spawn positions " + HasSpawnPositions +" number " + spawnPositions.Count);

    }

    public override void spawnEnemy(ref GameObject enemy)
    { 

        if(currentSpawnCount != spawnPositions.Count)
        {
            Debug.Log(" SPAWNING ENEMY spawning enemy for " + classification + "current count " + currentSpawnCount + "current max " + currentEnemiesCount);
            Vector3 spawnPos = spawnPositions[currentSpawnCount];
            Debug.Log(" SPAWNING ENEMY spawn position " + spawnPos);
            currentSpawnCount++;  
            enemy.transform.position = new Vector3(spawnPos.x, spawnPos.y + enemy.transform.lossyScale.y, spawnPos.z);



        }
         
    }
    public override void TerrainStart()
    {
        previousTransFormPosition = transform.position;
        hasSpawnPositions = false;
        enemySpawnRight = new Vector3(transform.position.x + transform.localScale.x / enemySpawnDivider, transform.position.y,transform.position.z);
        enemySpawnLeft = new Vector3(-enemySpawnRight.x, enemySpawnRight.y, enemySpawnRight.z);
        currentSpawnCount = 0;
        spawnPositions.Clear();

        Debug.Log(" SPAWNING ENEMY reseting terrain  " + classification + " spawn positions count " + spawnPositions.Count + "spawnPositions right " + enemySpawnRight + "spawn positions left" + enemySpawnLeft);
        if(player == null)
        { 

            player = FindObjectOfType<playerController>();
       
        }
    }


}
