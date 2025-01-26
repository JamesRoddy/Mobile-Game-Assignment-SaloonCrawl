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
    private float interactableSpawnRadiusOverlap;
    private List<Vector3> EnemySpawnPositions =  new List<Vector3>();
    private GameObject previousEnemy;
    public override bool Validate()
    {
    /*    Debug.Log("saloon trigger condition hit " + (Vector3.Distance(previousTransFormPosition, player.transform.position) > baseSaloonDist));*/
        return (Vector3.Distance(previousTransFormPosition, player.transform.position) > baseSaloonDist);

    }
   
    public override void setSpawnPositions()
    {
        float previous = transform.position.x;
        for(int i = 0; i < currentEnemiesCount; i++)
        {   
           
            float randomX = UnityEngine.Random.Range(enemySpawnLeft.x, enemySpawnRight.x + 1.0f);
            if ((previous < transform.position.x && randomX < transform.position.x) || (previous > transform.position.x && randomX > transform.position.x))
            {
                float side = previous < transform.position.x ? enemySpawnLeft.x : enemySpawnRight.x;
          
                randomX =  UnityEngine.Random.Range(transform.position.x,side);
               Debug.Log("SPAWNING ENEMIES SALOON  previous was " + previous + " transform position is " + transform.position.x + "side is " + side+" new random is "+randomX);

            }
            previous = randomX;
            EnemySpawnPositions.Add(new Vector3(randomX, transform.position.y, transform.position.z));
            

        } 

        hasSpawnPositions = true;
        Debug.Log(" SPAWNING ENEMY current terrain to spawn on " + classification + " has spawn positions " + HasSpawnPositions +" number " + EnemySpawnPositions.Count);

    }



 

    public override void spawnEnemy(ref GameObject enemy, EnemyDescriptorInfo currentDescriptor)
    {
        if(currentEnemySpawnCount != EnemySpawnPositions.Count)
        {

            Debug.Log("SPAWNING ENEMY   in saloon " + classification + " enemy decriptor info was " + (currentDescriptor == null));

/*            Debug.Log(" SPAWNING ENEMY spawning enemy for " + classification + "current count " + currentEnemySpawnCount + "current max " + currentEnemiesCount);*/
            Vector3 enemySpawnPos = EnemySpawnPositions[currentEnemySpawnCount] + new Vector3(0.0f,enemy.transform.localScale.y,0.0f); 
            
           /* Debug.Log(" SPAWNING ENEMY spawn position " + enemySpawnPos);*/
            
            currentEnemySpawnCount++;
            
            activateObject(ref enemy,enemySpawnPos);
          
            currentDescriptor.resetDeath();
            currentDescriptor.EnemyEnable();
            previousEnemy = enemy;
            return;


        } 

        hasMetEnemyRequirements = true;
        
         
    }

    public override void ResetTerrain()
    {
        hasSpawnPositions = false;
        currentEnemySpawnCount = 0;
        EnemySpawnPositions.Clear();
        NextTerrainType = null;
        hasMetEnemyRequirements = false;
        
                Debug.Log("reset terrain called for " + classification + " has spawn positions is now false " + hasSpawnPositions +" terrain type null"+(NextTerrainType == null));

    }
    public override void TerrainEnable()
    {
        currentEnemySpawnCount = 0;
        previousTransFormPosition = transform.position;
        enemySpawnRight = new Vector3(transform.position.x + transform.localScale.x / enemySpawnDivider, transform.position.y, transform.position.z);
        enemySpawnLeft = new Vector3(transform.position.x - transform.localScale.x / enemySpawnDivider, enemySpawnRight.y, enemySpawnRight.z);
        previousEnemy = null;
        hasMetEnemyRequirements = false;
        assignSpawnVlaue();
   


        /*   Debug.Log(" terrain enable called for " + classification + " previousSpawnPos now  " + previousTransFormPosition);*/

    }



    public override void TerrainStart()
    {
        previousTransFormPosition = transform.position;
        hasSpawnPositions = false;
        enemySpawnRight = new Vector3(transform.position.x + transform.localScale.x / enemySpawnDivider, transform.position.y,transform.position.z);
        enemySpawnLeft = new Vector3(transform.position.x - transform.localScale.x / enemySpawnDivider, enemySpawnRight.y, enemySpawnRight.z);
        currentEnemySpawnCount = 0;
        EnemySpawnPositions.Clear();

/*        Debug.Log(" SPAWNING ENEMY reseting terrain  " + classification + " spawn positions count " + EnemySpawnPositions.Count + "EnemyspawnPositions right " + enemySpawnRight + "spawn positions left" + enemySpawnLeft);
*/      if(player == null)
        { 

            player = FindObjectOfType<playerController>();
       
        }
    }


}
