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
    private List<Vector3> EnemySpawnPositions =  new List<Vector3>();
    public override bool Validate()
    {
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

            }
            previous = randomX;
            EnemySpawnPositions.Add(new Vector3(randomX, transform.position.y, transform.position.z));
            

        } 

        hasSpawnPositions = true;

    }



 

    public override void spawnEnemy(ref GameObject enemy, EnemyDescriptorInfo currentDescriptor)
    {
        if(currentEnemySpawnCount != EnemySpawnPositions.Count)
        {


            Vector3 enemySpawnPos = EnemySpawnPositions[currentEnemySpawnCount];
            
            
            currentEnemySpawnCount++;
            
            activateObject(ref enemy,enemySpawnPos);
            enemy.transform.position = new Vector3(enemy.transform.position.x, enemy.transform.position.y + enemy.GetComponent<Collider2D>().bounds.size.y, enemy.transform.position.z);

            currentDescriptor.resetDeath();
            StartCoroutine(currentDescriptor.EnemyEnableWait());
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
        

    }
    public override void TerrainEnable()
    {
        currentEnemySpawnCount = 0;
        previousTransFormPosition = transform.position;
        enemySpawnRight = new Vector3(transform.position.x + transform.localScale.x / enemySpawnDivider, transform.position.y, transform.position.z);
        enemySpawnLeft = new Vector3(transform.position.x - transform.localScale.x / enemySpawnDivider, enemySpawnRight.y, enemySpawnRight.z);
        hasMetEnemyRequirements = false;
        assignSpawnVlaue();
   



    }



    public override void TerrainStart()
    {
        previousTransFormPosition = transform.position;
        hasSpawnPositions = false;
        enemySpawnRight = new Vector3(transform.position.x + transform.localScale.x / enemySpawnDivider, transform.position.y,transform.position.z);
        enemySpawnLeft = new Vector3(transform.position.x - transform.localScale.x / enemySpawnDivider, enemySpawnRight.y, enemySpawnRight.z);
        currentEnemySpawnCount = 0;
        EnemySpawnPositions.Clear();

     if(player == null)
        { 

            player = FindObjectOfType<playerController>();
       
        }
    }


}
