using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyDescriptorInfo : MonoBehaviour
{



    // generic abstarcted class used to store information about the enemies for the terrain manager and the pool manager for enemies and interactables 
    [SerializeField] protected List<float> spawnIntervals ;
    protected playerController controller;
    [SerializeField] protected int scoreIncrement;
    protected Transform scoreIncrementPopUp;
    private float spawnInterval = 0.0f;
    protected DeathChecker alive;
    protected InstaniateScorePopUp scorePopUp;

    
    public float SpawnInterval
    {
        get { return spawnInterval; }
        set { spawnInterval = value; }
    }

    private void Start()
    {
        controller =  FindFirstObjectByType<playerController>();
        scorePopUp = GetComponent<InstaniateScorePopUp>();

       
         

    

       
       
       
        EnemyStart(); 
       
    }

    public abstract void EnemyEnable();


    public IEnumerator EnemyEnableWait()
    {
        yield return new WaitForSeconds(0.5f);
        EnemyEnable();
    }
    public abstract void EnemyStart();
  
    private void Update()
    {

        if (!controller.IsViewingNextTerrain) // if the player has not started viewing the next terrain menaing that they cant see their current 
        {
            EnemyUpdate();
        }

    }


    

    public void InstantiatePopUp()
    {
        scorePopUp.inistantiateScorePop(transform.position,Quaternion.identity); // allows for a score symbol to appear on the enemy on death
    }

    public abstract void EnemyUpdate();
    
    public void resetDeath()
    {
        alive = GetComponent<DeathChecker>();
        alive.isAlive = true;
      
    }
    public void assignSpawnValues() 
    {
        
       
        if (spawnIntervals.Count > 0) {

            spawnInterval = spawnIntervals[Random.Range(0, spawnIntervals.Count)];
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
