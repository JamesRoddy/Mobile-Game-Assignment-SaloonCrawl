using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class TerrainEvent : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] protected List<GameObject> eventObjects;
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
    void Start()
    {

        terrainEvent = gameObject.GetComponent<TerrainEvent>(); 
        Debug.Log("terrain event is null "+gameObject.GetComponent<TerrainEvent>());    
        playerController = FindFirstObjectByType<playerController>();
        playerCam = FindFirstObjectByType<CamMovement>();
        currentShouldFireTimer += shouldFireTimer;
        Debug.Log("EVENTS initial fire timer " + currentShouldFireTimer);
    }
    public abstract void EventStart();
    public abstract void Fire();

    public abstract bool HasFinished();

    public void pushToEventQueue()
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
    public bool shouldFire()
    {

        if (alloactedWaitTime > 0.0f)
        {
            alloactedWaitTime -= Time.deltaTime;
            return false;
        }

        incrementTimeToEvent();


        return eventTimer > currentShouldFireTimer;


    }


  

    public void incrementTimeToEvent()
    {
        if (!(eventTimer > currentShouldFireTimer) && !isFiring)
        {
            
            eventTimer += Time.deltaTime;
            Debug.Log("event fire timer " + eventTimer);
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
