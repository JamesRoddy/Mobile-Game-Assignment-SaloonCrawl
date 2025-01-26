using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyDescriptorInfo : MonoBehaviour
{

    [SerializeField] protected List<float> spawnIntervals ;
    protected playerController controller;
    [SerializeField] protected int scoreIncrement;
    protected Transform scoreIncrementPopUp;
    private float spawnInterval = 0.0f;
    protected DeathChecker alive;
    private InstaniateScorePopUp scorePopUp;
    public float SpawnInterval
    {
        get { return spawnInterval; }
        set { spawnInterval = value; }
    }

    private void Start()
    {
        controller =  FindFirstObjectByType<playerController>();
        scorePopUp = GetComponent<InstaniateScorePopUp>();

        Debug.Log("enemy start");
       
       
        EnemyStart(); 
       
    }

    public abstract void EnemyEnable();
    


    public abstract void EnemyStart();
  
    private void Update()
    {

        if (!controller.IsViewingNextTerrain)
        {
            Debug.Log("enemy updating");
            EnemyUpdate();
        }

    }


    public void InstantiatePopUp()
    {
        scorePopUp.inistantiateScorePop(transform.position,Quaternion.identity);
    }

    public abstract void EnemyUpdate();
    
    public void resetDeath()
    {
        alive = GetComponent<DeathChecker>();
        Debug.Log("ENEMY SPAWNING RESETTING DEATH alive is null " + (alive == null));
        alive.isAlive = true;
      
    }
    public void assignSpawnValues() 
    {
        
       
        if (spawnIntervals.Count > 0) {

            spawnInterval = spawnIntervals[Random.Range(0, spawnIntervals.Count)];
            Debug.Log("new spawn value " + spawnInterval);
            /*Debug.Log("SPAWNING ENEMY assigning new spawn interval " + spawnInterval);*/
        }

       

        
    
    
    } 

    
    public int ScoreIncrement
    {
        get { return scoreIncrement; }


    }

    public bool isAlive
    {
        get { return alive.isAlive; }
    }

}
