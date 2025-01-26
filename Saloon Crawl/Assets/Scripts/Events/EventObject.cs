using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EventObject : EventObjectDescriptor
{

    protected playerController playerController;

    protected Collider2D playerCol;
    private bool eventStarted = false;
    public void Start()
    {
        playerController = FindFirstObjectByType<playerController>();
       
        playerCol = playerController.GetComponent<Collider2D>();
        Debug.Log("EVENT event object start player controller is null " + (playerController == null));
      
        
    }

    public abstract void EventStart();
    public abstract void EventObjUpdate();
    public abstract void EventObjEnable();

}

