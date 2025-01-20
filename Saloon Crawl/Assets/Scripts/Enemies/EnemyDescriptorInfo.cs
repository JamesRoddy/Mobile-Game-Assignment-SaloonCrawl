using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyDescriptorInfo : MonoBehaviour
{

    [SerializeField] protected List<float> spawnIntervals ;
    private float spawnInterval = 0.0f;
    public float SpawnInterval
    {
        get { return spawnInterval; }
        set { spawnInterval = value; }
    }


    public void assignSpawnValues() 
    {

        if (spawnIntervals.Count > 0) {

            spawnInterval = spawnIntervals[Random.Range(0, spawnIntervals.Count)];
            Debug.Log("SPAWNING ENEMY assigning new spawn interval " + spawnInterval);
        }

    

        
    
    
    }



}
