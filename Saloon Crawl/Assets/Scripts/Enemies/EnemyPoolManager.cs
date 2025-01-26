using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemypool : MonoBehaviour
{

    // main object responsible for storing a pool of enemies and 
    private int maxSpawnCount = 0;
    
    private int poolPointer = 0;
    private List<GameObject> pool = new List<GameObject>();

    public void setValues(List<GameObject> terrainEnemies, int maxEnemyNum, int minEnemyNum)
    {
        // insantiate all enemies needed in the pool based on their max spawn count 
        for(int i = 0; i < terrainEnemies.Count; i++)
        {
            for (int j = 0; j < maxEnemyNum; j++)
            {

                GameObject enemyInstance = Instantiate(terrainEnemies[i], Vector3.zero, Quaternion.identity);
                enemyInstance.SetActive(false);
                pool.Add(enemyInstance);





            }
        } 

        maxSpawnCount = pool.Count;


    }  



    public GameObject requestAvaialbeObject()
    {

        // request the object accessed by the current pool pointer 
      
        return pool[poolPointer];


    }

    public void setEnemyValues()
    {
        foreach(GameObject enemyInstance in pool)
        {
            
         
            EnemyDescriptorInfo enemydesc  =  enemyInstance.GetComponent<EnemyDescriptorInfo>();
            // assigning new spawn values to each enemy in the pool this is used when the next terrain needs to be generated with the pools enemy type 
            enemydesc.assignSpawnValues();

          

        }



    }
    public bool hasAvailableObject()
    {
        if( pool[poolPointer].activeSelf)
        {
            poolPointer++; 
            if(poolPointer == maxSpawnCount)
            {
                poolPointer = 0;
                // reset the current pool pointer when it reaches the maximum amount 
            }


            
        }
        return !pool[poolPointer].activeSelf; // check if the pool has availabe objects(the current pool  pointer is not active) this is done to avoid looping through the objects each time one is needed which could result in looping through all objects to activate one 

    }











}