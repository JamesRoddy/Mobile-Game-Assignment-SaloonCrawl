using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemypool : MonoBehaviour
{


    private int maxSpawnCount = 0;
    
    private int poolPointer = 0;
    private List<GameObject> pool = new List<GameObject>();

    public void setValues(List<GameObject> terrainEnemies, int maxEnemyNum, int minEnemyNum)
    {
        
        for(int i = 0; i < terrainEnemies.Count; i++)
        {
            for (int j = 0; j < maxEnemyNum; j++)
            {
/*
                Debug.Log("enemy added to pool at enemy list index " + i);*/
                GameObject enemyInstance = Instantiate(terrainEnemies[i], Vector3.zero, Quaternion.identity);
                enemyInstance.SetActive(false);
                pool.Add(enemyInstance);





            }
        } 

        maxSpawnCount = pool.Count;


    }  



    public GameObject requestAvaialbeObject()
    {


      
        return pool[poolPointer];


    }

    public void setEnemyValues()
    {
        foreach(GameObject enemyInstance in pool)
        {
            
         
            EnemyDescriptorInfo enemydesc  =  enemyInstance.GetComponent<EnemyDescriptorInfo>();
            Debug.Log("assigning spawn values enemy descriptor was null " + (enemyInstance.GetComponent<EnemyDescriptorInfo>() == null));
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
                Debug.Log("SPAWNING ENEMY pool pointer reached max " + poolPointer + " reseting... ");
            }

            Debug.Log(" SPAWNING ENEMY bool for object availablilty " + !pool[poolPointer].activeSelf);

            
        }
        return !pool[poolPointer].activeSelf;

    }











}