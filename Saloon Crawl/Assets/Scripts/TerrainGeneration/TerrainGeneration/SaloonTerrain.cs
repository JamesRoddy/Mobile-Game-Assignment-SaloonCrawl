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
            float randomX = UnityEngine.Random.Range(enemySpawnLeft.x, enemySpawnRight.x + 1);
            spawnPositions.Add(new Vector3(randomX, transform.position.y, transform.position.z));
            

        } 

    }

    public override void spawnEnemy(ref GameObject enemy)
    { 

        if(currentSpawnCount != spawnPositions.Count)
        {
            Vector3 spawnPos = spawnPositions[currentSpawnCount];
            currentSpawnCount++;  
            enemy.transform.position = new Vector3(spawnPos.x, spawnPos.y + enemy.transform.lossyScale.y, spawnPos.z);



        }
         
    }
    public override void TerrainStart()
    {
        previousTransFormPosition = transform.position;

        enemySpawnRight = new Vector3(transform.position.x + transform.localScale.x / 4,transform.position.y,transform.position.z);
        enemySpawnLeft = new Vector3(-enemySpawnRight.x, enemySpawnRight.y, enemySpawnRight.z);
        currentSpawnCount = 0;
        spawnPositions.Clear();
        if(player == null)
        { 

            player = FindObjectOfType<playerController>();
       
        }
    }


}
