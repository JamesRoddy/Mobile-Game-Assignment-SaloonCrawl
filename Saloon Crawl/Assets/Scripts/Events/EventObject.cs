using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EventObject : MonoBehaviour
{

    playerController playerController;
    Camera playerCam; 
    public void Start()
    {
        playerController = FindFirstObjectByType<playerController>();
        playerCam = FindFirstObjectByType<Camera>();

        EventStart();
        EventObjEnable();
    }

    public abstract void EventStart();
    public abstract void EventObjUpdate();
    public abstract void EventObjEnable();

}

