using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class TerrainEvent : MonoBehaviour
{
    // Start is called before the first frame update

    // main base class for all event object holders 

    [SerializeField] protected List<GameObject> eventObjects; // used to assign a list of prefabs to the event that will be object pooled 
    [SerializeField] protected float shouldFireTimer;
    [SerializeField] protected float timerRandomOffsetMin;
    [SerializeField] protected float timerRandomOffsetMax;
    
    
    private List<TerrainEvent> attachedQueue;
    protected EventObjectPool objectPool;
    protected float currentShouldFireTimer = 0.0f;
    protected float eventTimer = 0.0f;
    protected bool hasFinished = false;
    protected playerController playerController;
    protected CamMovement playerCam;
    protected float alloactedWaitTime = 0.0f;
    protected bool isFiring = false;
    private TerrainEvent terrainEvent;
    protected Collider2D playerCol;
    void Start()
    {

        terrainEvent = gameObject.GetComponent<TerrainEvent>(); 
        playerController = FindFirstObjectByType<playerController>();
        playerCam = FindFirstObjectByType<CamMovement>();
        playerCol = playerController.GetComponent<Collider2D>();
        currentShouldFireTimer = shouldFireTimer;
        
    }
    public abstract void EventStart();
    public abstract void Fire();

    public abstract bool HasFinished();

    public void pushToEventQueue() // push to the main event manager queue 
    {

        if (!isFiring  && shouldFire() &&!attachedQueue.Contains(terrainEvent))
        {
            attachedQueue.Add(terrainEvent); 

            assignNewTriggerTime();
            eventTimer = 0.0f;
        }

    }

    private void Update()
    {
        pushToEventQueue();
    }
    public bool shouldFire() // increment event timers
    {

        if (alloactedWaitTime > 0.0f)
        {
            
            alloactedWaitTime -= Time.deltaTime;
            return false;
        }

        incrementTimeToEvent();


        return eventTimer > currentShouldFireTimer;


    }



    public abstract void EventEnable();
    
    public void incrementTimeToEvent()
    {
        if (!(eventTimer > currentShouldFireTimer) && !isFiring)
        {
            
            eventTimer += Time.deltaTime;
            return;
        }

       
       
            
        
        



    }


    public void assignNewTriggerTime()
    {

        currentShouldFireTimer = shouldFireTimer + Random.Range(timerRandomOffsetMin, timerRandomOffsetMax + 1.0f);

        
    }

    public EventObjectPool ObjectPool { 
        
        get { return objectPool; } 
        set { objectPool = value; }
    
    }
    public List<GameObject> EventObjects
    {

        get { return eventObjects; }
    }
    public float AllocatedWaitTime
    {
        set { alloactedWaitTime = value; }
    }
    public List<TerrainEvent> AttachedQueue
    {
        get { return attachedQueue; }
        set { attachedQueue = value; }
    }
  

}
