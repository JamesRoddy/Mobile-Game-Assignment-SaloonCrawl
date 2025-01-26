using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EventObject : EventObjectDescriptor
{

    protected playerController playerController;
    protected Camera playerCam; 
    public void Start()
    {
        playerController = FindFirstObjectByType<playerController>();
        playerCam = FindFirstObjectByType<Camera>();
        Debug.Log("EVENT event object start player controller is null " + (playerController == null) + " camera is null " + (playerCam == null));
        EventStart();
        EventObjEnable();
    }

    public abstract void EventStart();
    public abstract void EventObjUpdate();
    public abstract void EventObjEnable();

}

