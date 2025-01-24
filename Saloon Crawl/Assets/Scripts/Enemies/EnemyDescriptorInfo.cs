using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyDescriptorInfo : MonoBehaviour
{

    [SerializeField] protected List<float> spawnIntervals ;
    playerController controller;
    private float spawnInterval = 0.0f;
    protected DeathChecker alive;
    public float SpawnInterval
    {
        get { return spawnInterval; }
        set { spawnInterval = value; }
    }

    private void Start()
    {
        controller =  FindFirstObjectByType<playerController>();
        Debug.Log("enemy start");
       
     
        EnemyStart(); 
       
    }
    public abstract void EnemyStart();
  
    private void Update()
    {

        if (!controller.IsViewingNextTerrain)
        {
            Debug.Log("enemy updating");
            EnemyUpdate();
        }

    }

    public abstract void EnemyUpdate();
    
    public void assignSpawnValues() 
    {
        alive = GetComponent<DeathChecker>();
        Debug.Log("alive is null " + (alive == null));
        alive.isAlive = true;
       
        if (spawnIntervals.Count > 0) {

            spawnInterval = spawnIntervals[Random.Range(0, spawnIntervals.Count)];
            /*Debug.Log("SPAWNING ENEMY assigning new spawn interval " + spawnInterval);*/
        }

    

        
    
    
    }

    public bool isAlive
    {
        get { return alive.isAlive; }
    }

}
