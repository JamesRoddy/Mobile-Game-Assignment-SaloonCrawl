using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.Mathematics;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class SallonTerrainType : TerrainType
{

    private Vector3 previousTransFormPosition;
    private playerController player = null;
    private Vector3 enemySpawnLeft;
    private Vector3 enemySpawnRight;
    private float baseSaloonDist = 65.0f;
    private float enemySpawnDivider = 4.0f;
    private List<Vector3> spawnPositions =  new List<Vector3>(); 

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
        float previous = 0.0f;
        for(int i = 0; i < currentEnemiesCount; i++)
        {   
           
            float randomX = UnityEngine.Random.Range(enemySpawnLeft.x, enemySpawnRight.x + 1.0f);
            if ((previous < 0.0f && randomX < 0.0f) || (previous > 0.0f && randomX > 0.0f))
            {
                float side = previous < 0.0f ? enemySpawnLeft.x : enemySpawnRight.x;
                randomX =  UnityEngine.Random.Range(transform.position.x,side);

            }
            previous = randomX;
            spawnPositions.Add(new Vector3(randomX, transform.position.y, transform.position.z));
            

        } 

        hasSpawnPositions = true;
        Debug.Log(" SPAWNING ENEMY current terrain to spawn on " + classification + " has spawn positions " + HasSpawnPositions +" number " + spawnPositions.Count);

    }

    public override void spawnEnemy(ref GameObject enemy, EnemyDescriptorInfo currentDescriptor)
    { 

        if(currentSpawnCount != spawnPositions.Count)
        {
          


            Debug.Log(" SPAWNING ENEMY spawning enemy for " + classification + "current count " + currentSpawnCount + "current max " + currentEnemiesCount);
            Vector3 enemySpawnPos = spawnPositions[currentSpawnCount] + new Vector3(0.0f,enemy.transform.localScale.y,0.0f);
            Debug.Log(" SPAWNING ENEMY spawn position " + enemySpawnPos);
            currentSpawnCount++;

            activateObject(ref enemy,enemySpawnPos);



        }
         
    }

    public override void ResetTerrain()
    {
        hasSpawnPositions = false;
        currentSpawnCount = 0;
        spawnPositions.Clear();
        Debug.Log("reset terrain called for " + classification + " has spawn positions is now false " + hasSpawnPositions);

    }
    public override void TerrainEnable()
    {
        previousTransFormPosition = transform.position;
        enemySpawnRight = new Vector3(transform.position.x + transform.localScale.x / enemySpawnDivider, transform.position.y, transform.position.z);
        enemySpawnLeft = new Vector3(transform.position.x - transform.localScale.x / enemySpawnDivider, enemySpawnRight.y, enemySpawnRight.z);
        Debug.Log("enemy spawn right for saloon " + enemySpawnRight +"enemy spawn left for saloon " + enemySpawnLeft);
     
        Debug.Log(" terrain enable called for " + classification + " previousSpawnPos now  " + previousTransFormPosition);

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
