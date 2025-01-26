using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    // Start is called before the first frame update



    // central class for managing the events and when they generate 

    [SerializeField] private List<GameObject> eventObjectHolders;

    List<TerrainEvent> events =  new List<TerrainEvent>();
    
    
    TerrainEvent currentEvent = null;
    private float genericCoolDown = 0.0f;
    private float genericCooldownMin = 5.0f; // used to assign random offsets to the event timers of each event  
    private float genericCooldDownMax = 10.0f;




    void Start()
    {


        // intialise events assinging the object pool they need and give them access to the current event queue so the event objects can push their TerrainEvent compoenent to the list to be updated 
        foreach (GameObject obj in eventObjectHolders) 
        {
            EventObjectPool pool =  gameObject.AddComponent<EventObjectPool>();
            TerrainEvent currentEvent = obj.GetComponent<TerrainEvent>();
            // assigning the pool objects to the added eventObjectPool compoenent 
            pool.setValues(currentEvent.EventObjects); 
            currentEvent.ObjectPool = pool;

            
            currentEvent.AttachedQueue = events;
            currentEvent.EventStart(); // initialise event values 
         
        
        }

        


    }

    void Update()
    {
        checkForEvent();
        if ( currentEvent != null )
        {

            if (!currentEvent.HasFinished())
            {

                currentEvent.Fire();
                return;
            }

            genericCoolDown = Random.Range(genericCooldownMin, genericCooldDownMax);
            currentEvent.AllocatedWaitTime = genericCoolDown;

            events.Remove(currentEvent);
            currentEvent = null;


     

        }

        
   


        
    }



    private void checkForEvent()
    {

        
        if (events.Count > 0 && currentEvent ==null)
        {
            Debug.Log("EVENT event found");
            currentEvent = events[0];
            currentEvent.EventEnable();
            
            
        }



    }



}
