using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeerBottleEvent : TerrainEvent
{


    private GameObject beerBottle;
    private EventObject beerBottleEventScript;


    public override void EventStart()
    {
      
        beerBottle = objectPool.requestAvaialbeObject();
        beerBottle.SetActive(true);
        beerBottleEventScript = beerBottle.GetComponent<EventObject>();
        isFiring = true;


    }

    public override void Fire() {

        beerBottleEventScript.EventObjUpdate();


    }

    public override bool HasFinished() {

        if(beerBottle.activeSelf == false)
        {
            isFiring = false;
            return true;
        }


        return false;    
    }

  

}
