using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class EventObjectDescriptor : MonoBehaviour
{

    protected playerController playerController;
    //generice base class for objects to define information that exsists across all event objects 
    [SerializeField] private int amountThatCanSpawn;
    EventObject eventObject;
    protected Collider2D playerCol;

    public void Start()
    {
        playerController = FindAnyObjectByType<playerController>();
      
        playerCol = playerController.GetComponent<Collider2D>();
        GetComponent<EventObject>().EventStart(); 
        eventObject = GetComponent<EventObject>();
    }
    public void Update()
    {
        if (playerController.IsViewingNextTerrain && playerController.CurrentTerrain.containsPoint(transform.position)) // if the player has not started viewing the next terrain menaing that they cant see their current 
        {
            return;
        }
        eventObject.EventObjUpdate();
       

    }
    public int AmountThatSpawn 
    {
        get { return amountThatCanSpawn; }




    }


}
