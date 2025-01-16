using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemypool : MonoBehaviour
{


    private int maxSpawnCount = 0;
    private int minSpawnCount = 0;
    private int poolPointer = 0;
    private List<GameObject> pool = new List<GameObject>();

    public void setValues(List<GameObject> terrainEnemies, int maxEnemyNum, int minEnemyNum)
    {
        minSpawnCount = minEnemyNum;
        for(int i = 0; i < terrainEnemies.Count; i++)
        {
            for (int j = 0; j < maxEnemyNum; j++)
            {

                Debug.Log("enemy added to pool at enemy list index " + i);
                GameObject enemyInstance = Instantiate(terrainEnemies[i], Vector3.zero, Quaternion.identity);
                enemyInstance.SetActive(false);
                pool.Add(enemyInstance);





            }
        } 

        maxSpawnCount = pool.Count;


    }  



    public GameObject requestAvaialbeObject()
    {

        pool[poolPointer].SetActive(true);
        GameObject pointedTo = pool[poolPointer];
        poolPointer++;
        return pointedTo;


    }

    public bool hasAvailableObject()
    {
        if(poolPointer == maxSpawnCount)
        {
            Debug.Log("SPAWNING ENEMY pool pointer reached max " + poolPointer + " reseting... ");
            poolPointer = 0;
        } 

        return pool[poolPointer].activeSelf;

    }











}