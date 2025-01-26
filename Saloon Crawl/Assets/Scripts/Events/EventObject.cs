using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EventObject : EventObjectDescriptor
{


    // generic event object class for event objects such as the beer bottle 
    // allows for generic istaniation of each event object pool without requring multiple different methods inn order to instaniate pools for each 
    // this means that the instantiation of event objects can be done in a single for loop

    protected playerController playerController;

    protected Collider2D playerCol;
    private bool eventStarted = false;
    public void Start()
    {
        playerController = FindFirstObjectByType<playerController>();
       
        playerCol = playerController.GetComponent<Collider2D>();
      
        
    }

    public abstract void EventStart();
    public abstract void EventObjUpdate();
    public abstract void EventObjEnable();

}

