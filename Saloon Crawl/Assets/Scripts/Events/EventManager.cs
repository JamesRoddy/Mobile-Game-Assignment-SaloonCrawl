using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private List<GameObject> eventObjectHolders;

    List<TerrainEvent> events =  new List<TerrainEvent>();
    

    TerrainEvent currentEvent = null;
    private float genericCoolDown = 0.0f;
    private float genericCooldownMin = 5.0f;
    private float genericCooldDownMax = 10.0f;




    void Start()
    {

        foreach (GameObject obj in eventObjectHolders) 
        {
            EventObjectPool pool =  gameObject.AddComponent<EventObjectPool>();
            TerrainEvent currentEvent = obj.GetComponent<TerrainEvent>();
            pool.setValues(currentEvent.EventObjects); 
            currentEvent.ObjectPool = pool;

            currentEvent.AttachedQueue = events;
         
        
        }

        


    }

    // Update is called once per frame
    void Update()
    {
        checkForEvent();
        if ( currentEvent != null )
        {

            if (!currentEvent.HasFinished())
            {
                Debug.Log("event firing event is not null " + currentEvent != null); 

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
            currentEvent = events[0];
            currentEvent.EventStart();
            
            
        }



    }



}
