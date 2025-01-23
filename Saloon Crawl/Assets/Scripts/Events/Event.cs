using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Event : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] protected List<GameObject> eventObjects;
    [SerializeField] protected float shouldFireTimer;
    [SerializeField] protected float timerRandomOffsetMin;
    [SerializeField] protected float timerRandomOffsetMax;
    

    private List<Event> attachedQueue;
    protected EventObjectPool objectPool;
    protected float currentShouldFireTimer = 0.0f;
    protected float eventTimer = 0.0f;
    protected bool hasFinished = false;
    protected playerController playerController;
    protected CamMovement playerCam;
    protected float alloactedWaitTime = 0.0f;


    void Start()
    {



        playerController = FindFirstObjectByType<playerController>();
        playerCam = FindFirstObjectByType<CamMovement>();

    }

    public abstract void Fire();

    public abstract bool HasFinished();

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
        if (!(eventTimer > currentShouldFireTimer))
        {
            eventTimer += Time.deltaTime;
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
    public List<Event> AttachedQueue
    {
        get { return attachedQueue; }
        set { attachedQueue = value; }
    }
  

}
